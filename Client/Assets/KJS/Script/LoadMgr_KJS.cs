using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadMgr_KJS : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public GameObject imageBoxPrefab;
    public GameObject pagePrefab;
    public Scrollbar pageScrollbar;

    private int totalPages;

    public Transform parent;
    public Transform pagesParentTransform;

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    private string saveDirectory = Application.dataPath;
    private string saveFileName = "Magazine.json"; // Ȯ���ڸ� .json���� ����
    private string savePath;

    private RootObject rootData = new RootObject();

    private void Start()
    {
        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);

        if (File.Exists(savePath))
        {
            LoadDataFromJsonFile(); // JSON ���� �ε� �޼���� ����
        }
    }

    // Ư�� postId�� �´� �����͸� �ε��ϴ� �޼���
    public void LoadObjectsFromFile(string postId)
    {
        try
        {
            Debug.Log($"LoadObjectsFromFile() called for postId: {postId}");

            // postId�� ���� �ε� ��� ����
            string postDirectory = Path.Combine(saveDirectory, postId);
            string postLoadPath = Path.Combine(postDirectory, saveFileName);

            // �ش� ��ο� magazine.json ������ �ִ��� Ȯ��
            if (!File.Exists(postLoadPath))
            {
                Debug.LogWarning($"postId '{postId}'�� �ش��ϴ� ������ �����ϴ�: {postLoadPath}");
                return;
            }

            // ���Ͽ��� �����͸� �о�� JSON �Ľ�
            string json = File.ReadAllText(postLoadPath);
            rootData = JsonUtility.FromJson<RootObject>(json);
            Debug.Log("Data loaded from JSON file successfully.");

            // ������ postId�� �Խù��� ã��
            Post post = rootData.posts.Find(p => p.postId == postId);
            if (post == null)
            {
                Debug.LogWarning($"postId '{postId}'�� �ش��ϴ� �Խù��� �����ϴ�.");
                return;
            }

            // ���� UI ��� �ʱ�ȭ
            textBoxes.ForEach(Destroy);
            imageBoxes.ForEach(Destroy);
            pages.ForEach(Destroy);

            textBoxes.Clear();
            imageBoxes.Clear();
            pages.Clear();

            // �ε��� �����͸� �������� �������� ��� ����
            foreach (var page in post.pages)
            {
                GameObject newPageObj = Instantiate(pagePrefab, pagesParentTransform);
                AddPage(newPageObj);

                foreach (var element in page.elements)
                {
                    GameObject newObj = null;

                    if (element.type == Element.ElementType.Text_Box)
                    {
                        newObj = Instantiate(textBoxPrefab, newPageObj.transform);
                        TMP_Text textComponent = newObj.GetComponentInChildren<TMP_Text>();
                        if (textComponent != null)
                        {
                            textComponent.text = element.content;
                            textComponent.fontSize = element.fontSize;

                            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>($"Fonts/{element.fontFace}");
                            if (fontAsset != null) textComponent.font = fontAsset;

                            textComponent.fontStyle = FontStyles.Normal;
                            if (element.isUnderlined) textComponent.fontStyle |= FontStyles.Underline;
                            if (element.isStrikethrough) textComponent.fontStyle |= FontStyles.Strikethrough;
                        }
                        AddTextBox(newObj);
                    }
                    else if (element.type == Element.ElementType.Image_Box)
                    {
                        newObj = Instantiate(imageBoxPrefab, newPageObj.transform);
                        Image imageComponent = newObj.transform.GetChild(0).GetComponent<Image>();

                        if (element.imageData != null && element.imageData.Length > 0)
                        {
                            Texture2D texture = Element.DecodeImageFromBytes(element.imageData);
                            imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        }
                        AddImageBox(newObj);
                    }

                    if (newObj != null)
                    {
                        newObj.transform.localPosition = element.position;
                        newObj.transform.localScale = element.scale;
                    }
                }
            }

            // ������ ���� ������Ʈ
            totalPages = pages.Count;
            UpdateScrollbar();

            pageScrollbar.value = 0f;

            Debug.Log("Data loaded successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
        }
    }

    private void AddTextBox(GameObject textBox) => textBoxes.Add(textBox);
    private void AddImageBox(GameObject imageBox) => imageBoxes.Add(imageBox);
    private void AddPage(GameObject page) => pages.Add(page);

    private void UpdateScrollbar()
    {
        if (totalPages <= 1)
        {
            pageScrollbar.size = 1f;
            pageScrollbar.value = 1f;
            pageScrollbar.interactable = false;
        }
        else
        {
            pageScrollbar.size = 1f / totalPages;
            pageScrollbar.value = 1f;
            pageScrollbar.interactable = true;

            pageScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
        }

        pageScrollbar.value = 0f;
    }

    private void OnScrollbarValueChanged(float value)
    {
        float step = 1f / (totalPages - 1);
        int currentPage = Mathf.RoundToInt(value / step);
        float targetValue = currentPage * step;

        pageScrollbar.value = targetValue;

        Debug.Log($"Current Page: {currentPage + 1}/{totalPages}");
    }

    // JSON ���Ϸκ��� �����͸� �ε��ϴ� �޼���
    private void LoadDataFromJsonFile()
    {
        try
        {
            string json = File.ReadAllText(savePath);
            rootData = JsonUtility.FromJson<RootObject>(json);
            Debug.Log("Data loaded from JSON file successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data from JSON file: {e.Message}");
        }
    }
}