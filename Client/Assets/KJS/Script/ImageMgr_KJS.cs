using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshPro ��� �� �ʿ�
using SFB;   // StandaloneFileBrowser ���ӽ����̽� ���

public class ImageMgr_KJS : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();  // �������� ������ ��ư��
    private HashSet<Button> buttonsWithHiddenText = new HashSet<Button>();  // �ؽ�Ʈ�� ������ ��ư�� ����

    // ���� ������ ��ư�� ����Ʈ�� �߰��ϰ� �̺�Ʈ ����
    public void AddButton(Button newButton)
    {
        if (newButton != null)
        {
            buttons.Add(newButton);
            newButton.onClick.AddListener(() => OnButtonClick(newButton));
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void OnButtonClick(Button targetButton)
    {
        OpenFileExplorerAndSetImage(targetButton);  // ���� Ž���⸦ ���� �̹��� ����
    }

    // ���� Ž���⸦ ���� �̹��� ������ �����ϴ� �޼���
    private void OpenFileExplorerAndSetImage(Button targetButton)
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            "Select an Image", "",
            new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg", "gif", "bmp") }, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            SetImageToButton(targetButton, paths[0]);  // �̹��� �Ҵ�
        }
    }

    // �̹��� ������ ��ư�� �Ҵ��ϴ� �޼���
    private void SetImageToButton(Button button, string imagePath)
    {
        byte[] imageBytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);

        Sprite newSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        // ��ư�� Image ������Ʈ�� Sprite �Ҵ�
        button.GetComponent<Image>().sprite = newSprite;
    }

    // �� �����Ӹ��� ��ư���� üũ�Ͽ� �̹����� �Ҵ�� ��� �ؽ�Ʈ�� ����
    private void Update()
    {
        for (int i = buttons.Count - 1; i >= 0; i--)  // ����Ʈ�� �������� ��ȸ�Ͽ� ���� ������ Ȯ��
        {
            Button button = buttons[i];
            if (button == null)  // ��ư�� ������ ��� ����Ʈ���� �����ϰ� ��������
            {
                buttons.RemoveAt(i);
                continue;
            }

            Image buttonImage = button.GetComponent<Image>();

            // �̹����� �Ҵ�Ǿ� �ְ�, ���� �ؽ�Ʈ�� �������� ���� ��ư�� ó��
            if (buttonImage != null && buttonImage.sprite != null && !buttonsWithHiddenText.Contains(button))
            {
                HideButtonText(button);
                buttonsWithHiddenText.Add(button);  // �ؽ�Ʈ�� ������ ��ư���� ���
            }
        }
    }

    // ��ư�� Text �Ǵ� TMP_Text ������Ʈ�� ����� �޼���
    private void HideButtonText(Button button)
    {
        Text uiText = button.GetComponentInChildren<Text>();
        TMP_Text tmpText = button.GetComponentInChildren<TMP_Text>();

        if (uiText != null) uiText.gameObject.SetActive(false);
        if (tmpText != null) tmpText.gameObject.SetActive(false);
    }
}