using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    public int superSize = 1;
    int screenshotCount = 0;

    void Start()
    {
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CaptureScreenshot();
        }
    }

    // 스크린 캡쳐 함수
    public void CaptureScreenshot()
    {
        string screenshotFileName = "Screenshot_" + screenshotCount + ".png";
        string savePath = Path.Combine(Application.dataPath, screenshotFileName);
        
        ScreenCapture.CaptureScreenshot(savePath, superSize);
        screenshotCount++;

        Debug.Log("스크린샷 저장됨: " + savePath);

        InventoryManager.instance.AddScreenshot(savePath);
    }
}
