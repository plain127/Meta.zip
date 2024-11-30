using UnityEngine;

public class TargetTexture_KJS : MonoBehaviour
{
    public Camera renderCamera; // ����� ī�޶�
    public RenderTexture renderTexture; // ����� Render Texture

    void Start()
    {
        if (renderCamera != null && renderTexture != null)
        {
            // ī�޶� Render Texture ����
            renderCamera.targetTexture = renderTexture;
        }
        else
        {
            Debug.LogError("ī�޶� �Ǵ� Render Texture�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
