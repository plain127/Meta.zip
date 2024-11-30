using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class H_PostInfo
{
    public string postid;
    public string thumburl;
}

[System.Serializable]
public class PostInfoList
{
    public List<H_PostInfo> postData = new List<H_PostInfo>();
}

public class PostMgr : MonoBehaviour
{
    public List<H_PostInfo> allPost = new List<H_PostInfo>();
    public GameObject prefabfactory;
    public GameObject content;
    public GameObject MagCanvas;
    public GameObject btn_Pos;

    private LoadMgr_KJS loadManager;

    List<Button> btns = new List<Button>();
    public Button btn_Exit;

    private string baseDirectory = Application.dataPath;

    void Start()
    {
        GameObject editorManagerObj = GameObject.Find("EditorManager");
        if (editorManagerObj != null)
        {
            loadManager = editorManagerObj.GetComponent<LoadMgr_KJS>();
            if (loadManager == null)
            {
                Debug.LogError("EditorManager ������Ʈ�� LoadMgr_KJS ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("EditorManager ������Ʈ�� ã�� �� �����ϴ�.");
        }

        ThumStart();
    }

    public void ThumStart()
    {
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
                    catch (System.Exception e)
                    {
                        Debug.LogError($"JSON ������ �ε��ϴ� ���� ������ �߻��߽��ϴ�: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"���� {postDirectory}�� thumbnail.json ������ �����ϴ�.");
                }
            }
        }
        else
        {
            Debug.LogError($"�⺻ ���丮�� �������� �ʽ��ϴ�: {baseDirectory}");
        }

        CreatePostThumbnails();
    }

    private void CreatePostThumbnails()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        btns.Clear();

        for (int i = 0; i < allPost.Count; i++)
        {
            GameObject go = Instantiate(prefabfactory, content.transform);
            PostThumb post = go.GetComponent<PostThumb>();
            Button bu = go.GetComponent<Button>();
            btns.Add(bu);

            string postId = allPost[i].postid;
            btns[i].onClick.RemoveAllListeners();
            btns[i].onClick.AddListener(() => OnClickMagContent(postId));

            post.SetInfo(allPost[i]);
        }

        if (MagCanvas)
        {
            MagCanvas.SetActive(false);
        }
    }

    public void OnClickMagContent(string postId)
    {
        if (!MagCanvas.activeSelf)
        {
            MagCanvas.SetActive(true);
        }

        if (loadManager != null)
        {
            loadManager.LoadObjectsFromFile(postId);
        }
        else
        {
            Debug.LogError("loadManager�� null�Դϴ�. LoadMgr_KJS�� ã�� �� �����ϴ�.");
        }
    }

    public void OnClickExit()
    {
        if (MagCanvas.activeSelf)
        {
            MagCanvas.SetActive(false);
        }
    }
}