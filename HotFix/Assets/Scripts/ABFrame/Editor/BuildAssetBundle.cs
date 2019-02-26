
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class BuildAssetBundle
{
    [MenuItem("ABTools/BuildAssetBundles/Windnows")]
    public static void BuildAllAB()
    {
        //打包AB输出路径
        string strABOutPathDIR = string.Empty;
        //获取"StreamingAssets"数值
        strABOutPathDIR = Application.streamingAssetsPath + "/" + "Windows";
        //判断生成输出目录文件夹
        if(Directory.Exists(strABOutPathDIR))
        {
            Directory.Delete(strABOutPathDIR,true);
        }
        Directory.CreateDirectory(strABOutPathDIR);
        //打包生成
        BuildPipeline.BuildAssetBundles(strABOutPathDIR,BuildAssetBundleOptions.None,EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    [MenuItem("ABTools/BuildAssetBundles/Android")]
    public static void BuildAllABAndroid()
    {
        //打包AB输出路径
        string strABOutPathDIR = string.Empty;
        //获取"StreamingAssets"数值
        strABOutPathDIR = Application.streamingAssetsPath + "/" + "Android";
        
        //判断生成输出目录文件夹
        if (Directory.Exists(strABOutPathDIR))
        {
            Directory.Delete(strABOutPathDIR,true);
        }
        Directory.CreateDirectory(strABOutPathDIR);
        //打包生成
        BuildPipeline.BuildAssetBundles(strABOutPathDIR, BuildAssetBundleOptions.None, BuildTarget.Android);
        AssetDatabase.Refresh();
    }
}

