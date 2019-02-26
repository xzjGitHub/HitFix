using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SingleABLoader: IDisposable
{

    private AsetLoader m_asetLoader;

    private string m_abName;

    private string m_abDownLoadPath;

    private LoadComplete m_loadCompleteHandle;

    public SingleABLoader(string abName,LoadComplete loadComplete)
    {
        m_asetLoader = null;

        m_abName = abName;

        m_loadCompleteHandle = loadComplete;

        m_abDownLoadPath = PathTools.GetWWWPath() + "/" + m_abName;

        LogHelperLSK.Log("下载路径是:  " + m_abDownLoadPath);
    }

    /// <summary>
    /// ab包下载
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadAssetBundle()
    {
        using(WWW www = new WWW(m_abDownLoadPath))
        {
            yield return www;

            //下载完成
            if(www.progress >= 1)
            {
                LogHelperLSK.Log("加载完成:  " + m_abDownLoadPath);

                if(!string.IsNullOrEmpty(www.error))
                    LogHelperLSK.LogError("www加载ab出错，路径是: " + m_abDownLoadPath + "错误： " + www.error);

                //获取assetBundle的实例
                AssetBundle abObj = www.assetBundle;
                if (abObj != null)
                {
                    m_asetLoader = new AsetLoader(abObj);

                    //下载完成  调用委托
                    if (m_loadCompleteHandle != null)
                    {
                        m_loadCompleteHandle(m_abName);
                    }
                }
                else
                {
                    LogHelperLSK.LogError("AssetBundle is null:  " + m_abDownLoadPath);
                }
            }
            else
            {
                LogHelperLSK.LogError("www下载失败,下载路径是:  " + m_abDownLoadPath +
                    "    www.error: " + www.error);
            }
        }
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="isCache">是否缓存</param>
    /// <returns></returns>
    public UnityEngine.Object LoadAsset(string assetName,bool isCache)
    {
        if(m_asetLoader != null)
        {
            return m_asetLoader.LoadAsset(assetName,isCache);
        }
        else
        {
            LogHelperLSK.LogError("加载资源出错，m_assetLoader is null: " + assetName);
            return null;
        }
    }

    /// <summary>
    /// 卸载指定资源
    /// </summary>
    /// <param name="assetObj"></param>
    public void UnLoadAsset(UnityEngine.Object assetObj)
    {
        if(m_asetLoader != null)
        {
            m_asetLoader.UnLoadAsset(assetObj);
        }
        else
        {
            LogHelperLSK.LogError("卸载资源出错 m_assetLoader is null ");
        }
    }

    /// <summary>
    /// 释放当前镜像资源 还要释放内存资源
    /// </summary>
    public void DisposeAll()
    {
        if(m_asetLoader != null)
        {
            m_asetLoader.DisposeAll();
            m_asetLoader = null;
        }
        else
        {
            LogHelperLSK.LogError("释放资源出错(DisposeAll) m_assetLoader is null ");
        }
    }

    /// <summary>
    /// /释放资源
    /// </summary>
    public void Dispose()
    {
        if(m_asetLoader != null)
        {
            m_asetLoader.Dispose();
        }
        else
        {
            LogHelperLSK.LogError("释放资源出错(Dispose) m_assetLoader is null ");
        }
    }

    /// <summary>
    /// 释放某个资源
    /// </summary>
    /// <param name="resName"></param>
    public void DisposeResObj(string resName)
    {
        if(m_asetLoader != null)
        {
            m_asetLoader.DisposeResObj(resName);
        }
        else
        {
            LogHelperLSK.LogError("释放资源出错(DisposeResObj) m_assetLoader is null ");
        }
    }

    /// <summary>
    /// 查询包含得所有资源名称
    /// </summary>
    /// <returns></returns>
    public string[] RetiveAllAssetName()
    {
        if(m_asetLoader != null){
            return m_asetLoader.RetiveAllAssetName();
        }
        else
        {
            LogHelperLSK.LogError(" m_assetLoader is null ");
            return null;
        }
    }
}

