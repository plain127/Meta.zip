using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KeywordUI : MonoBehaviour
{
    public RawImage trendmap;          // 트렌드 이미지를 표시할 UI 컴포넌트
    public APIManager apiManager;     // APIManager 인스턴스

    void Start()
    {
        if (apiManager != null)
        {
            // APIManager의 Trend 메서드 호출
            apiManager.Trend();

            // Trend 이미지가 로드될 때까지 기다린 후 업데이트
            StartCoroutine(UpdateTrendImage());
        }
        else
        {
            Debug.LogError("APIManager가 설정되지 않았습니다.");
        }
    }

    private IEnumerator UpdateTrendImage()
    {
        // APIManager에서 트렌드 이미지가 로드될 때까지 대기
        while (apiManager.trendDownloadTexture == null)
        {
            yield return null; // 한 프레임 대기
        }

        // 트렌드 이미지를 RawImage에 할당
        trendmap.texture = apiManager.trendDownloadTexture;
        Debug.Log("트렌드 이미지가 성공적으로 적용되었습니다.");
    }
}
