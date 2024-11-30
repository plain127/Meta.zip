using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraToUI_KJS : MonoBehaviour
{
    public RawImage rawImageUI;          // UI에서 Render Texture를 표시할 RawImage
    public RenderTexture renderTexture; // Render Texture
    public Camera renderCamera;        // 동적으로 생성될 카메라

    void Start()
    {
        if (rawImageUI == null || renderTexture == null)
        {
            Debug.LogError("RawImage 또는 RenderTexture가 할당되지 않았습니다.");
            return;
        }

        // 동적으로 생성되는 카메라를 찾는 코루틴 실행
        StartCoroutine(FindDynamicCamera());
    }

    IEnumerator FindDynamicCamera()
    {
        // 동적 생성된 카메라를 찾을 때까지 반복
        while (renderCamera == null)
        {
            // 카메라가 생성되었는지 확인 (Tag 또는 이름으로 찾기)
            renderCamera = GameObject.FindWithTag("DynamicCamera")?.GetComponent<Camera>();

            if (renderCamera == null)
            {
                Debug.Log("동적으로 생성된 카메라를 기다리는 중...");
                yield return null; // 한 프레임 대기
            }
            else
            {
                Debug.Log("동적으로 생성된 카메라를 찾았습니다!");

                // 카메라 설정
                renderCamera.targetTexture = renderTexture;

                // RawImage UI에 Render Texture를 연결
                rawImageUI.texture = renderTexture;
            }
        }
    }
}