using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraToUI_KJS : MonoBehaviour
{
    public RawImage rawImageUI;          // UI���� Render Texture�� ǥ���� RawImage
    public RenderTexture renderTexture; // Render Texture
    public Camera renderCamera;        // �������� ������ ī�޶�

    void Start()
    {
        if (rawImageUI == null || renderTexture == null)
        {
            Debug.LogError("RawImage �Ǵ� RenderTexture�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        // �������� �����Ǵ� ī�޶� ã�� �ڷ�ƾ ����
        StartCoroutine(FindDynamicCamera());
    }

    IEnumerator FindDynamicCamera()
    {
        // ���� ������ ī�޶� ã�� ������ �ݺ�
        while (renderCamera == null)
        {
            // ī�޶� �����Ǿ����� Ȯ�� (Tag �Ǵ� �̸����� ã��)
            renderCamera = GameObject.FindWithTag("DynamicCamera")?.GetComponent<Camera>();

            if (renderCamera == null)
            {
                Debug.Log("�������� ������ ī�޶� ��ٸ��� ��...");
                yield return null; // �� ������ ���
            }
            else
            {
                Debug.Log("�������� ������ ī�޶� ã�ҽ��ϴ�!");

                // ī�޶� ����
                renderCamera.targetTexture = renderTexture;

                // RawImage UI�� Render Texture�� ����
                rawImageUI.texture = renderTexture;
            }
        }
    }
}