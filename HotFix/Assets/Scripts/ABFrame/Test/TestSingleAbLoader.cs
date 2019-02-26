using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleAbLoader: MonoBehaviour
{
    private SingleABLoader m_abLoader;

    private string m_abName = "scenes_1/prefab.ab";

    private string m_assetName = "Cube.prefab";

    private void Start()
    {
        m_abLoader = new SingleABLoader(m_abName,LoadComplate);
        StartCoroutine(m_abLoader.LoadAssetBundle());
    }


    private void LoadComplate(string abName)
    {
        LogHelperLSK.Log(abName + " 加载完成");

        GameObject obj = m_abLoader.LoadAsset(m_assetName,false) as GameObject;
        GameObject temp = GameObject.Instantiate(obj);

        string[] str = m_abLoader.RetiveAllAssetName();
        for(int i = 0; i < str.Length; i++)
        {
            LogHelperLSK.LogError(str[i]);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            m_abLoader.Dispose();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            m_abLoader.DisposeAll();
        }
    }

}
