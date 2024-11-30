using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicketId_KJS : MonoBehaviour
{
    [Header("Picket ID (Inspector View)")]
    [SerializeField]
    private string picketId; // Picket의 고유 ID (Inspector에 표시)

    [Header("Screenshot Path (Inspector View)")]
    [SerializeField]
    private string screenshotPath; // 스크린샷 경로 (Inspector에 표시)

    // ID 설정 함수
    public void SetPicketId(string id)
    {
        picketId = id;
    }

    // 스크린샷 경로 설정 함수
    public void SetScreenshotPath(string path)
    {
        screenshotPath = path;
        Billboard billboard = FindObjectOfType<Billboard>(); // Billboard 찾기
        if (billboard != null)
        {
            billboard.UpdateScreenshotPath(screenshotPath);
        }
    }

    // ID를 가져오는 함수
    public string GetPicketId()
    {
        return picketId;
    }

    // 스크린샷 경로를 가져오는 함수
    public string GetScreenshotPath()
    {
        return screenshotPath;
    }

    void Start()
    {
        // 테스트: Picket ID와 스크린샷 경로 출력
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
