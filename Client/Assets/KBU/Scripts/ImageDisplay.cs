using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplay : MonoBehaviour
{
    public APIManager imageDownloader; 
    public RawImage rawImage;

    public void Start()
    {   
        StartCoroutine(TrendWaitForDownloadAndDisplayImage());
        StartCoroutine(CoverWaitForDownloadAndDisplayImage());
    }

    public IEnumerator CoverWaitForDownloadAndDisplayImage()
    {
       
        while (imageDownloader.CoverGetDownloadedImage() == null)
        {
            yield return null; 
        }

        rawImage.texture = imageDownloader.CoverGetDownloadedImage();
        Debug.Log("Image displayed on UI.");
    }

    public IEnumerator TrendWaitForDownloadAndDisplayImage()
    {
       
        while (imageDownloader.TrendGetDownloadedImage() == null)
        {
            yield return null; 
        }

        rawImage.texture = imageDownloader.TrendGetDownloadedImage();
        Debug.Log("Image displayed on UI.");
    }
}