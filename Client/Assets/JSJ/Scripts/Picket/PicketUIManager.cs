using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PicketUIManager : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject player;
    public GameObject playerPrefabPos;   // Player Charater
    public GameObject currentPicket;   // 방금 생성된 Picket Prefab
    public GameObject selectPicket;   // 현재 선택된 Picket Prefab

    [Header("Picket UI")]
    public GameObject panelInventory;   // 인벤토리 UI
    public GameObject panelLinkNews;   // Picket이랑 기사 링크 여부 Panel
    public GameObject panelPicket;   // Picket UI

    [Header("Button")]
    public Button btn_Yes;   // 링크 'Yes'
    public Button btn_No;   // 링크 'No'
    public Button btn_X;

    [Header("Screenshot")]
    public RawImage img_News;   // 기사 스크린샷을 보여주는 이미지

    [Header("Inventory Data")]
    public List<string> screenshotList; // InventoryManager에서 가져온 스크린샷 경로 리스트

    public bool isShowPicketUI = false;

    public DrawWithMouse drawWithMouse;


    void Start()
    {
        player = FindObjectOfType<GameManager>().player;
        playerPrefabPos = player.transform.GetChild(1).gameObject;

        if (player != null)
        {
            panelInventory = player.transform.Find("Canvas_Inventory/Panel_Inventory")?.gameObject;
        }

        // InventoryManager에서 스크린샷 데이터 가져오기
        if (InventoryManager.instance != null)
        {
            screenshotList = InventoryManager.instance.GetScreenshotList();
        }

        // UI에 스크린샷 데이터 적용
        if (screenshotList != null && screenshotList.Count > 0)
        {
            Debug.Log("스크린샷 데이터 로드 완료!");
            foreach (string screenshot in screenshotList)
            {
                Debug.Log("스크린샷 경로: " + screenshot);
            }
        }
        else
        {
            Debug.LogWarning("스크린샷 데이터가 없습니다.");
        }

        // 버튼 이벤트 등록
        btn_Yes.onClick.AddListener(OnClickYesBtn);
        btn_No.onClick.AddListener(OnClickNoBtn);
        btn_X.onClick.AddListener(OnClickXBtn);

        // UI 비활성화
        panelInventory.SetActive(false);
        panelLinkNews.SetActive(false);
        panelPicket.SetActive(false);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            
            if (Physics.Raycast(ray, out hitInfo, 50, 1 << 18))
            {
                print(hitInfo.collider.name);
                // 만일, 피켓을 클릭했다면
                if (hitInfo.collider.gameObject.layer == 18)
                {
                    // 클릭한 Picket 변수에 담기
                    selectPicket = hitInfo.collider.gameObject;

                    ShowPicketUI();
                }
            }
        }
    }

    
    public void ShowPicketUI()
    {
        isShowPicketUI = true;

        // Picket UI 활성화
        panelPicket.SetActive(true);

        // 라인 오브젝트 활성화
        drawWithMouse.lineParent.SetActive(true);
        // 스티커 오브젝트 활성화
        drawWithMouse.stickerParent.SetActive(true);

        // Picket에 연결된 스크린샷 경로로 newsTexture 설정
        LoadNewsTextureFromPicket();
    }

    public void LoadNewsTextureFromPicket()
    {
        if (currentPicket != null)
        {
            // PicketId_KJS 컴포넌트에서 스크린샷 경로 가져오기
            PicketId_KJS picketIdKJS = selectPicket.GetComponent<PicketId_KJS>();
            if (picketIdKJS != null)
            {
                string screenshotPath = picketIdKJS.GetScreenshotPath();

                if (!string.IsNullOrEmpty(screenshotPath) && File.Exists(screenshotPath))
                {
                    byte[] fileData = File.ReadAllBytes(screenshotPath);
                    Texture2D texture = new Texture2D(2, 2);
                    if (texture.LoadImage(fileData))
                    {
                        img_News.texture = texture;
                        Debug.Log("스크린샷이 성공적으로 로드되었습니다: " + screenshotPath);
                    }
                    else
                    {
                        Debug.LogError("텍스처 로드 실패: " + screenshotPath);
                    }
                }
                else
                {
                    Debug.LogError("스크린샷 경로가 유효하지 않거나 파일이 존재하지 않습니다: " + screenshotPath);
                }
            }
            else
            {
                Debug.LogError("currentPicket에 PicketId_KJS 컴포넌트가 없습니다.");
            }
        }
    }

    public void SetURL(int index)
    {
        currentPicket.GetComponent<PicketId_KJS>().SetScreenshotPath(screenshotList[index]);
    }


    // 질문에 'Yes'버튼을 눌렀을 때
    public void OnClickYesBtn()
    {
        if (panelLinkNews.activeSelf)
        {
            // 질문 UI 비활성화
            panelLinkNews.SetActive(false);

            // 인벤토리 UI 활성화
            panelInventory.SetActive(true);
        }
    }

    // 질문에 'No'버튼을 눌렀을 때
    public void OnClickNoBtn()
    {
        if (panelLinkNews.activeSelf)
        {
            // 질문 UI 비활성화
            panelLinkNews.SetActive(false);

            // 방금 생성된 Picket 제거
            Destroy(currentPicket);
        }
    }

    // X 버튼을 눌렀을 때
    public void OnClickXBtn()
    {
        // 질문 UI 비활성화
        panelLinkNews.SetActive(false);

        // Picket UI 비활성화
        panelPicket.SetActive(false);

        // 라인 오브젝트 비활성화
        drawWithMouse.lineParent.SetActive(false);
        // 스티커 오브젝트 비활성화
        drawWithMouse.stickerParent.SetActive(false);

        drawWithMouse.isDrawing = false;
        drawWithMouse.isAttaching = false;

        // 커서 아이콘 초기화
        drawWithMouse.ResetCursor();
    }
}