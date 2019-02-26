using UnityEngine;

public class PathTools
{

    public const string AB_RESOURCES = "AB_Res";  

    /// <summary>
    /// 得到AB资源的输入目录
    /// </summary>
    /// <returns></returns>
    public static string GetABResourcesPath()
    {
        return Application.dataPath + "/" + AB_RESOURCES;
    }

    /// <summary>
    /// 获取AB输出路径
    /// </summary>
    public static string GetABOutPath()
    {
        return Application.streamingAssetsPath + "/" + GetPlatformName();
    }

    /// <summary>
    /// 获取平台的名称
    /// </summary>
    /// <returns></returns>
    public static string GetPlatformName()
    {
        string strReturnPlatformName = string.Empty;

        switch(Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                strReturnPlatformName = "Windows";
                break;
            case RuntimePlatform.IPhonePlayer:
                strReturnPlatformName = "Iphone";
                break;
            case RuntimePlatform.Android:
                strReturnPlatformName = "Android";
                
                break;
        }

        return strReturnPlatformName;
    }


    /// <summary>
    /// 获取WWW协议下载（AB包）路径
    /// </summary>
    /// <returns></returns>
    public static string GetWWWPath()
    {
        //返回路径字符串
        string url = string.Empty;

        switch(Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                url = "file://" + Application.streamingAssetsPath + "/" + "Windows";
                break;
            case RuntimePlatform.Android:
                url = Application.streamingAssetsPath + "/" + "Android";
                break;
        }

        return url;
    }

}



