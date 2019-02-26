using System.Collections.Generic;
using System.IO;
using UnityEditor;


/// <summary>
/// 创建校验文件
/// 针对AB包 资源文件 生成MD5
/// </summary>
public class CreatVerifyFiles
{

    [MenuItem("HotFix/CreatVerifyFile")]
    public static void CreatFileMethod()
    {
        //ab输出路径
        string abOutPath = PathTools.GetABOutPath();
        //校验文件路径
        string verifyFile = abOutPath + "/"+HotFixTool.VerifyFileName;

        //存储所有合法文件的相对路径信息
        List<string> fileList = new List<string>();

        //如果有这个文件 先删除
        if(File.Exists(verifyFile))
        {
            File.Delete(verifyFile);
        }

        ListFiles(new DirectoryInfo(abOutPath),ref fileList);
        WriteVerifyFile(verifyFile,fileList,abOutPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    //遍历文件 得到所有合法文件
    private static void ListFiles(FileSystemInfo fileSysInfo,ref List<string> fileList)
    {
        DirectoryInfo dirInfo = fileSysInfo as DirectoryInfo;
        FileSystemInfo[] fileSysInfos = dirInfo.GetFileSystemInfos();
        for(int i = 0; i < fileSysInfos.Length; i++)
        {
            FileInfo fileInfo = fileSysInfos[i] as FileInfo;
            if(fileInfo != null)
            {
                //把Win系统中路径分割符改为Unity的类型
                string fileFullName = fileInfo.FullName.Replace("\\","/");

                //过滤无效文件
                string fileExt = Path.GetExtension(fileFullName);
                if(fileExt.EndsWith(".meta") || fileExt.EndsWith(".back"))
                {
                    continue;
                }

                fileList.Add(fileFullName);
            }
            else
            {
                ListFiles(fileSysInfos[i],ref fileList);
            }
        }
    }

    //把文件的名称和MD5写入校验文件
    private static void WriteVerifyFile(string filePath,List<string> fileList,string abOutPath)
    {
        FileStream fs = new FileStream(filePath,FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for(int i = 0; i < fileList.Count; i++)
        {
            string fileName = fileList[i].Replace(abOutPath + "/",string.Empty);
            string md5 = Util.GetMd5(fileList[i]); ;
            sw.WriteLine(fileName + "|" + md5);
        }

        sw.Close();
        fs.Close();
    }
}
