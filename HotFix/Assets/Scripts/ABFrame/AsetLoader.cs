using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class AsetLoader:IDisposable
{

    private AssetBundle m_currentAssetBundle;

    private Dictionary<string, UnityEngine.Object> m_dict;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="ab">给定www加载的ab</param>
    public AsetLoader(AssetBundle ab)
    {
        if (ab != null)
        {
            m_currentAssetBundle = ab;
            m_dict =new Dictionary<string, UnityEngine.Object>();
        }
        else
        {
            LogHelperLSK.LogError("AssetLoader 构造函数 ab is null");
            //LogHelperLSK
        }
    }

    /// <summary>
    /// 加载指定的资源
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <param name="isCache">是否需要缓存</param>
    /// <returns></returns>
    public UnityEngine.Object LoadAsset(string assetName,bool isCache=false)
    {
        return LoadResource<UnityEngine.Object>(assetName,isCache);
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <param name="isCache">是否需要缓存</param>
    /// <returns></returns>
    private T LoadResource<T>(string assetName,bool isCache = false) where T : UnityEngine.Object
    {
        //先判断是否已经缓存
        if (m_dict.ContainsKey(assetName))
        {
            return m_dict[assetName] as T;
        }

        T temp = m_currentAssetBundle.LoadAsset<T>(assetName);
        if (temp!=null && isCache)
        {
            m_dict[assetName] = temp;
        }
        else if (temp == null)
        {
            
            LogHelperLSK.LogError(GetType() + "/ 获取资源为空，资源名： "+assetName);
        }
        return temp;
    }

    /// <summary>
    /// 卸载指定资源
    /// </summary>
    /// <param name="asset"></param>
    /// <returns></returns>
    public bool UnLoadAsset(UnityEngine.Object asset)
    {
        if (asset != null)
        {
            Resources.UnloadAsset(asset);
            return true;
        }
        else
        {
            LogHelperLSK.LogError(GetType() + "要卸载得资源为空");
            return false;
        }
    }


    /// <summary>
    /// 释放当前镜像资源 还要释放内存资源
    /// </summary>
    public void DisposeAll()
    {
        m_dict.Clear();
        m_currentAssetBundle.Unload(true);
    }

    /// <summary>
    /// 释放内存镜像资源
    /// </summary>
    public void Dispose()
    {
        m_currentAssetBundle.Unload(false);
    }

    /// <summary>
    /// 释放某个资源
    /// </summary>
    /// <param name="resName"></param>
    public void DisposeResObj(string resName)
    {
        if(m_dict.ContainsKey(resName))
        {
            Resources.UnloadAsset((GameObject)m_dict[resName]);
            m_dict.Remove(resName);
        }
    }

    /// <summary>
    /// 查询包含得所有资源名称
    /// </summary>
    /// <returns></returns>
    public string[] RetiveAllAssetName()
    {
        return m_currentAssetBundle.GetAllAssetNames();
    }

}

