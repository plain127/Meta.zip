using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Thumbnail_KJS : MonoBehaviour
{
    public Button saveButton;   // ���� ��ư (Inspector���� ����)
    public Button loadButton;   // �ε� ��ư (Inspector���� ����)
    public Image targetImage;   // ������ �̹����� �Ҵ�� Image ������Ʈ (Inspector���� �Ҵ�)
    public TMP_InputField postIdInputField; // ����� �Է��� ���� postId�� ������ InputField (Inspector���� �Ҵ�)

    private PostInfoList postInfoList = new PostInfoList();  // �� ����Ʈ�� �ʱ�ȭ
    private string saveDirectory = Application.dataPath;

    void Start()
    {
        // ���� ��ư ����
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveImageAndData);  // ���� ��ư Ŭ�� �� �̹����� �����͸� ����
        }
        else
        {
            Debug.LogError("saveButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // �ε� ��ư ����
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(LoadImageAndData);  // �ε� ��ư Ŭ�� �� �̹����� �����͸� �ε�
        }
        else
        {
            Debug.LogError("loadButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (postIdInputField == null)
        {
            Debug.LogError("postIdInputField�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� postIdInputField�� �����ϼ���.");
        }
    }

    // �̹����� JSON �����͸� �����ϴ� �޼���
    void SaveImageAndData()
    {
        if (targetImage == null || targetImage.sprite == null || targetImage.sprite.texture == null)
        {
            Debug.LogError("������ �̹����� �������� �ʾҰų� �ؽ�ó�� �����ϴ�.");
            return;
        }

        if (string.IsNullOrWhiteSpace(postIdInputField.text))
        {
            Debug.LogError("postId�� �Է��ϼ���. postIdInputField�� ��� �ֽ��ϴ�.");
            return;
        }

        // postId�� ���� ��� ����
        string postId = postIdInputField.text;
        string postDirectory = Path.Combine(saveDirectory, postId);
        string jsonFilePath = Path.Combine(postDirectory, "thumbnail.json");

        // postId�� ���丮�� ������ ����
        if (!Directory.Exists(postDirectory))
        {
            Directory.CreateDirectory(postDirectory);
            Debug.Log($"Directory created at: {postDirectory}");
        }

        string fileName = "UserImage_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string imagePath = Path.Combine(postDirectory, fileName);

        // �̹��� ������ ���ÿ� ����
        SaveImageToLocal(targetImage.sprite.texture, imagePath);

        // ���� JSON ������ ������ �ε��Ͽ� postInfoList�� ������Ʈ
        if (File.Exists(jsonFilePath))
        {
            string existingJson = File.ReadAllText(jsonFilePath);
            postInfoList = JsonUtility.FromJson<PostInfoList>(existingJson);
        }
        else
        {
            postInfoList = new PostInfoList(); // ���ο� JSON ������ ���� ����Ʈ �ʱ�ȭ
        }

        // ������ postId�� �����ϸ� ���� �׸��� �����
        H_PostInfo existingPost = postInfoList.postData.Find(post => post.postid == postId);
        if (existingPost != null)
        {
            existingPost.thumburl = imagePath; // ���� �׸��� thumburl ������Ʈ
        }
        else
        {
            // ������ postId�� ���� ��� �� �׸� �߰�
            H_PostInfo newPost = new H_PostInfo
            {
                postid = postId,
                thumburl = imagePath
            };
            postInfoList.postData.Add(newPost);  // ����Ʈ ����Ʈ�� �߰�
        }

        SaveJsonToLocal(jsonFilePath);  // postId�� ������ JSON ����
    }

    private void SaveImageToLocal(Texture2D texture, string path)
    {
        byte[] pngData = texture.EncodeToPNG();
        if (pngData == null)
        {
            Debug.LogError("�̹����� PNG �������� ���ڵ��ϴµ� �����߽��ϴ�.");
            return;
        }

        try
        {
            File.WriteAllBytes(path, pngData);
            Debug.Log($"�̹����� ���ÿ� {path} ��η� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"���ÿ� �̹��� ���� �� ���� �߻�: {e.Message}");
        }
    }

    private void SaveJsonToLocal(string jsonFilePath)
    {
        try
        {
            string json = JsonUtility.ToJson(postInfoList, true);
            File.WriteAllText(jsonFilePath, json);
            Debug.Log($"JSON �����Ͱ� ���ÿ� {jsonFilePath} ��η� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"���ÿ� JSON ���� �� ���� �߻�: {e.Message}");
        }
    }

    // JSON ���Ͽ��� Ư�� postId�� �ش��ϴ� �����͸� �ε��ϰ� �̹����� ǥ��
    void LoadImageAndData()
    {
        if (string.IsNullOrWhiteSpace(postIdInputField.text))
        {
            Debug.LogError("postId�� �Է��ϼ���. postIdInputField�� ��� �ֽ��ϴ�.");
            return;
        }

        string postId = postIdInputField.text;
        string postDirectory = Path.Combine(saveDirectory, postId);
        string jsonFilePath = Path.Combine(postDirectory, "thumbnail.json");

        // JSON ���� ���� ���� Ȯ��
        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError($"JSON ������ {jsonFilePath} ��ο� �������� �ʽ��ϴ�.");
            return;
        }

        try
        {
            // JSON ���� �б�
            string json = File.ReadAllText(jsonFilePath);
            postInfoList = JsonUtility.FromJson<PostInfoList>(json);

            // �Էµ� postId�� ��ġ�ϴ� �׸� ã��
            H_PostInfo postInfo = postInfoList.postData.Find(post => post.postid == postId);
            if (postInfo == null)
            {
                Debug.LogError("�ش� postId�� ���� �׸��� ã�� �� �����ϴ�.");
                return;
            }

            // �ش� �̹��� �ε� �� ǥ��
            string imagePath = postInfo.thumburl;
            if (File.Exists(imagePath))
            {
                byte[] imageData = File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(imageData))
                {
                    targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    Debug.Log($"�̹����� {imagePath} ��ο��� �ε�Ǿ����ϴ�.");
                }
                else
                {
                    Debug.LogError("�̹��� �����͸� �ε��ϴ� �� �����߽��ϴ�.");
                }
            }
            else
            {
                Debug.LogError("�ش� ��ο� �̹��� ������ �����ϴ�.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON ������ �ε��ϴ� ���� ������ �߻��߽��ϴ�: {e.Message}");
        }
    }
}