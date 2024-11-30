using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InventoryText_KJS : MonoBehaviour
{
    public List<H_PostInfo> allPost = new List<H_PostInfo>(); // ��� post ������ ����
    public GameObject prefabfactory;                         // ��ư ������
    public Transform inventoryPanel;                         // ��ư�� ��ġ�� �θ�
    public GameObject MagCanvas;                             // �� ������ ǥ���� ĵ����

    private LoadMgr_KJS loadManager;                         // �ܺο��� �����͸� �ε��� �Ŵ���

    private string baseDirectory = Application.dataPath;     // �⺻ ���� ���
    private List<Button> btns = new List<Button>();          // ������ ��ư ����Ʈ

    void Start()
    {
        // EditorManager���� LoadMgr_KJS ������Ʈ�� ã���ϴ�.
        GameObject editorManagerObj = GameObject.Find("EditorManager");
        if (editorManagerObj != null)
        {
            loadManager = editorManagerObj.GetComponent<LoadMgr_KJS>();
            if (loadManager == null)
            {
                Debug.LogError("EditorManager ������Ʈ�� LoadMgr_KJS ������Ʈ�� �����ϴ�.");
            }
        }
        // MagCanvas�� MagazineView 2�� �ڽ� �߿��� Tool 2�� �Ҵ�
        Transform magazineView = GameObject.Find("MagazineView 2")?.transform;
        if (magazineView != null)
        {
            Transform tool2 = magazineView.Find("Tool 2");
            if (tool2 != null)
            {
                MagCanvas = tool2.gameObject;
            }
        }

        // �ʱ�ȭ �۾�
        ThumStart();
    }

    public void ThumStart()
    {
        // postId���� ����� �������� JSON ������ �н��ϴ�.
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

        // �ε��� �����͸� �������� UI ����
        CreatePostThumbnails();
    }

    private void CreatePostThumbnails()
    {
        // �о�� ��� post ������ ������� ��ư�� ����
        for (int i = 0; i < allPost.Count; i++)
        {
            // ��ư ������ ����
            GameObject go = Instantiate(prefabfactory, inventoryPanel);

            // ��ư ������Ʈ�� ������
            Button button = go.GetComponent<Button>();
            btns.Add(button);

            string postId = allPost[i].postid;

            // ��ư Ŭ�� �̺�Ʈ �߰�
            btns[i].onClick.AddListener(() => OnClickMagContent(postId));

            // ��ư�� ����� �̹����� ����
            StartCoroutine(SetButtonImage(go, allPost[i].thumburl));
        }

        // MagCanvas�� ������ ��Ȱ��ȭ
        if (MagCanvas)
        {
            MagCanvas.SetActive(false);
        }
    }

    private IEnumerator SetButtonImage(GameObject buttonObject, string imageUrl)
    {
        // URL���� �̹����� �ٿ�ε�
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///" + imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // �ؽ�ó�� Sprite�� ��ȯ
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            // ��ư�� Image ������Ʈ�� ������ ����
            Image buttonImage = buttonObject.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = sprite;
            }
        }
    }

    public void OnClickMagContent(string postId)
    {
        // Canvas_Inventory�� ù ��° �ڽ��� ��Ȱ��ȭ
        GameObject inventoryUI = GameObject.Find("Canvas_Inventory").transform.GetChild(0).gameObject;
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }

        // �� ���� UI Ȱ��ȭ
        if (MagCanvas != null)
        {
            MagCanvas.SetActive(true);
        }

        // LoadMgr_KJS �Ŵ����� ���� �ش� postId�� �����͸� �ε�
        if (loadManager != null)
        {
            loadManager.LoadObjectsFromFile(postId);
        }
    }
}