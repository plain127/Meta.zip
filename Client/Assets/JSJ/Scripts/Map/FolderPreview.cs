using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class FolderPreview : MonoBehaviour
{
    public GameObject previewImage;

    public Vector2 offset = new Vector2(200, -200);

    RectTransform previewRect;

    private void Start()
    {
        previewRect = previewImage.GetComponent<RectTransform>();

        previewImage.SetActive(false);
    }

    private void Update()
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
