using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABManifestLoader: IDisposable
{

    private static ABManifestLoader m_instance;
    public static ABManifestLoader Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = new ABManifestLoader();
            }
            return m_instance;
        }
    }

    //系统类
    private AssetBundleManifest m_manifest;

    //manifest的路径
    private string m_manifestPath;

    //读取manifest清单文件的AssetBundle
    private AssetBundle m_abReadManifest;

    //是否已经加载完成
    private bool m_isLoadFnish;
    public bool IsLoadFnish
    {
        get { return m_isLoadFnish; }
    }

    private ABManifestLoader()
    {
        m_manifestPath = PathTools.GetWWWPath() + "/" + PathTools.GetPlatformName();
        m_manifest = null;
        m_abReadManifest = null;
        m_isLoadFnish = false;
    }


    public IEnumerator LoadManifestFile()
    {
        using(WWW www = new WWW(m_manifestPath))
        {
            yield return www;
            if(www.progress >= 1)
            {
                //加载完成 
                AssetBundle temp = www.assetBundle;
                if(temp != null)
                {
                    m_abReadManifest = temp;
                    //读取到Manifest
                    m_manifest = m_abReadManifest.LoadAsset(ABDefine.AssetBundleManifest) as AssetBundleManifest;
                    m_isLoadFnish = true;
                }
                else
                {
                    LogHelperLSK.LogError("加载Manifest出错,请检查manifestPath: " + m_manifestPath + "  www error: " + www.error);
                }
            }
        }
    }

    /// <summary>
    /// 获取AssetBundleManifest
    /// </summary>
    /// <returns></returns>
    public AssetBundleManifest GetAssetBundleManifest()
    {
        if(m_isLoadFnish)
        {
            if(m_manifest != null)
            {
                return m_manifest;
            }
            else
            {
                LogHelperLSK.LogError("m_manifest is null");
            }
        }
        else
        {
            LogHelperLSK.Log("manifest 加载尚未完成");
        }
        return null;
    }


    /// <summary>
    /// 获取指定ab包所有依赖关系
    /// </summary>
    /// <param name="abName">ab包名</param>
    /// <returns></returns>
    public string[] GetAllDependices(string abName)
    {
        if(m_manifest != null && !string.IsNullOrEmpty(abName))
        {
            return m_manifest.GetAllDependencies(abName);
        }
        else
        {
            LogHelperLSK.LogError("m_manifest is null");
        }
        return null;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        if(m_abReadManifest != null)
        {
            //清空这个清单所在的包的内存资源和镜像资源
            m_abReadManifest.Unload(true);
        }
    }
}


