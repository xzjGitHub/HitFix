using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using UnityEditor;
using UnityEngine;

public class AutoMarkAssetBundle
{

    [MenuItem("ABTools/AutoMark")]
    public static void AutoMark()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();

        string needAutoMarkRoot = string.Empty;

        //根目录下 所有场景目录
        DirectoryInfo[] dirScenensDir = null;

        //需要打包资源的文件夹根目录
        needAutoMarkRoot = PathTools.GetABResourcesPath();
        //
        DirectoryInfo directoryInfo = new DirectoryInfo(needAutoMarkRoot);
        dirScenensDir = directoryInfo.GetDirectories();
        foreach(var currentDir in dirScenensDir)
        {
            string tempScenesDir = needAutoMarkRoot + "/" + currentDir.Name;
            int tempScenesIndex = tempScenesDir.LastIndexOf("/");
            string tempScenesName = tempScenesDir.Substring(tempScenesIndex + 1);

            JudgeDirOrFile(currentDir,tempScenesName);
        }

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 判断是否是目录与文件 然后修改标记
    /// </summary>
    /// <param name="currentInfo">当前文件（目录）信息 </param>
    /// <param name="scenesName">当前场景名称</param>
    private static void JudgeDirOrFile(FileSystemInfo fileSystemInfo,string scenesName)
    {
        if(!fileSystemInfo.Exists)
        {
            Debug.LogError("文件或者目录名称: " + fileSystemInfo + "不存在,请检查");
            return;
        }
        //得到当前目录下一级的文件信息集合
        DirectoryInfo dirInfo = fileSystemInfo as DirectoryInfo;
        FileSystemInfo[] fileSysArr = dirInfo.GetFileSystemInfos();
        foreach(FileSystemInfo fileInfo in fileSysArr)
        {
            FileInfo file = fileInfo as FileInfo;
            //文件类型
            if(file != null)
            {
                //修改此文件的ab标签
                SetFileAbLabel(file,scenesName);
            }
            //目录类型
            else
            {
                //递归调用
                JudgeDirOrFile(fileInfo,scenesName);
            }
        }
    }

    /// <summary>
    /// 设置AB包名称
    /// </summary>
    /// <param name="file"></param>
    /// <param name="scenesName"></param>
    private static void SetFileAbLabel(FileInfo file,string scenesName)
    {
        //不处理meta文件
        if(file.Extension == ".meta")
            return;

        int tempIndex = file.FullName.IndexOf("Assets");
        string assetFilePath = file.FullName.Substring(tempIndex);

        string abName = GetAbName(file,scenesName);

        AssetImporter assetImporter = AssetImporter.GetAtPath(assetFilePath);
        assetImporter.assetBundleName = abName;

        if(file.Extension == ".unity")
        {
            assetImporter.assetBundleVariant = "u3d";
        }
        else
        {
            assetImporter.assetBundleVariant = "ab";
        }
    }

    /// <summary>
    /// 得到ab包名称
    /// </summary>
    /// <param name="file"></param>
    /// <param name="scenesName"></param>
    /// <returns></returns>
    private static string GetAbName(FileInfo file,string scenesName)
    {
        string abName = string.Empty;

        //文件信息全路径 windows格式
        string tempWinPath = file.FullName;

        //转换成Unity的路径格式
        string tempUnityPath = tempWinPath.Replace("\\","/");

        //场景名称后面字符位置
        int tempSceneNamePos = tempUnityPath.IndexOf(scenesName) + scenesName.Length;

        string abFileNameArea = tempUnityPath.Substring(tempSceneNamePos + 1);


        if(abFileNameArea.Contains("/"))
        {
            string[] tempArr = abFileNameArea.Split('/');
            abName = scenesName + "/" + tempArr[0];
        }
        else
        {
            abName = scenesName + "/" + scenesName;
        }

        return abName;
    }
}

