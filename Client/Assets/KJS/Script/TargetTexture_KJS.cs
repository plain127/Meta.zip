using UnityEngine;

public class TargetTexture_KJS : MonoBehaviour
{
    public Camera renderCamera; // 출력할 카메라
    public RenderTexture renderTexture; // 사용할 Render Texture

    void Start()
    {
        if (renderCamera != null && renderTexture != null)
        {
            // 카메라에 Render Texture 연결
            renderCamera.targetTexture = renderTexture;
        }
        else
        {
            Debug.LogError("카메라 또는 Render Texture가 할당되지 않았습니다.");
        }
    }
}
