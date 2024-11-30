using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Photon.Pun; // Photon ���ӽ����̽� �߰�

public class MaterialMgr_KJS : MonoBehaviourPunCallbacks // Photon�� MonoBehaviourPunCallbacks ���
{
    [Header("PicketId_KJS ����")]
    [SerializeField]
    private PicketId_KJS picketIdComponent; // PicketId_KJS ������Ʈ�� ����

    [Header("��ũ���� ��� (Debug)")]
    public string screenshotPathDebug; // PicketId_KJS�� ��θ� Inspector�� ǥ��

    public Texture autoAssignTexture; // �������� �ε�� �ؽ�ó

    private string lastScreenshotPath; // ���� ��θ� ������ ���� ����

    void Start()
    {
        // PicketId_KJS ������Ʈ�� �ڵ����� ã�ų� Inspector���� ����
        if (picketIdComponent == null)
        {
            picketIdComponent = GetComponent<PicketId_KJS>();
        }

        if (picketIdComponent == null)
        {
            Debug.LogError("PicketId_KJS ������Ʈ�� ã�� �� �����ϴ�. Inspector���� ���� �Ҵ��ؾ� �մϴ�.");
            return;
        }

        // �ʱ� ��� ��������
        lastScreenshotPath = picketIdComponent.GetScreenshotPath();
        screenshotPathDebug = lastScreenshotPath;

        // ��ΰ� ��ȿ�� ��� �ؽ�ó �ε�
        if (!string.IsNullOrEmpty(lastScreenshotPath))
        {
            StartCoroutine(LoadTextureFromPath(lastScreenshotPath));
        }
        else
        {
            Debug.LogWarning("�ʱ� ��ũ���� ��ΰ� ��� �ֽ��ϴ�.");
        }
    }

    void Update()
    {
        // PicketId_KJS�� ��� ���� ����
        string currentPath = picketIdComponent.GetScreenshotPath();
        if (currentPath != lastScreenshotPath)
        {
            lastScreenshotPath = currentPath;
            screenshotPathDebug = currentPath;

            Debug.Log("��ũ���� ��ΰ� ����Ǿ����ϴ�: " + currentPath);

            // ��ΰ� ����Ǿ����Ƿ� �� �ؽ�ó�� �ε�
            StartCoroutine(LoadTextureFromPath(currentPath));
        }
    }

    IEnumerator LoadTextureFromPath(string path)
    {
        // ���� ���Ͽ��� �ؽ�ó �ε�
        string filePath = "file://" + path;

        using (WWW www = new WWW(filePath))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                autoAssignTexture = www.texture; // �ؽ�ó �ε� ����
                photonView.RPC("AssignTextureToMaterialRPC", RpcTarget.AllBuffered); // ���� RPC ȣ��
            }
            else
            {
                Debug.LogError("Failed to load texture from path: " + path + "\nError: " + www.error);
            }
        }
    }

    [PunRPC]
    private void AssignTextureToMaterialRPC()
    {
        // ù ��° �ڽ��� ������
        Transform firstChild = transform.GetChild(0);

        // �ڽ� ������Ʈ�� �ִ��� Ȯ��
        if (firstChild != null && firstChild.name == "PicketPicture")
        {
            Renderer renderer = firstChild.GetComponent<Renderer>();

            if (renderer != null && renderer.material != null && autoAssignTexture != null)
            {
                renderer.material.SetTexture("_BaseMap", autoAssignTexture); // Base Map�� �ؽ�ó ����
                Debug.Log("Texture assigned to PicketPicture's material.");
            }
            else
            {
                Debug.LogWarning("Renderer, Material, or Texture is missing.");
            }
        }
        else
        {
            Debug.LogWarning("First child object is missing or not named 'PicketPicture'.");
        }
    }
}