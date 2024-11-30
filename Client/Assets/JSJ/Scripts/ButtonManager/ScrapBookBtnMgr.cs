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

    // X버튼 클릭시, Tool1 UI 비활성화
    public void OnClickXTool1()
    {
        tool1.SetActive(false);

    }
}
