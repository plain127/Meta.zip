using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPreview : MonoBehaviour
{
    public GameObject previewImage;

    public Vector2 offset = new Vector2(50, -50);

    RectTransform previewRect;

    void Start()
    {
        previewRect = previewImage.GetComponent<RectTransform>();

        previewImage.SetActive(false);
    }

    void Update()
    {
        if (previewImage.activeSelf)
        {
            Vector2 mousePosition = Input.mousePosition;
            previewRect.position = mousePosition + offset;
        }
    }

    // �̸����� Ȱ��ȭ
    public void ShowPreview()
    {
        previewImage.SetActive(true);
    }

    // �̸����� ��Ȱ��ȭ
    public void HidePreview()
    {
        previewImage.SetActive(false);
    }
}
