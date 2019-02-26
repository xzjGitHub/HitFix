using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AssetBundleMgr: Singleton<AssetBundleMgr>
{

    private Dictionary<string,MutilABMgr> m_allScenes;

    private AssetBundleMgr()
    {
        m_allScenes = new Dictionary<string,MutilABMgr>();
    }

    //给Lua使用
    public void LoadAssetBundleByLua(string sceneName,string abName,LoadComplete loadComplete)
    {
        //test todo
        TestGame.Ins.StartCoroutine(LoadAssetBundle(sceneName,abName,loadComplete));
    }


    /// <summary>
    /// 加载指定场景中的ab包
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="abName">ab包名</param>
    /// <param name="loadComplete">加载完成回调</param>
    /// <returns></returns>
    public IEnumerator LoadAssetBundle(string sceneName,string abName,LoadComplete loadComplete)
    {
        while(!ABManifestLoader.Instance.IsLoadFnish)
        {
            yield return null;
        }
        //   m_mainfest = ABManifestLoader.Instance.GetAssetBundleManifest();
        if(!m_allScenes.ContainsKey(sceneName))
        {
            MutilABMgr temp = new MutilABMgr(abName,loadComplete);
            m_allScenes.Add(sceneName,temp);
        }
        MutilABMgr mutilABMgr = m_allScenes[sceneName];
        yield return mutilABMgr.LoadAssetBundle(abName);
    }

    /// <summary>
    /// 加载指定场景中指定包中的指定资源
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="abName"></param>
    /// <param name="assetName"></param>
    /// <param name="isCache"></param>
    /// <returns></returns>

    public UnityEngine.Object LoadAsset(string sceneName,string abName,string assetName,bool isCache)
    {
        if(m_allScenes.ContainsKey(sceneName))
        {
            MutilABMgr mutilABMgr = m_allScenes[sceneName];
            return mutilABMgr.LoadAsset(abName,assetName,isCache);
        }
        else
        {
            LogHelperLSK.LogError("加载资源出错,不存在sceneName: " + sceneName);
        }
        return null;
    }


    /// <summary>
    /// 卸载某个场景种的某个ab包
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="abName"></param>
    public void DisposeBundleByScene(string sceneName,string abName)
    {
        if(m_allScenes.ContainsKey(sceneName))
        {
            m_allScenes[sceneName].DisposeBundle(abName);
        }
        else
        {
            LogHelperLSK.LogError("加载资源出错,不存在sceneName: " + sceneName);
        }
    }

    /// <summary>
    /// 释放某个场景种的所有ab包 
    /// </summary>
    /// <param name="sceneName"></param>
    public void DisposeAllBundleByScene(string sceneName)
    {
        if(m_allScenes.ContainsKey(sceneName))
        {
            m_allScenes[sceneName].DisposeAllBundle();
        }
        else
        {
            LogHelperLSK.LogError("加载资源出错,不存在sceneName: " + sceneName);
        }
    }

    public void DisposeAllBundle()
    {
        List<string> keys = new List<string>();
        keys.AddRange(m_allScenes.Keys);
        for(int i = 0; i < keys.Count; i++)
        {
            DisposeAllBundleByScene(keys[i]);
        }
        m_allScenes.Clear();
    }


    /// <summary>
    /// 释放一个场景中的所有资源
    /// </summary>
    /// <param name="sceneName"></param>
    public void DisposeAllAsset(string sceneName)
    {
        if(m_allScenes.ContainsKey(sceneName))
        {
            m_allScenes[sceneName].DisposeAllAsset();
            m_allScenes.Remove(sceneName);
        }
        else
        {
            LogHelperLSK.LogError("DisposeAllAsset资源出错,不存在sceneName: " + sceneName);
        }
    }

    /// <summary>
    /// 释放单个资源
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="abName">ab包名</param>
    /// <param name="resName">资源名</param>
    public void DisposeResObj(string sceneName,string abName,string resName)
    {
        if(m_allScenes.ContainsKey(sceneName))
        {
            m_allScenes[sceneName].DisposeResObj(abName,resName);
        }
        else
        {
            LogHelperLSK.LogError("DisposeResObj 出错,不存在sceneName: " + sceneName);
        }
    }
}

