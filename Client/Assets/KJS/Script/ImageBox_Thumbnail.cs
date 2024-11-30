using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro ���
using SFB;   // StandaloneFileBrowser ���

public class ImageBox_Thumbnail : MonoBehaviour
{
    private Button button;  // ��ư ������Ʈ ����
    public Image targetImage;  // ���õ� �̹����� ǥ���� Image ������Ʈ (Inspector���� �Ҵ�)
    public TextMeshProUGUI placeholderText;  // �̹����� �Ҵ�Ǹ� ��Ȱ��ȭ�� TextMeshProUGUI ������Ʈ (Inspector���� �Ҵ�)

    void Start()
    {
        // ��ư ������Ʈ ��������
        button = GetComponent<Button>();

        if (button != null)
        {
            // ��ư Ŭ�� �� OpenFileExplorer �޼��� ȣ��
            button.onClick.AddListener(OpenFileExplorer);
        }
        else
        {
            Debug.LogError("Button ������Ʈ�� ã�� �� �����ϴ�.");
        }

        if (targetImage == null)
        {
            Debug.LogError("Target Image�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� Image ������Ʈ�� �����ϼ���.");
        }

        if (placeholderText == null)
        {
            Debug.LogError("Placeholder Text�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� TextMeshProUGUI ������Ʈ�� �����ϼ���.");
        }
    }

    // ���� Ž���⸦ ���� �޼���
    void OpenFileExplorer()
    {
        // StandaloneFileBrowser�� ����Ͽ� ���� Ž���� ����
        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            "Select an Image",                 // â ����
            "",                               // �ʱ� ��� (�� ���ڿ��� ���� �� �⺻ ��� ���)
            new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") }, // ��� Ȯ���� ����
            false                             // ���� ���� ���� (false: ���� ���� ����)
        );

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            Debug.Log($"���õ� ���� ���: {paths[0]}");
            StartCoroutine(LoadImage(paths[0]));  // ���õ� ��η� �̹��� �ε�
        }
        else
        {
            Debug.LogWarning("������ ���õ��� �ʾҽ��ϴ�.");
        }
    }

    // ���õ� �̹��� ������ �ε��Ͽ� Image ������Ʈ�� �����ϴ� �ڷ�ƾ
    IEnumerator LoadImage(string path)
    {
        // ���� ��ο��� �̹��� �����͸� �о����
        byte[] imageData = File.ReadAllBytes(path);

        // Texture2D ����
        Texture2D texture = new Texture2D(2, 2);  // �ӽ� ũ�� (�ε� �� �ڵ����� ������)
        bool isLoaded = texture.LoadImage(imageData);  // �̹��� ������ �ε�

        if (isLoaded)
        {
            // Texture2D�� Sprite�� ��ȯ
            Sprite newSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            // Image ������Ʈ�� �� Sprite ����
            targetImage.sprite = newSprite;
            Debug.Log("�̹����� ���������� �ε�Ǿ����ϴ�.");

            // �̹����� �Ҵ�Ǿ����Ƿ� Text ��Ȱ��ȭ
            if (placeholderText != null)
            {
                placeholderText.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("�̹��� �ε忡 �����߽��ϴ�.");
        }

        yield return null;
    }
}