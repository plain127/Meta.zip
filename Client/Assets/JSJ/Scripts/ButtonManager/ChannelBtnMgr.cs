using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelBtnMgr : MonoBehaviour
{
    public GameObject tool2;   //Tool2 UI

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // X��ư Ŭ����, Tool2 UI ��Ȱ��ȭ
    public void OnClickXTool2()
    {
        tool2.SetActive(false);
        print("@@@");

    }
}
