using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapBookBtnMgr : MonoBehaviour
{
    public GameObject tool1;   //Tool1 UI

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // X��ư Ŭ����, Tool1 UI ��Ȱ��ȭ
    public void OnClickXTool1()
    {
        tool1.SetActive(false);

    }
}
