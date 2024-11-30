using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // 경로 설정
        string videoPath = Path.Combine(Application.streamingAssetsPath, "Loginvideo.mp4");
        
        videoPlayer.url = "file:///" + videoPath;
        Debug.Log("Video Path: " + videoPlayer.url);

        // 비디오 실행
        videoPlayer.Play();
    }
}

