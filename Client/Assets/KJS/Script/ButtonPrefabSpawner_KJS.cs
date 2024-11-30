using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class ButtonPrefabSpawner_KJS : MonoBehaviour
{
    public string objPath; // �� ��ư���� �ε��� .obj ���� ��� (���� ���� ���)
    public string texturePath; // �� ��ư���� �ε��� �ؽ�ó ���� ��� (���� ���� ���)
    private InventoryText_KJS inventoryText; // �θ��� InventoryText_KJS ��ũ��Ʈ ����
    private Button button; // ��ư ������Ʈ ����
    private static int currentPostIdIndex = 0; // POSTID ����Ʈ �ε��� ����
    private string assignedPostId; // �� ��ư�� �Ҵ�� postId

    private string localObjSavePath; // ���ÿ� ����� .obj ���� ���
    private string localTextureSavePath; // ���ÿ� ����� �ؽ�ó ���� ���

    private void Awake()
    {
        button = GetComponent<Button>();

        // �θ� ������Ʈ���� InventoryText_KJS ������Ʈ ã��
        inventoryText = GetComponentInParent<InventoryText_KJS>();

        if (inventoryText == null)
        {
            Debug.LogError("InventoryText_KJS ��ũ��Ʈ�� �θ� ������Ʈ���� ã�� �� �����ϴ�.");
            return;
        }

        AssignPostId();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void AssignPostId()
    {
        //List<string> postIdList = inventoryText.inventoryPostIds;

    //    if (postIdList == null || postIdList.Count == 0)
    //    {
    //        Debug.LogWarning("POSTID ����Ʈ�� ����ֽ��ϴ�.");
    //        return;
    //    }

    //    if (currentPostIdIndex < postIdList.Count)
    //    {
    //        assignedPostId = postIdList[currentPostIdIndex];
    //        Debug.Log($"��ư ���� ������ ���� �Ҵ�� POSTID: {assignedPostId}");
    //        currentPostIdIndex++;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("POSTID ����Ʈ�� ��� �׸��� �̹� ����߽��ϴ�.");
    //    }
    }

    private void OnButtonClicked()
    {
        byte[] objData = File.Exists(objPath) ? File.ReadAllBytes(objPath) : null;
        byte[] textureData = File.Exists(texturePath) ? File.ReadAllBytes(texturePath) : null;

        if (objData == null)
        {
            Debug.LogError("������ ��ο� .obj ������ �������� �ʽ��ϴ�.");
            return;
        }

        SpawnPrefabFromObj(objData, textureData, assignedPostId);
    }

    private void SpawnPrefabFromObj(byte[] objData, byte[] textureData, string postId)
    {
        // ���� ���� ��� ����
        localObjSavePath = Path.Combine(Application.persistentDataPath, $"{postId}_model.obj");
        localTextureSavePath = Path.Combine(Application.persistentDataPath, $"{postId}_texture.png");

        // .obj ���� ���ÿ� ����
        File.WriteAllBytes(localObjSavePath, objData);
        Debug.Log($"������ ���ÿ� ����Ǿ����ϴ�: {localObjSavePath}");

        // �ؽ�ó ������ �ִ� ��� ���ÿ� ����
        if (textureData != null)
        {
            File.WriteAllBytes(localTextureSavePath, textureData);
            Debug.Log($"�ؽ�ó ������ ���ÿ� ����Ǿ����ϴ�: {localTextureSavePath}");
        }

        // ����� ������ �ε��ϰ� ������Ʈ�� ����
        LoadAndInstantiateModel(localObjSavePath, localTextureSavePath, postId);
    }

    private void LoadAndInstantiateModel(string objFilePath, string textureFilePath, string postId)
    {
        if (!File.Exists(objFilePath))
        {
            Debug.LogError("���ÿ� ����� .obj ������ ã�� �� �����ϴ�.");
            return;
        }

        GameObject loadedObject = new OBJLoader().Load(objFilePath);
        if (loadedObject == null)
        {
            Debug.LogError("OBJ ���� �ε忡 �����߽��ϴ�.");
            return;
        }

        // ������ ������Ʈ�� �±׸� "Item"���� ����
        loadedObject.tag = "Item";

        // �÷��̾� ��ġ���� ���� ���� Z ����(forward)���� 0.5��ŭ ������ ��ġ ���
        Vector3 spawnPosition = transform.parent.position + transform.parent.forward.normalized * 0.5f;
        loadedObject.transform.position = spawnPosition;
        loadedObject.transform.localScale = Vector3.one;

        // ������Ʈ�� �θ� ������Ʈ�� ���� ������ �ٶ󺸵��� ȸ�� ���� (�ʿ信 ���� ����)
        loadedObject.transform.rotation = transform.parent.rotation;

        // PrefabManager_KJS ������Ʈ�� �߰��ϰ� postId ����
        PrefabManager_KJS prefabManager = loadedObject.GetComponent<PrefabManager_KJS>();
        if (prefabManager == null)
        {
            prefabManager = loadedObject.AddComponent<PrefabManager_KJS>();
        }
        prefabManager.postId = postId;
        Debug.Log($"PrefabManager_KJS�� �Ҵ�� postId: {prefabManager.postId}");

        Text prefabText = loadedObject.GetComponentInChildren<Text>();
        if (prefabText != null)
        {
            prefabText.text = postId;
        }
        else
        {
            Debug.LogWarning("������ �����տ� Text ������Ʈ�� �����ϴ�.");
        }

        Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLitShader != null)
        {
            MeshRenderer[] meshRenderers = loadedObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in meshRenderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    mat.shader = urpLitShader;
                }
            }
        }
        else
        {
            Debug.LogError("URP�� Lit ���̴��� ã�� �� �����ϴ�. URP�� ������Ʈ�� ����Ǿ����� Ȯ���ϼ���.");
        }

        // �ؽ�ó ������ ������ �ε��Ͽ� ����
        if (!string.IsNullOrEmpty(textureFilePath) && File.Exists(textureFilePath))
        {
            byte[] fileData = File.ReadAllBytes(textureFilePath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData)) // �ؽ�ó �ε� ����
            {
                foreach (MeshRenderer renderer in loadedObject.GetComponentsInChildren<MeshRenderer>())
                {
                    foreach (Material mat in renderer.materials)
                    {
                        mat.mainTexture = texture; // �ؽ�ó �Ҵ�
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}