using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSpeed : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public float videoSpeed = 0.5f;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Update()
    {
        SetVideoPlaySpeed(videoSpeed);
    }

    // 비디오 속도 조절
    public void SetVideoPlaySpeed(float speed)
    {
        videoPlayer.playbackSpeed = speed;
    }
}
