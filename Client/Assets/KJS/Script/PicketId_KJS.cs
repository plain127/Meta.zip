using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicketId_KJS : MonoBehaviour
{
    [Header("Picket ID (Inspector View)")]
    [SerializeField]
    private string picketId; // Picket�� ���� ID (Inspector�� ǥ��)

    [Header("Screenshot Path (Inspector View)")]
    [SerializeField]
    private string screenshotPath; // ��ũ���� ��� (Inspector�� ǥ��)

    // ID ���� �Լ�
    public void SetPicketId(string id)
    {
        picketId = id;
    }

    // ��ũ���� ��� ���� �Լ�
    public void SetScreenshotPath(string path)
    {
        screenshotPath = path;
        Billboard billboard = FindObjectOfType<Billboard>(); // Billboard ã��
        if (billboard != null)
        {
            billboard.UpdateScreenshotPath(screenshotPath);
        }
    }

    // ID�� �������� �Լ�
    public string GetPicketId()
    {
        return picketId;
    }

    // ��ũ���� ��θ� �������� �Լ�
    public string GetScreenshotPath()
    {
        return screenshotPath;
    }

    void Start()
    {
        // �׽�Ʈ: Picket ID�� ��ũ���� ��� ���
        if (!string.IsNullOrEmpty(picketId))
        {
            Debug.Log("Picket ID: " + picketId);
        }

        if (!string.IsNullOrEmpty(screenshotPath))
        {
            Debug.Log("Screenshot Path: " + screenshotPath);
        }
    }
}
