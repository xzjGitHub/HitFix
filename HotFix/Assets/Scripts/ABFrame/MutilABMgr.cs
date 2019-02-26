using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MutilABMgr
{

    private SingleABLoader m_currentSingleABLoader;

    //ab包缓存集合
    private Dictionary<string,SingleABLoader> m_singleABDict;

    //ab包与对应关系集合
    private Dictionary<string,ABRelation> m_relationDict;

    private string m_currentABName;

    //所有ab包加载完成
    private LoadComplete m_ABCompleteHandle;

    public MutilABMgr(string abName,LoadComplete loadCompleteHandle)
    {
        m_currentABName = abName;
        m_ABCompleteHandle = loadCompleteHandle;

        m_singleABDict = new Dictionary<string,SingleABLoader>();
        m_relationDict = new Dictionary<string,ABRelation>();
    }

    /// <summary>
    /// 加载指定ab包的回调
    /// </summary>
    /// <param name="abName"></param>
    private void CompleteLoadABCallBack(string abName)
    {
        if(abName.Equals(m_currentABName))
        {
            m_ABCompleteHandle(abName);
        }
    }

    public IEnumerator LoadAssetBundle(string abName)
    {
        //ab包依赖关系的建立
        if(!m_relationDict.ContainsKey(abName))
        {
            ABRelation tempRelation = new ABRelation(abName);
            m_relationDict.Add(abName,tempRelation);
        }
        ABRelation abRelation = m_relationDict[abName];

        //得到ab包所有的依赖关系(查询清单文件)
        string[] strDependenceArr = ABManifestLoader.Instance.GetAllDependices(abName);
        for(int i = 0; i < strDependenceArr.Length; i++)
        {
            //添加依赖项
            abRelation.AddDependence(strDependenceArr[i]);
            //添加引用项
            yield return LoadReferenceAB(strDependenceArr[i],abName);
        }

        //加载ab包
        if(m_singleABDict.ContainsKey(abName))
        {
            yield return m_singleABDict[abName].LoadAssetBundle();
        }
        else
        {
            m_currentSingleABLoader = new SingleABLoader(abName,CompleteLoadABCallBack);
            m_singleABDict.Add(abName,m_currentSingleABLoader);
            yield return m_currentSingleABLoader.LoadAssetBundle();
        }
    }

    /// <summary>
    /// 加载引用ab包
    /// </summary>
    /// <param name="abName">ab包名</param>
    /// <param name="refName">被引用ab包名称</param>
    /// <returns></returns>
    private IEnumerator LoadReferenceAB(string abName,string refName)
    {
        //ab包已经被加载
        if(m_relationDict.ContainsKey(abName))
        {
            ABRelation tempRelation = m_relationDict[abName];
            //添加ab包引用关系
            tempRelation.AddReference(refName);
        }
        else
        {
            ABRelation temp = new ABRelation(abName);
            temp.AddReference(refName);
            m_relationDict.Add(abName,temp);

            //开始加载依赖的包 
            yield return LoadAssetBundle(abName);
        }
    }

    /// <summary>
    /// 加载ab包中的资源
    /// </summary>
    /// <param name="abName">ab包名</param>
    /// <param name="assetName">资源名</param>
    /// <param name="isCache">是否需要缓存</param>
    /// <returns></returns>
    public UnityEngine.Object LoadAsset(string abName,string assetName,bool isCache)
    {
        if(m_singleABDict.ContainsKey(abName))
        {
            return m_singleABDict[abName].LoadAsset(assetName,isCache);
        }
        else
        {
            LogHelperLSK.LogError("加载资源出错,资源名: " + assetName + "  ab包名:" + abName);
            return null;
        }
    }

    //public bool HaveLoad(string abName)
    //{
    //    return m_singleABDict.ContainsKey(abName);
    //}

    /// <summary>
    /// 释放本场景中的所有资源
    /// </summary>
    public void DisposeAllAsset()
    {
        List<string> keys = new List<string>();
        keys.AddRange(m_singleABDict.Keys);
        try
        {
            for(int i = 0; i < keys.Count; i++)
            {
                m_singleABDict[keys[i]].DisposeAll();
            }
        }
        catch(Exception ex)
        {
            LogHelperLSK.LogError(ex);
        }
        finally
        {
            m_singleABDict.Clear();
            m_singleABDict = null;

            //释放其他的
            m_relationDict.Clear();
            m_relationDict = null;

            m_currentSingleABLoader = null;
            m_currentABName = null;
            m_ABCompleteHandle = null;

            //卸载没有使用到的资源
            Resources.UnloadUnusedAssets();

            //强制GC
            GC.Collect();
        }
    }

    /// <summary>
    /// 这是卸载ab包 卸载一个 需要处理其依赖关系
    /// </summary>
    /// <param name="abName"></param>
    public void DisposeBundle(string abName)
    {
        if (m_singleABDict.ContainsKey(abName))
        {
            ABRelation relation = m_relationDict[abName];
            List<string> def = relation.GetAllDependence();
            for (int i = 0; i < def.Count; i++)
            {
                if (m_relationDict.ContainsKey(def[i]))
                {
                    ABRelation temp = m_relationDict[def[i]];
                    if (temp.RemoveReference(abName))
                    {
                        DisposeBundle(temp.GetABName());
                    }
                }
            }
            if (relation.GetAllReference().Count == 0)
            {
                m_singleABDict[abName].Dispose();
                m_relationDict.Remove(abName);
            }
        }
    }

    /// <summary>
    /// 卸载所有的ab包 这里不需要处理依赖关系 直接干掉
    /// </summary>
    public void DisposeAllBundle()
    {
        List<string> keys = new List<string>();
        keys.AddRange(m_relationDict.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            m_singleABDict[keys[i]].Dispose();
        }
        m_relationDict.Clear();
    }

    /// <summary>
    /// 释放某个ab包里面的某个资源
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    public void DisposeResObj(string abName, string resName)
    {
        if (m_singleABDict.ContainsKey(abName))
        {
            m_singleABDict[abName].DisposeResObj(resName);
        }
        else
        {
            LogHelperLSK.LogError("卸载资源出错，包名: " + abName + " 资源名:" + resName);
        }
    }
}

