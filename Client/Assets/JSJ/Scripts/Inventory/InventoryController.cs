using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class InventoryController : MonoBehaviourPun
{
    public GameObject inventoryPanel;   // 인벤토리 창
    
    public Button btn_ScreenshotInven;  // 스크린샷 인벤토리 버튼
    public Button btn_TextInven;        // 기사 인벤토리 버튼

    public GameObject screenshotBG;     // 스크린샷 인벤토리 화면
    public GameObject textBG;           // 기사 인벤토리 화면

    void Start()
    {
        // 버튼 기능 추가
        btn_ScreenshotInven.onClick.AddListener(() => OnClickScreenshotInven());
        btn_TextInven.onClick.AddListener(() => OnClickTextInven());
    }

    void Update()
    {
        // 내 것이고, 키보드 key 숫자1키를 누를 때마다 인벤토리 UI Toggle
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    // 스크린샷 인벤토리 버튼 기능
    public void OnClickScreenshotInven()
    {
        // 스크린샷 인벤토리 화면 활성화
        screenshotBG.SetActive(true);
        // 기사 인벤토리 화면 비활성화
        textBG.SetActive(false);
    }

    // 기사 인벤토리 버튼 기능
    public void OnClickTextInven()
    {
        // 스크린샷 인벤토리 화면 비활성화
        screenshotBG.SetActive(false);
        // 기사 인벤토리 화면 활성화
        textBG.SetActive(true);
    }
}
