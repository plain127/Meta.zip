using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using System;
using TMPro;

[RequireComponent(typeof(PhotonView))]
public class InventoryUI : MonoBehaviourPun
{
    [Header("인벤토리")]
    public Transform slot;
    public RawImage img_Preview;        // 스크린샷 미리보기 화면
    public Button btn_SlotItemPrefab;   // 스크린샷 프리팹
    public Button btn_Delete;           // 스크린샷 삭제 버튼

    [Header("포스트잇")]
    public GameObject postItPrefab;     // 포스트잇 프리팹
    public Transform noticePos;         // 액자 위치
    public Button btn_PostIt;           // 포스트잇 생성 버튼
    int selectIndex = -1;
    Texture2D selectScreenshot;

    
    [Header("큰 이미지로 보기")]
    public GameObject largeImagePreviewPanel; // 큰 이미지 패널
    public RawImage largeImagePreview;        // 확대 이미지를 표시할 RawImage
    public Button closeButton;                // 큰 이미지 닫기

    public GameObject Colorbtn;

    public GameObject canvasLarge;
    
    private void Start()
    {
        // 지금 'Meta_ScrapBook_Scene'이라면, 
        if (SceneManager.GetActiveScene().name == "Meta_ScrapBook_Scene")
        {
            // 액자를 찾아라.
            noticePos = GameObject.Find("noticepos").transform;

            // 오브젝트 할당
            canvasLarge = GameObject.Find("Canvas_large");
            largeImagePreviewPanel = canvasLarge.transform.GetChild(0).gameObject;
            largeImagePreview = largeImagePreviewPanel.transform.GetChild(0).GetComponent<RawImage>();
            closeButton = largeImagePreviewPanel.transform.GetChild(1).GetComponent<Button>();
        }
        else
        {
            // 모든 오브젝트 null
            noticePos = null;
            canvasLarge = null;
            largeImagePreviewPanel = null;
            largeImagePreview = null;
            closeButton = null;
        }

        btn_Delete.interactable = false;
        btn_PostIt.interactable = false;

        if (largeImagePreviewPanel != null)
        {
            largeImagePreviewPanel.SetActive(false);
        }
        
        btn_PostIt.onClick.AddListener(() => OnPostitButtionClick());
        print("왜안돼");

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HideLargeImage);
        }
    }

    // 인벤토리 UI 업데이트
    public void UpdateInventoryUI()
    {
        // 기존 슬롯의 모든 자식 오브젝트 제거
        foreach (Transform child in slot)
        {
            Destroy(child.gameObject);
        }

        // 스크린샷 개수만큼 버튼 생성
        for (int i = 0; i < InventoryManager.instance.ScreenshotCount(); i++)
        {
            Button newButton = Instantiate(btn_SlotItemPrefab, slot); // 버튼 생성
            int index = i;

            // 버튼에 클릭 이벤트 추가
            newButton.onClick.AddListener(() => OnSlotClick(index));

            // 스크린샷 날짜 가져오기
            string screenshotPath = InventoryManager.instance.GetScreenshotPath(index);
            if (!string.IsNullOrEmpty(screenshotPath))
            {
                DateTime fileDate = File.GetCreationTime(screenshotPath); // 파일 생성 날짜
                string formattedDate = fileDate.ToString("MM월/dd일");    // 날짜 포맷

                // 버튼 위 텍스트 설정
                 TextMeshProUGUI tmpText = newButton.GetComponentInChildren<TextMeshProUGUI>();
                if (tmpText != null)
                {
                    tmpText.text = formattedDate; // TMP 텍스트에 날짜 설정
                }
            }
        }
    }
    public void OnClickColoroff()
    {
        Colorbtn.gameObject.SetActive(false);
    }
    public void OnClickColoron()
    {
        Colorbtn.gameObject.SetActive(true);
    }

    public void RPC_getpostit()
    {
        photonView.RPC("OnSlotClick", RpcTarget.MasterClient);
    }

    // 스크린샷 클릭하면 발동하는 함수
    [PunRPC]
    public void OnSlotClick(int index)
    {
        selectIndex = index;

        string screenshotPath = InventoryManager.instance.GetScreenshotPath(selectIndex);

        if (!string.IsNullOrEmpty(screenshotPath))
        {
            // RPC로 모든 클라이언트에 이미지 로드를 요청
            photonView.RPC("RPC_DisplayScreenshot", RpcTarget.All, screenshotPath);

            // UI 버튼 상태 업데이트
            btn_Delete.interactable = true;
            btn_PostIt.interactable = true;
        }

        btn_Delete.onClick.AddListener(() => OnSlotDeleteClick(selectIndex));

      // PicketUIManager 객체를 안전하게 체크하고 호출
      PicketUIManager picketUIManager = FindObjectOfType<PicketUIManager>();
      if (picketUIManager != null)
      {
          picketUIManager.SetURL(index);
      }
      else
      {
          Debug.LogWarning("PicketUIManager가 씬에 존재하지 않습니다. SetURL을 호출하지 않았습니다.");
      }
    }
    public void RPC_deletepostit()
    {
        photonView.RPC("OnSlotDeleteClick", RpcTarget.MasterClient);
    }

    [PunRPC]
    // 스크린샷 삭제 버튼 기능
    public void OnSlotDeleteClick(int index)
    {
        InventoryManager.instance.DeleteScreenshot(index);

        img_Preview.texture = null;

        btn_Delete.interactable = false;
        btn_PostIt.interactable = false;

        selectIndex = -1;

        UpdateInventoryUI();
    }
    public void RPC_displaypostit()
    {
        photonView.RPC("DisplayScreenshot", RpcTarget.All);
    }
    [PunRPC]
    public void RPC_DisplayScreenshot(string path)
    {
        // Coroutine으로 이미지 로드
        StartCoroutine(LoadImage(path));
    }

    public IEnumerator LoadImage(string path)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///" + path);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            img_Preview.texture = texture;
            selectScreenshot = texture;

            //ShowLargeImage(texture);
        }
        else
        {
            Debug.LogError("이미지 로드 실패: " + request.error);
        }
    }

    [PunRPC]
    // 정렬을 위한 위치 값 설정
    private int postItColumnCount;      // 한 행에 들어갈 포스트잇 개수 (3으로 설정)
    private int postItRowCount;         // 총 행 개수 (3으로 설정)
    //private float postItSpacing = 8f;      // 포스트잇 간 간격 (픽셀 단위)
    private int postItIndex = 0;            // 생성된 포스트잇의 인덱스

    public Material matPostIt;
    public Color postItColor = Color.white;
    public void SetPostItColor(Color color)
    {
        postItColor = color;
    }
    [PunRPC]
    public void OnPostitButtionClick()
    {
        if (selectScreenshot != null)
        {

            Vector3 spawnPosition = GetNewPosition();
            GameObject newPostIt = Instantiate(postItPrefab, spawnPosition, Quaternion.identity, noticePos);
            newPostIt.transform.localScale = new Vector3(18, 12, 2);
            newPostIt.GetComponent<H_MergePostIt>().noticepos = noticePos;

            // 색상 및 텍스처 설정
            Material mat = new Material(matPostIt);
            mat.color = postItColor;
            newPostIt.GetComponent<MeshRenderer>().material = mat;
            
            RawImage postItImage = newPostIt.GetComponentInChildren<RawImage>();
            if (postItImage != null)
            {
                postItImage.texture = selectScreenshot;
            }
         
            // 새 포스트잇에 클릭 리스너 추가하여 이미지를 확대함
            Button postItButton = newPostIt.GetComponentInChildren<Button>();
            Texture2D currentScreenshot = selectScreenshot;
            postItButton.onClick.AddListener(() => ShowLargeImage(currentScreenshot));

            // 포스트잇 인덱스 증가
            postItIndex++;
            if (postItIndex >= postItColumnCount * postItRowCount)
            {
                postItIndex = 0;
            }

            // 버튼 인터랙션 초기화
            btn_Delete.interactable = false;
            btn_PostIt.interactable = false;
        }
    }
    private Vector3 GetNewPosition()
    {

        // 랜덤 위치 범위 설정
        float xOffset = UnityEngine.Random.Range(-5f, 5f); // X축 이동 범위
        float yOffset = UnityEngine.Random.Range(-5f, 5f); // Y축 이동 범위
        float zOffset = -0.5f;                              // 고정된 Z축

        return noticePos.position + new Vector3(xOffset, yOffset, zOffset);
    }
    [PunRPC]
    public void ShowLargeImage(Texture2D screenshot)
    {
        // 큰 이미지 UI 패널에 텍스처 설정
        largeImagePreview.texture = screenshot;
        largeImagePreviewPanel.SetActive(true);
    }

    // 큰 이미지 닫기
    [PunRPC]
    public void HideLargeImage()
    {
        // 큰 이미지 UI 패널 비활성화
        largeImagePreviewPanel.SetActive(false);
    }
}
