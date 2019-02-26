using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


public class ABRelation
{

    private string m_abName;

    private List<string> m_allDependiceAB;

    private List<string> m_allReferenceAB;

    public ABRelation(string abName)
    {
        m_abName = abName;
        m_allDependiceAB = new List<string>();
        m_allReferenceAB = new List<string>();
    }

    //-------------------依赖-------------------
    public void AddDependence(string abName)
    {
        if(!m_allDependiceAB.Contains(abName))
        {
            m_allReferenceAB.Add(abName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="abName"></param>
    /// <returns>true: 没用依赖项 false: 还有依赖项</returns>
    public bool RemoveDependence(string abName)
    {
        if(m_allDependiceAB.Contains(abName))
        {
            m_allDependiceAB.Remove(abName);
        }
        return m_allDependiceAB.Count <= 0;
    }

    /// <summary>
    /// 获取所有依赖项
    /// </summary>
    /// <returns></returns>
    public List<string> GetAllDependence()
    {
        return m_allDependiceAB;
    }


    //-------------引用-----------------
    public void AddReference(string abName)
    {
        if(!m_allReferenceAB.Contains(abName))
        {
            m_allReferenceAB.Add(abName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="abName"></param>
    /// <returns>true：没有其他引用项 false: 还有其他引用项</returns>
    public bool RemoveReference(string abName)
    {
        if(m_allReferenceAB.Contains(abName))
        {
            m_allReferenceAB.Remove(abName);
        }
        return m_allReferenceAB.Count <= 0;
    }

    public List<string> GetAllReference()
    {
        return m_allReferenceAB;
    }

    public string GetABName()
    {
        return m_abName;
    }
}

