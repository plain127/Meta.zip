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
    private string saveFileName = "Magazine.json"; // 확장자를 .json으로 변경
    private string savePath;

    private RootObject rootData = new RootObject();

    private void Start()
    {
        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);

        if (File.Exists(savePath))
        {
            LoadDataFromJsonFile(); // JSON 파일 로드 메서드로 변경
        }
    }

    // 특정 postId에 맞는 데이터를 로드하는 메서드
    public void LoadObjectsFromFile(string postId)
    {
        try
        {
            Debug.Log($"LoadObjectsFromFile() called for postId: {postId}");

            // postId에 따라 로드 경로 설정
            string postDirectory = Path.Combine(saveDirectory, postId);
            string postLoadPath = Path.Combine(postDirectory, saveFileName);

            // 해당 경로에 magazine.json 파일이 있는지 확인
            if (!File.Exists(postLoadPath))
            {
                Debug.LogWarning($"postId '{postId}'에 해당하는 파일이 없습니다: {postLoadPath}");
                return;
            }

            // 파일에서 데이터를 읽어와 JSON 파싱
            string json = File.ReadAllText(postLoadPath);
            rootData = JsonUtility.FromJson<RootObject>(json);
            Debug.Log("Data loaded from JSON file successfully.");

            // 지정된 postId의 게시물을 찾기
            Post post = rootData.posts.Find(p => p.postId == postId);
            if (post == null)
            {
                Debug.LogWarning($"postId '{postId}'에 해당하는 게시물이 없습니다.");
                return;
            }

            // 기존 UI 요소 초기화
            textBoxes.ForEach(Destroy);
            imageBoxes.ForEach(Destroy);
            pages.ForEach(Destroy);

            textBoxes.Clear();
            imageBoxes.Clear();
            pages.Clear();

            // 로드한 데이터를 바탕으로 페이지와 요소 생성
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

            // 페이지 정보 업데이트
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

    // JSON 파일로부터 데이터를 로드하는 메서드
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