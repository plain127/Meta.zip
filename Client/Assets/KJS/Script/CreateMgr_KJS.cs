using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMgr_KJS : MonoBehaviour
{
    public SaveMgr_KJS saveMgr;  // SaveMgr_KJS ����

    public GameObject panelPage;   // ������ ������
    public GameObject textBox;     // �ؽ�Ʈ �ڽ� ������
    public GameObject imageBox;    // �̹��� �ڽ� ������

    public Transform content;  // ������ ������Ʈ�� �θ� Ʈ������
    public Scrollbar horizontalScrollbar;
    public ScrollRect scrollRect;

    public int pageCount = 1;
    private float pageWidth;

    public ToolMgr_KJS toolManager;
    public EditorMgr_KJS editorMgr;
    public ImageMgr_KJS imageMgr;  // ImageMgr_KJS ����

    void Start()
    {
        if (panelPage == null || content == null) return;
        pageWidth = panelPage.GetComponent<RectTransform>().rect.width;
        UpdateScrollbarSteps();
    }

    public void CreatePage()
    {
        GameObject newPage = Instantiate(panelPage, content);
        newPage.name = $"Page_{System.Guid.NewGuid()}";
        pageCount++;

        saveMgr.AddPage(newPage);

        Button btn_TextBox = newPage.transform.Find("btn_TextBox")?.GetComponent<Button>();
        if (btn_TextBox != null)
        {
            btn_TextBox.onClick.AddListener(() => CreateTextBox(newPage.transform));
        }

        Button btn_ImageBox = newPage.transform.Find("btn_Image")?.GetComponent<Button>();
        if (btn_ImageBox != null)
        {
            btn_ImageBox.onClick.AddListener(() => CreateImageBox(newPage.transform));
        }

        Button deleteButton = newPage.transform.Find("btn_Delete")?.GetComponent<Button>();
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(() => RemovePage(newPage));
        }

        UpdateContentWidth();
        UpdateScrollbarSteps();
    }

    public void CreateTextBox(Transform parent)
    {
        GameObject newTextBox = Instantiate(textBox, parent);
        newTextBox.name = $"TextBox_{System.Guid.NewGuid()}";

        saveMgr.AddTextBox(newTextBox);

        Button buttonContent = newTextBox.GetComponentInChildren<Button>();
        if (buttonContent != null)
        {
            buttonContent.name = $"{newTextBox.name}_Button";

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

    public void CreateImageBox(Transform parent)
    {
        GameObject newImageBox = Instantiate(imageBox, parent);
        newImageBox.name = $"ImageBox_{System.Guid.NewGuid()}";

        saveMgr.AddImageBox(newImageBox);

        Button buttonContent = newImageBox.GetComponentInChildren<Button>();
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

    public void RemovePage(GameObject page)
    {
        saveMgr.RemovePage(page);
        Destroy(page);
        pageCount = Mathf.Max(1, pageCount - 1);

        UpdateContentWidth();
        UpdateScrollbarSteps();
    }

    public void UpdateContentWidth()
    {
        if (content == null) return;

        RectTransform contentRect = content.GetComponent<RectTransform>();
        float newWidth = pageWidth * pageCount;
        contentRect.sizeDelta = new Vector2(newWidth, contentRect.sizeDelta.y);
    }

    public void UpdateScrollbarSteps()
    {
        if (horizontalScrollbar == null || pageCount <= 1) return;

        horizontalScrollbar.numberOfSteps = pageCount;
    }
}