using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplay_KJS : MonoBehaviour
{
    public APIManager imageDownloader;
    public Image image;  // RawImage ��� Image ������Ʈ ���

    void Start()
    {
        StartCoroutine(TrendWaitForDownloadAndDisplayImage());
        StartCoroutine(CoverWaitForDownloadAndDisplayImage());
    }

    IEnumerator CoverWaitForDownloadAndDisplayImage()
    {
        // �̹����� �ٿ�ε�� ������ ���
        while (imageDownloader.CoverGetDownloadedImage() == null)
        {
            yield return null;
        }

        // �ٿ�ε�� Texture2D�� Sprite�� ��ȯ�Ͽ� Image ������Ʈ�� ǥ��
        Texture2D downloadedTexture = imageDownloader.CoverGetDownloadedImage();
        image.sprite = TextureToSprite(downloadedTexture);
        Debug.Log("Cover image displayed on UI.");
    }

    IEnumerator TrendWaitForDownloadAndDisplayImage()
    {
        print("!!!");
        // �̹����� �ٿ�ε�� ������ ���
        while (imageDownloader.TrendGetDownloadedImage() == null)
        {
            yield return null;
        }

        // �ٿ�ε�� Texture2D�� Sprite�� ��ȯ�Ͽ� Image ������Ʈ�� ǥ��
        Texture2D downloadedTexture = imageDownloader.TrendGetDownloadedImage();
        image.sprite = TextureToSprite(downloadedTexture);
        Debug.Log("Cover image displayed on UI.");
    }

    // Texture2D�� Sprite�� ��ȯ�ϴ� �޼���
    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}