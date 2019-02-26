using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;


public class TestAbFrame: MonoBehaviour
{
    public static TestAbFrame Instance;

    public Image image;

    private string m_sceneName= "scene_mainui";

    private string m_abName= "scene_mainui/Prefab.ab";

    private string m_assetName= "Cube.prefab";

    private void Awake()
    {
        Instance = this;

        StartCoroutine(ABManifestLoader.Instance.LoadManifestFile());
    }

    private void Start()
    {
        StartCoroutine(AssetBundleMgr.Instance.LoadAssetBundle("scene_comon","scene_comon/needloadsprite.ab",LoadCompleteCallBack1));

        StartCoroutine(AssetBundleMgr.Instance.LoadAssetBundle(m_sceneName,m_abName,LoadCompleteCallBack));
    }


    private void LoadCompleteCallBack(string abName)
    {
        LogHelperLSK.LogError("加载完成: "+abName);

        GameObject temp = AssetBundleMgr.Instance.LoadAsset(m_sceneName,m_abName,m_assetName,false) as GameObject;
        GameObject obj = GameObject.Instantiate(temp);
        obj.transform.SetParent(this.transform);

    }

    private void LoadCompleteCallBack1(string abName)
    {
        LogHelperLSK.LogError("加载完成: "+abName);
        image.sprite= AssetBundleMgr.Instance.LoadAsset("scene_comon","scene_comon/needloadsprite.ab","Equipment_40.png",false) as Sprite;
    }


    public void UnloadTrue()
    {
        AssetBundleMgr.Instance.DisposeAllAsset("scene_comon");
        AssetBundleMgr.Instance.DisposeAllAsset("scene_mainui");
    }

    public void UnLoadFalse()
    {
        AssetBundleMgr.Instance.DisposeAllBundleByScene("scene_comon");
        AssetBundleMgr.Instance.DisposeAllBundleByScene("Scene_MainUI");
    }

    public void FreeCube()
    {
        AssetBundleMgr.Instance.DisposeResObj("scene_mainui",m_abName,m_assetName);
       // AssetBundleMgr.Instance.DisposeResObj("Scene_MainUI","scene_comon/needloadsprite.ab","Equipment_40.png");
    }
}

