using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class TakeScreenshotAndSave : MonoBehaviour
{
    //Object To Screenshot
    [SerializeField] private RectTransform _objToScreenshot;
    //Assign the button to take screenshot on clicking
    [SerializeField] private Button _takeScreenshotButton;
    void Start()
    {
        _takeScreenshotButton.onClick.AddListener(OnClickTakeScreenshotAndSaveButton);
    }
    private void OnClickTakeScreenshotAndSaveButton()
    {
        StartCoroutine(TakeSnapShotAndSave());
    }
    //Using a Coroutine instead of normal method
    public IEnumerator TakeSnapShotAndSave()
    {
        //Code will throw error at runtime if this is removed
        yield return new WaitForEndOfFrame();

        //Get the corners of RectTransform rect and store it in a array vector
        Vector3[] corners = new Vector3[4];
        _objToScreenshot.GetWorldCorners(corners);

        //Remove 100 and you will get error
        int width = ((int)corners[3].x - (int)corners[0].x) - 100;
        int height = (int)corners[1].y - (int)corners[0].y;
        var startX = corners[0].x;
        var startY = corners[0].y;

        //Make a temporary texture and read pixels from it
        Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        ss.Apply();
        Debug.Log("Start X : " + startX + " Start Y : " + startY);
        Debug.Log("Screen Width : " + Screen.width + " Screen Height : " + Screen.height);
        Debug.Log("Texture Width : " + width + " Texture Height : " + height);

        //Save the screenshot to disk
        byte[] byteArray = ss.EncodeToPNG();
        string savePath = Application.persistentDataPath + "/ScreenshotSave.png";
        System.IO.File.WriteAllBytes(savePath, byteArray);
        Debug.Log("Screenshot Path : " + savePath);

        // Destroy texture to avoid memory leaks
        Destroy(ss);
    }
}