using System.Collections.Generic;
using XLua;
using System.IO;


public class LuaHelper: Singleton<LuaHelper>
{
    private LuaEnv m_luaEnv = new LuaEnv();

    private Dictionary<string,byte[]> m_luaFileDict = new Dictionary<string,byte[]>();

    private LuaHelper()
    {
        m_luaEnv.AddLoader(CustomLoader);
    }

    private byte[] CustomLoader(ref string fileName)
    {
        string luaPath = PathTools.GetABOutPath() + "/Lua";
        if(m_luaFileDict.ContainsKey(fileName))
        {
            return m_luaFileDict[fileName];
        }
        return ProcessDir(new DirectoryInfo(luaPath),fileName);
    }

    private byte[] ProcessDir(FileSystemInfo fileSysInfo,string fileName)
    {
        DirectoryInfo dirInfo = fileSysInfo as DirectoryInfo;
        FileSystemInfo[] files = dirInfo.GetFileSystemInfos();
        foreach(var item in files)
        {
            FileInfo file = item as FileInfo;
            if(file == null)
            {
                ProcessDir(item,fileName);
            }
            else
            {
                string temp = item.Name.Split('.')[0];//aa.lua
                if(item.Extension == ".meta" || temp != fileName)
                {
                    continue;
                }
                byte[] bytes = File.ReadAllBytes(file.FullName);
                m_luaFileDict.Add(fileName,bytes);
                return bytes;
            }
        }
        return null;
    }

    public void DoString(string chunk,string chunkName = "chunk",LuaTable table = null)
    {
        m_luaEnv.DoString(chunk,chunkName,table);
    }

    public object[] CallLuaFunc(string luaScript,string luaMethodName,params object[] args)
    {
        LuaTable table = m_luaEnv.Global.Get<LuaTable>(luaScript);
        LuaFunction func = table.Get<LuaFunction>(luaMethodName);
        return func.Call(args);
    }


}

