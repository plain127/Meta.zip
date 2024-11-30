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

    // 미리보기 활성화
    public void ShowPreview()
    {
        previewImage.SetActive(true);
    }

    // 미리보기 비활성화
    public void HidePreview()
    {
        previewImage.SetActive(false);
    }
}
