using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Element
{
    public string content;
    public enum ElementType { Text_Box, Image_Box }
    public ElementType type;
    public byte[] imageData; // string ��� byte[]�� ����
    public Vector3 position;
    public Vector3 scale;

    public int fontSize;
    public string fontFace;
    public bool isUnderlined;
    public bool isStrikethrough;

    public Element(
        ElementType type, string content, byte[] imageData, Vector3 position, Vector3 scale,
        int fontSize = 14, string fontFace = "Arial", bool isUnderlined = false, bool isStrikethrough = false)
    {
        this.type = type;
        this.content = content;
        this.imageData = imageData;
        this.position = position;
        this.scale = scale;
        this.fontSize = fontSize;
        this.fontFace = fontFace;
        this.isUnderlined = isUnderlined;
        this.isStrikethrough = isStrikethrough;
    }

    public static byte[] EncodeImageToBytes(Texture2D texture)
    {
        return texture != null ? texture.EncodeToPNG() : null;
    }

    public static Texture2D DecodeImageFromBytes(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0) return null;
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        return texture;
    }
}

[System.Serializable]
public class Page
{
    public int pageId;
    public List<Element> elements = new List<Element>();

    public Page(int id)
    {
        pageId = id;
    }
}

[System.Serializable]
public class Post
{
    public string postId;
    public List<Page> pages = new List<Page>();

    public Post(string id)
    {
        postId = id;
    }
}

[System.Serializable]
public class RootObject
{
    public List<Post> posts = new List<Post>();
}

public class SaveMgr_KJS : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public GameObject imageBoxPrefab;
    public GameObject pagePrefab;
    public Scrollbar pageScrollbar;
    public ToolMgr_KJS toolManager;
    public EditorMgr_KJS editorMgr;
    public ImageMgr_KJS imageMgr;

    public int totalPages = 1;

    public Transform parent;
    public Transform pagesParentTransform;

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    public Button saveButton;
    public List<Button> loadButtons = new List<Button>();

    private string saveDirectory = Application.dataPath;
    private string saveFileName = "Magazine.json";
    private string savePath;

    private RootObject rootData = new RootObject();
    public CreateMgr_KJS createMgr;

    public TMP_InputField inputPostIdField;
    public TMP_InputField loadPostIdField;
    private bool isUpdatingScrollbar = false;
    public GameObject successUI; // ���� ���� UI�� Inspector���� �Ҵ� �����ϵ��� �ۺ� �ʵ� �߰�


    private void Start()
    {
        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);

        saveButton.onClick.RemoveAllListeners();
        saveButton.onClick.AddListener(SaveObjectsToFile);

        EnsureDirectoryExists();

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            rootData = JsonUtility.FromJson<RootObject>(json);
        }
        else
        {
            Directory.CreateDirectory(saveDirectory);
        }
        if (successUI != null)
        {
            successUI.SetActive(false);
        }
    }

    public void SetLoadButton(Button button)
    {
        loadButtons.Add(button);
        Debug.Log($"��ư {button.name}�� �߰��Ǿ����ϴ�.");
    }

    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
            Debug.Log($"Directory created at: {saveDirectory}");
        }
    }

    public void AddTextBox(GameObject textBox) => textBoxes.Add(textBox);
    public void AddImageBox(GameObject imageBox) => imageBoxes.Add(imageBox);
    public void AddPage(GameObject page)
    {
        pages.Add(page); // pages ����Ʈ�� ������ �߰�
        totalPages = pages.Count; // totalPages�� pages.Count�� ������Ʈ
        Debug.Log($"Page added. Current totalPages: {totalPages}");
    }

    public void RemovePage(GameObject page)
    {
        pages.Remove(page); // pages ����Ʈ���� ������ ����
        totalPages = pages.Count; // totalPages�� pages.Count�� ������Ʈ
        Debug.Log($"Page removed. Current totalPages: {totalPages}");
    }

    private void SaveObjectsToFile()
    {
        string targetPostId = inputPostIdField.text;

        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("��ȿ�� postId�� �Է��ϼ���.");
            return;
        }

        try
        {
            // postId�� ���� ���� ��� ����
            string postDirectory = Path.Combine(saveDirectory, targetPostId);
            string postSavePath = Path.Combine(postDirectory, saveFileName);

            // �ش� postId�� ������ ������ ����
            if (!Directory.Exists(postDirectory))
            {
                Directory.CreateDirectory(postDirectory);
                Debug.Log($"Directory created at: {postDirectory}");
            }

            // ������ postId�� �̹� �ִ��� Ȯ��
            Post existingPost = rootData.posts.Find(post => post.postId == targetPostId);

            // ������ ID�� �Խù��� �ִٸ� �����, ������ ���� ����
            Post targetPost = existingPost ?? new Post(targetPostId);
            if (existingPost == null)
            {
                rootData.posts.Add(targetPost);
            }
            else
            {
                // ���� �Խù� ������ ���ϴ�.
                targetPost.pages.Clear();
            }

            // ��ȿ�� textBox�� imageBox�� ���͸�
            textBoxes.RemoveAll(item => item == null);
            imageBoxes.RemoveAll(item => item == null);
            pages.RemoveAll(item => item == null);

            // ������ �� ��� ����
            for (int i = 0; i < pages.Count; i++)
            {
                Page newPage = new Page(i);

                foreach (var textBox in textBoxes)
                {
                    if (textBox.transform.parent != pages[i].transform) continue;

                    TMP_Text textComponent = textBox.GetComponentInChildren<TMP_Text>();
                    if (textComponent == null) continue;

                    string content = textComponent.text;
                    int fontSize = (int)textComponent.fontSize;
                    string fontFace = textComponent.font.name;
                    bool isUnderlined = textComponent.fontStyle.HasFlag(FontStyles.Underline);
                    bool isStrikethrough = textComponent.fontStyle.HasFlag(FontStyles.Strikethrough);

                    newPage.elements.Add(new Element(
                        Element.ElementType.Text_Box,
                        content,
                        null,
                        textBox.transform.localPosition,
                        textBox.transform.localScale,
                        fontSize,
                        fontFace,
                        isUnderlined,
                        isStrikethrough
                    ));
                }

                foreach (var imageBox in imageBoxes)
                {
                    if (imageBox.transform.parent != pages[i].transform) continue;

                    Image imageComponent = imageBox.transform.GetChild(0).GetComponent<Image>();
                    byte[] imageData = null;

                    if (imageComponent != null && imageComponent.sprite != null)
                    {
                        Texture2D texture = imageComponent.sprite.texture;
                        imageData = Element.EncodeImageToBytes(texture);
                    }

                    newPage.elements.Add(new Element(
                        Element.ElementType.Image_Box,
                        "",
                        imageData,
                        imageBox.transform.localPosition,
                        imageBox.transform.localScale
                    ));
                }

                targetPost.pages.Add(newPage);
            }

            // ��� �����͸� �ϳ��� ����(magazine.json)�� ����
            string json = JsonUtility.ToJson(rootData, true);
            File.WriteAllText(postSavePath, json);

            Debug.Log($"All data saved locally in {postSavePath}");

            // UI Ȱ��ȭ �� 3�� �� ��Ȱ��ȭ
            StartCoroutine(ShowUISuccessFeedback());

        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    // UI�� 3�ʰ� Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator ShowUISuccessFeedback()
    {
        if (successUI != null)
        {
            successUI.SetActive(true); // UI Ȱ��ȭ
            yield return new WaitForSeconds(3f); // 3�� ���
            successUI.SetActive(false); // UI ��Ȱ��ȭ
        }
        else
        {
            Debug.LogWarning("Success UI�� �������� �ʾҽ��ϴ�. Inspector���� �Ҵ��ϼ���.");
        }
    }

    // ... ������ �޼������ ���� �ڵ� �״�� ���� ...

public void LoadSpecificPostById(string targetPostId) // targetPostId�� ���� ���޹޴� ���
    {
        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("��ȿ�� postId�� �Է��ϼ���.");
            return;
        }

        // postId�� ���� ����� ��� ����
        string postDirectory = Path.Combine(saveDirectory, targetPostId);
        string postLoadPath = Path.Combine(postDirectory, saveFileName);

        // �ش� ��ο� magazine.json ������ �ִ��� Ȯ��
        if (!File.Exists(postLoadPath))
        {
            Debug.LogWarning($"postId '{targetPostId}'�� �ش��ϴ� ������ �����ϴ�: {postLoadPath}");
            return;
        }

        try
        {
            // ���Ͽ��� �����͸� �о�� JSON �Ľ�
            string json = File.ReadAllText(postLoadPath);
            rootData = JsonUtility.FromJson<RootObject>(json);

            // ������ postId�� �Խù��� ã��
            Post targetPost = rootData.posts.Find(post => post.postId == targetPostId);

            if (targetPost == null)
            {
                Debug.LogWarning($"postId '{targetPostId}'�� �ش��ϴ� �Խù��� �����ϴ�.");
                return;
            }

            // ���� UI ��� �ʱ�ȭ (���� �����͸� ����)
            textBoxes.ForEach(Destroy);
            imageBoxes.ForEach(Destroy);
            pages.ForEach(Destroy);

            textBoxes.Clear();
            imageBoxes.Clear();
            pages.Clear();

            Debug.Log($"���� UI ��� �ʱ�ȭ �Ϸ�. ������ ��: {pages.Count}");

            // �ε��� �����͸� �������� �������� ��� ����
            foreach (var page in targetPost.pages)
            {
                // ������ ����
                GameObject newPage = Instantiate(pagePrefab, pagesParentTransform);
                InitializePage(newPage); // ������ �ʱ�ȭ

                // �������� �ش��ϴ� �ؽ�Ʈ �ڽ��� �̹��� �ڽ� ����
                foreach (var element in page.elements)
                {
                    if (element.type == Element.ElementType.Text_Box)
                    {
                        // �ؽ�Ʈ �ڽ� ����
                        GameObject newTextBox = Instantiate(textBoxPrefab, newPage.transform);
                        InitializeTextBox(newTextBox);

                        // �ؽ�Ʈ �ڽ� ��ġ, ũ�� ����
                        newTextBox.transform.localPosition = element.position;
                        newTextBox.transform.localScale = element.scale;

                        // �ؽ�Ʈ �ڽ� ���� ����
                        TMP_Text textComponent = newTextBox.GetComponentInChildren<TMP_Text>();
                        if (textComponent != null)
                        {
                            textComponent.text = element.content;
                            textComponent.fontSize = element.fontSize;

                            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>($"Fonts/{element.fontFace}");
                            if (fontAsset != null) textComponent.font = fontAsset;

                            textComponent.fontStyle = FontStyles.Normal;
                            if (element.isUnderlined) textComponent.fontStyle |= FontStyles.Underline;
                            if (element.isStrikethrough) textComponent.fontStyle |= FontStyles.Strikethrough;

                            Debug.Log($"�ؽ�Ʈ �ڽ� �ε� �Ϸ�: {textComponent.text}");
                        }
                    }
                    else if (element.type == Element.ElementType.Image_Box)
                    {
                        // �̹��� �ڽ� ����
                        GameObject newImageBox = Instantiate(imageBoxPrefab, newPage.transform);
                        InitializeImageBox(newImageBox);

                        // �̹��� �ڽ� ��ġ, ũ�� ����
                        newImageBox.transform.localPosition = element.position;
                        newImageBox.transform.localScale = element.scale;

                        // �̹��� ������ ����
                        Image imageComponent = newImageBox.transform.GetChild(0).GetComponent<Image>();
                        if (imageComponent != null && element.imageData != null && element.imageData.Length > 0)
                        {
                            Texture2D texture = Element.DecodeImageFromBytes(element.imageData);
                            if (texture != null)
                            {
                                imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                                Debug.Log($"�̹��� �ڽ� �ε� �Ϸ�: {texture.width}x{texture.height}");
                            }
                        }
                    }
                }

                // �ߺ� ���� ����: �������� �̹� `pages` ����Ʈ�� �߰��ߴ��� Ȯ��
                if (!pages.Contains(newPage))
                {
                    pages.Add(newPage);
                }
            }

            // ������ ���� ������Ʈ
            totalPages = pages.Count;
            createMgr.pageCount = totalPages;
            createMgr.UpdateContentWidth();
            createMgr.UpdateScrollbarSteps();
            UpdateScrollbar();

            // ��ũ�ѹ� �ʱ� ��ġ ����
            pageScrollbar.value = 0f;

            Debug.Log($"Data loaded successfully for postId '{targetPostId}' from {postLoadPath}.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
        }
    }

    private void InitializePage(GameObject page)
    {
        page.name = $"Page_{System.Guid.NewGuid()}";
        pages.Add(page);

        Button btn_TextBox = page.transform.Find("btn_TextBox")?.GetComponent<Button>();
        if (btn_TextBox != null)
        {
            btn_TextBox.onClick.AddListener(() =>
            {
                GameObject newTextBox = Instantiate(textBoxPrefab, page.transform);
                InitializeTextBox(newTextBox);
            });
        }

        Button btn_ImageBox = page.transform.Find("btn_Image")?.GetComponent<Button>();
        if (btn_ImageBox != null)
        {
            btn_ImageBox.onClick.AddListener(() =>
            {
                GameObject newImageBox = Instantiate(imageBoxPrefab, page.transform);
                InitializeImageBox(newImageBox);
            });
        }

        Button deleteButton = page.transform.Find("btn_Delete")?.GetComponent<Button>();
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(() => RemovePage(page));
        }
    }

    private void InitializeTextBox(GameObject textBox)
    {
        textBox.name = $"TextBox_{System.Guid.NewGuid()}";
        textBoxes.Add(textBox);

        Button buttonContent = textBox.GetComponentInChildren<Button>();
        if (buttonContent != null)
        {
            buttonContent.name = $"{textBox.name}_Button";
            buttonContent.onClick.AddListener(toolManager.OnClickTogglePanel);
            buttonContent.onClick.AddListener(() =>
            {
                if (editorMgr != null)
                {
                    editorMgr.SetInputFieldTextFromButton(buttonContent);
                }
                else
                {
                    Debug.LogError("EditorMgr_KJS�� �Ҵ���� �ʾҽ��ϴ�.");
                }
            });
        }
        else
        {
            Debug.LogError("�ؽ�Ʈ �ڽ� �����տ� Button ������Ʈ�� �����ϴ�.");
        }
    }

    private void InitializeImageBox(GameObject imageBox)
    {
        imageBox.name = $"ImageBox_{System.Guid.NewGuid()}";
        imageBoxes.Add(imageBox);

        Button buttonContent = imageBox.GetComponentInChildren<Button>();
        if (buttonContent != null)
        {
            imageMgr.AddButton(buttonContent);
            Debug.Log($"ImageBox button {buttonContent.name} added to ImageMgr_KJS.");
        }
        else
        {
            Debug.LogError("ImageBox �����տ� Button ������Ʈ�� �����ϴ�.");
        }
    }

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
    }
    private void OnScrollbarValueChanged(float value)
    {
        if (isUpdatingScrollbar) return; // �̺�Ʈ ���� ����

        float step = 1f / Mathf.Max(totalPages - 1, 1); // ������ 0 ����
        int currentPage = Mathf.RoundToInt(value / step);
        float targetValue = currentPage * step;

        isUpdatingScrollbar = true;
        pageScrollbar.value = targetValue; // �� ������Ʈ
        isUpdatingScrollbar = false;

        Debug.Log($"Current Page: {currentPage + 1}/{totalPages}");
    }
}