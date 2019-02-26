using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGame: MonoBehaviour
{

    public static TestGame Ins;

    private UpdateResFromServer m_update;

    private void Awake()
    {
        Ins = this;
        m_update = gameObject.GetComponent<UpdateResFromServer>();
        m_update.Init();
    }

    public void Test()
    {
        Debug.Log("更新完成");
    }
}
