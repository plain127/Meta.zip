using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InventoryText_KJS : MonoBehaviour
{
    public List<H_PostInfo> allPost = new List<H_PostInfo>(); // 모든 post 정보를 저장
    public GameObject prefabfactory;                         // 버튼 프리팹
    public Transform inventoryPanel;                         // 버튼을 배치할 부모
    public GameObject MagCanvas;                             // 상세 정보를 표시할 캔버스

    private LoadMgr_KJS loadManager;                         // 외부에서 데이터를 로드할 매니저

    private string baseDirectory = Application.dataPath;     // 기본 저장 경로
    private List<Button> btns = new List<Button>();          // 생성된 버튼 리스트

    void Start()
    {
        // EditorManager에서 LoadMgr_KJS 컴포넌트를 찾습니다.
        GameObject editorManagerObj = GameObject.Find("EditorManager");
        if (editorManagerObj != null)
        {
            loadManager = editorManagerObj.GetComponent<LoadMgr_KJS>();
            if (loadManager == null)
            {
                Debug.LogError("EditorManager 오브젝트에 LoadMgr_KJS 컴포넌트가 없습니다.");
            }
        }
        // MagCanvas를 MagazineView 2의 자식 중에서 Tool 2로 할당
        Transform magazineView = GameObject.Find("MagazineView 2")?.transform;
        if (magazineView != null)
        {
            Transform tool2 = magazineView.Find("Tool 2");
            if (tool2 != null)
            {
                MagCanvas = tool2.gameObject;
            }
        }

        // 초기화 작업
        ThumStart();
    }

    public void ThumStart()
    {
        // postId별로 저장된 폴더에서 JSON 파일을 읽습니다.
        if (Directory.Exists(baseDirectory))
        {
            string[] postDirectories = Directory.GetDirectories(baseDirectory);

            foreach (string postDirectory in postDirectories)
            {
                string jsonFilePath = Path.Combine(postDirectory, "thumbnail.json");

                if (File.Exists(jsonFilePath))
                {
                    try
                    {
                        string json = File.ReadAllText(jsonFilePath);
                        PostInfoList postInfoList = JsonUtility.FromJson<PostInfoList>(json);
                        allPost.AddRange(postInfoList.postData);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        // 로드한 데이터를 바탕으로 UI 생성
        CreatePostThumbnails();
    }

    private void CreatePostThumbnails()
    {
        // 읽어온 모든 post 정보를 기반으로 버튼을 생성
        for (int i = 0; i < allPost.Count; i++)
        {
            // 버튼 프리팹 생성
            GameObject go = Instantiate(prefabfactory, inventoryPanel);

            // 버튼 컴포넌트를 가져옴
            Button button = go.GetComponent<Button>();
            btns.Add(button);

            string postId = allPost[i].postid;

            // 버튼 클릭 이벤트 추가
            btns[i].onClick.AddListener(() => OnClickMagContent(postId));

            // 버튼에 썸네일 이미지를 설정
            StartCoroutine(SetButtonImage(go, allPost[i].thumburl));
        }

        // MagCanvas가 있으면 비활성화
        if (MagCanvas)
        {
            MagCanvas.SetActive(false);
        }
    }

    private IEnumerator SetButtonImage(GameObject buttonObject, string imageUrl)
    {
        // URL에서 이미지를 다운로드
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///" + imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // 텍스처를 Sprite로 변환
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            // 버튼의 Image 컴포넌트를 가져와 설정
            Image buttonImage = buttonObject.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = sprite;
            }
        }
    }

    public void OnClickMagContent(string postId)
    {
        // Canvas_Inventory의 첫 번째 자식을 비활성화
        GameObject inventoryUI = GameObject.Find("Canvas_Inventory").transform.GetChild(0).gameObject;
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }

        // 상세 보기 UI 활성화
        if (MagCanvas != null)
        {
            MagCanvas.SetActive(true);
        }

        // LoadMgr_KJS 매니저를 통해 해당 postId의 데이터를 로드
        if (loadManager != null)
        {
            loadManager.LoadObjectsFromFile(postId);
        }
    }
}