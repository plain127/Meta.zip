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
    public byte[] imageData; // string 대신 byte[]로 변경
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
    public GameObject successUI; // 저장 성공 UI를 Inspector에서 할당 가능하도록 퍼블릭 필드 추가


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
        Debug.Log($"버튼 {button.name}이 추가되었습니다.");
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
        pages.Add(page); // pages 리스트에 페이지 추가
        totalPages = pages.Count; // totalPages를 pages.Count로 업데이트
        Debug.Log($"Page added. Current totalPages: {totalPages}");
    }

    public void RemovePage(GameObject page)
    {
        pages.Remove(page); // pages 리스트에서 페이지 제거
        totalPages = pages.Count; // totalPages를 pages.Count로 업데이트
        Debug.Log($"Page removed. Current totalPages: {totalPages}");
    }

    private void SaveObjectsToFile()
    {
        string targetPostId = inputPostIdField.text;

        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("유효한 postId를 입력하세요.");
            return;
        }

        try
        {
            // postId에 따라 저장 경로 설정
            string postDirectory = Path.Combine(saveDirectory, targetPostId);
            string postSavePath = Path.Combine(postDirectory, saveFileName);

            // 해당 postId의 폴더가 없으면 생성
            if (!Directory.Exists(postDirectory))
            {
                Directory.CreateDirectory(postDirectory);
                Debug.Log($"Directory created at: {postDirectory}");
            }

            // 동일한 postId가 이미 있는지 확인
            Post existingPost = rootData.posts.Find(post => post.postId == targetPostId);

            // 동일한 ID의 게시물이 있다면 덮어쓰고, 없으면 새로 생성
            Post targetPost = existingPost ?? new Post(targetPostId);
            if (existingPost == null)
            {
                rootData.posts.Add(targetPost);
            }
            else
            {
                // 기존 게시물 내용을 비웁니다.
                targetPost.pages.Clear();
            }

            // 유효한 textBox와 imageBox만 필터링
            textBoxes.RemoveAll(item => item == null);
            imageBoxes.RemoveAll(item => item == null);
            pages.RemoveAll(item => item == null);

            // 페이지 및 요소 생성
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

            // 모든 데이터를 하나의 파일(magazine.json)로 저장
            string json = JsonUtility.ToJson(rootData, true);
            File.WriteAllText(postSavePath, json);

            Debug.Log($"All data saved locally in {postSavePath}");

            // UI 활성화 후 3초 후 비활성화
            StartCoroutine(ShowUISuccessFeedback());

        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    // UI를 3초간 활성화하는 코루틴
    private IEnumerator ShowUISuccessFeedback()
    {
        if (successUI != null)
        {
            successUI.SetActive(true); // UI 활성화
            yield return new WaitForSeconds(3f); // 3초 대기
            successUI.SetActive(false); // UI 비활성화
        }
        else
        {
            Debug.LogWarning("Success UI가 설정되지 않았습니다. Inspector에서 할당하세요.");
        }
    }

    // ... 나머지 메서드들은 기존 코드 그대로 유지 ...

public void LoadSpecificPostById(string targetPostId) // targetPostId를 직접 전달받는 방식
    {
        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("유효한 postId를 입력하세요.");
            return;
        }

        // postId에 따라 저장된 경로 설정
        string postDirectory = Path.Combine(saveDirectory, targetPostId);
        string postLoadPath = Path.Combine(postDirectory, saveFileName);

        // 해당 경로에 magazine.json 파일이 있는지 확인
        if (!File.Exists(postLoadPath))
        {
            Debug.LogWarning($"postId '{targetPostId}'에 해당하는 파일이 없습니다: {postLoadPath}");
            return;
        }

        try
        {
            // 파일에서 데이터를 읽어와 JSON 파싱
            string json = File.ReadAllText(postLoadPath);
            rootData = JsonUtility.FromJson<RootObject>(json);

            // 지정된 postId의 게시물을 찾기
            Post targetPost = rootData.posts.Find(post => post.postId == targetPostId);

            if (targetPost == null)
            {
                Debug.LogWarning($"postId '{targetPostId}'에 해당하는 게시물이 없습니다.");
                return;
            }

            // 기존 UI 요소 초기화 (이전 데이터를 제거)
            textBoxes.ForEach(Destroy);
            imageBoxes.ForEach(Destroy);
            pages.ForEach(Destroy);

            textBoxes.Clear();
            imageBoxes.Clear();
            pages.Clear();

            Debug.Log($"기존 UI 요소 초기화 완료. 페이지 수: {pages.Count}");

            // 로드한 데이터를 바탕으로 페이지와 요소 생성
            foreach (var page in targetPost.pages)
            {
                // 페이지 생성
                GameObject newPage = Instantiate(pagePrefab, pagesParentTransform);
                InitializePage(newPage); // 페이지 초기화

                // 페이지에 해당하는 텍스트 박스와 이미지 박스 생성
                foreach (var element in page.elements)
                {
                    if (element.type == Element.ElementType.Text_Box)
                    {
                        // 텍스트 박스 생성
                        GameObject newTextBox = Instantiate(textBoxPrefab, newPage.transform);
                        InitializeTextBox(newTextBox);

                        // 텍스트 박스 위치, 크기 설정
                        newTextBox.transform.localPosition = element.position;
                        newTextBox.transform.localScale = element.scale;

                        // 텍스트 박스 내용 설정
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

                            Debug.Log($"텍스트 박스 로드 완료: {textComponent.text}");
                        }
                    }
                    else if (element.type == Element.ElementType.Image_Box)
                    {
                        // 이미지 박스 생성
                        GameObject newImageBox = Instantiate(imageBoxPrefab, newPage.transform);
                        InitializeImageBox(newImageBox);

                        // 이미지 박스 위치, 크기 설정
                        newImageBox.transform.localPosition = element.position;
                        newImageBox.transform.localScale = element.scale;

                        // 이미지 데이터 설정
                        Image imageComponent = newImageBox.transform.GetChild(0).GetComponent<Image>();
                        if (imageComponent != null && element.imageData != null && element.imageData.Length > 0)
                        {
                            Texture2D texture = Element.DecodeImageFromBytes(element.imageData);
                            if (texture != null)
                            {
                                imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                                Debug.Log($"이미지 박스 로드 완료: {texture.width}x{texture.height}");
                            }
                        }
                    }
                }

                // 중복 생성 방지: 페이지를 이미 `pages` 리스트에 추가했는지 확인
                if (!pages.Contains(newPage))
                {
                    pages.Add(newPage);
                }
            }

            // 페이지 정보 업데이트
            totalPages = pages.Count;
            createMgr.pageCount = totalPages;
            createMgr.UpdateContentWidth();
            createMgr.UpdateScrollbarSteps();
            UpdateScrollbar();

            // 스크롤바 초기 위치 설정
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
                    Debug.LogError("EditorMgr_KJS가 할당되지 않았습니다.");
                }
            });
        }
        else
        {
            Debug.LogError("텍스트 박스 프리팹에 Button 컴포넌트가 없습니다.");
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
            Debug.LogError("ImageBox 프리팹에 Button 컴포넌트가 없습니다.");
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
        if (isUpdatingScrollbar) return; // 이벤트 루프 방지

        float step = 1f / Mathf.Max(totalPages - 1, 1); // 나누기 0 방지
        int currentPage = Mathf.RoundToInt(value / step);
        float targetValue = currentPage * step;

        isUpdatingScrollbar = true;
        pageScrollbar.value = targetValue; // 값 업데이트
        isUpdatingScrollbar = false;

        Debug.Log($"Current Page: {currentPage + 1}/{totalPages}");
    }
}