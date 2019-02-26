using System.IO;
using UnityEngine;
using UnityEditor;

public class CopyLuaScripts
{
    private static string m_sourcePath = Application.dataPath + "/LuaScripts/src";
    private static string m_targetPath = PathTools.GetABOutPath() + "/LUA";

    [MenuItem("HotFix/CopyLuaScripts")]
    public static void CopyLuaFile()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(m_sourcePath);
        FileInfo[] fileInfo = dirInfo.GetFiles();

        if(!Directory.Exists(m_targetPath))
        {
            Directory.CreateDirectory(m_targetPath);
        }

        foreach(var item in fileInfo)
        {
            File.Copy(item.FullName,m_targetPath+"/"+item.Name,true);
        }

        Debug.Log("lua 文件拷贝完成");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

}
