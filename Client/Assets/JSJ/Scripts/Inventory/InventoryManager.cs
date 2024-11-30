using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ScreenshotData
{
    // 스크린샷 경로 List
    public List<string> screenshotList = new List<string>();
}


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    ScreenshotData screenshotData;

    public InventoryUI inventoryUI;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        LoadScreenshot();
    }

    void Update()
    {
        
    }

    // 스크린샷 로드
    public void LoadScreenshot()
    {
        string screenshotFolder = Application.dataPath;

        string[] files = Directory.GetFiles(screenshotFolder, "*.png");

        // 스크린샷 데이터 초기화
        if (screenshotData == null)
        {
            screenshotData = new ScreenshotData();
        }
        else
        {
            screenshotData.screenshotList.Clear();
        }

        // 이미 있는 파일 목록을 스크린샷 데이터에 추가
        foreach (string file in files)
        {
            screenshotData.screenshotList.Add(file);
        }

        // 인벤토리 UI 업데이트
        inventoryUI.UpdateInventoryUI();
    }

    // 스크린샷 저장 경로 추가
    public void AddScreenshot(string savePath)
    {
        if (screenshotData == null)
        {
            screenshotData = new ScreenshotData();
        }

        if (!screenshotData.screenshotList.Contains(savePath))
        {
            screenshotData.screenshotList.Add(savePath);

            inventoryUI.UpdateInventoryUI();
        }
    }

    // 스크린샷 삭제
    public void DeleteScreenshot(int index)
    {
        if (index >= 0 && index < ScreenshotCount())
        {
            string deletePath = screenshotData.screenshotList[index];

            if (File.Exists(deletePath))
            {
                File.Delete(deletePath);
                screenshotData.screenshotList.RemoveAt(index);
                inventoryUI.UpdateInventoryUI();
                Debug.Log("파일 삭제됨 : " + deletePath);
            }
        }
    }

    //스크린샷 카운트
    public int ScreenshotCount()
    {
        return screenshotData.screenshotList.Count;
    }

    // 스크린샷 이미지 확인
    public void ShowScreenshot(int index)
    {
        if (index >= 0 && index < ScreenshotCount())
        {
            string screenshotPath = screenshotData.screenshotList[index];

            if (!string.IsNullOrEmpty(screenshotPath))
            {
                // 스크린샷 파일의 날짜 가져오기
                DateTime fileDate = File.GetCreationTime(screenshotPath);
                string formattedDate = fileDate.ToString("MM월/dd일");

                // 스크린샷 표시
                inventoryUI.RPC_DisplayScreenshot(screenshotPath);

                // 버튼 위에 날짜 표기 (UI 처리)
                Transform slotButton = inventoryUI.slot.GetChild(index);
                TextMeshProUGUI tmpText = slotButton.GetComponentInChildren<TextMeshProUGUI>();
                if (tmpText != null)
                {
                    tmpText.text = formattedDate;
                }
            }
            else
            {
                Debug.LogWarning("유효하지 않은 인덱스: " + index);
            }
        }
    }
    public string GetScreenshotPath(int index)
    {
        if (index >= 0 && index < screenshotData.screenshotList.Count)
        {
            return screenshotData.screenshotList[index];
        }
        return null; // 유효하지 않은 인덱스인 경우 null 반환
    }
    public List<string> GetScreenshotList()
    {
        if (screenshotData == null)
        {
            screenshotData = new ScreenshotData();
        }

        return screenshotData.screenshotList;
    }
}
