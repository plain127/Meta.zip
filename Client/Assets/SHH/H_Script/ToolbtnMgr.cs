using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가

public class ToolbtnMgr : MonoBehaviour
{
    public static ToolbtnMgr instance;

    // TextMeshPro Button을 참조
    public GameObject BUTTONtmp; // TextMeshPro Button을 할당해야 함 (GameObject 형태)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        // V 키를 눌렀을 때 BUTTONtmp 비활성화
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (BUTTONtmp != null)
            {
                BUTTONtmp.SetActive(false); // TMP 버튼 비활성화
            }
            else
            {
                Debug.LogError("BUTTONtmp가 할당되지 않았습니다. TextMeshPro Button을 Inspector에 연결하세요.");
            }
        }
    }

    public void Exitpanel()
    {
        // 현재 스크립트가 할당된 UI를 비활성화
        this.gameObject.SetActive(false);

        // Canvas_Inventory의 첫 번째 자식을 활성화
        GameObject inventoryUI = GameObject.Find("Canvas_Inventory").transform.GetChild(0).gameObject;
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(true);
        }
        else
        {
            Debug.LogError("Canvas_Inventory 또는 첫 번째 자식을 찾을 수 없습니다.");
        }
    }
}