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

    // X버튼 클릭시, Tool2 UI 비활성화
    public void OnClickXTool2()
    {
        tool2.SetActive(false);
        print("@@@");

    }
}
