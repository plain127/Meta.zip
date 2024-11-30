using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;

public class ButtonPrefabSpawner2_KJS : MonoBehaviourPunCallbacks
{
    public string prefabResourcePath; // �� ��ư���� �ε��� �������� Resources ���
    private InventoryText_KJS inventoryText; // �θ��� InventoryText_KJS ��ũ��Ʈ ����
    private Button button; // ��ư ������Ʈ ����
    private static int currentPostIdIndex = 0; // POSTID ����Ʈ �ε��� ����
    private string assignedPostId; // �� ��ư�� �Ҵ�� postId

    private void Start()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (inventoryText == null)
        {
            inventoryText = GetComponentInParent<InventoryText_KJS>();
        }

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

        //if (postIdList == null || postIdList.Count == 0)
        //{
        //    Debug.LogWarning("POSTID ����Ʈ�� ����ֽ��ϴ�.");
        //    return;
        //}

        //if (currentPostIdIndex < postIdList.Count)
        //{
        //    assignedPostId = postIdList[currentPostIdIndex];
        //    Debug.Log($"��ư ���� ������ ���� �Ҵ�� POSTID: {assignedPostId}");
        //    currentPostIdIndex++;
        //}
        //else
        //{
        //    Debug.LogWarning("POSTID ����Ʈ�� ��� �׸��� �̹� ����߽��ϴ�.");
        //}
    }

    private void OnButtonClicked()
    {
        // ��ư�� Ŭ���� ���� �÷��̾ �������� ����
        if (photonView.IsMine)
        {
            SpawnPrefabFromResource(assignedPostId);
        }
    }

    private void SpawnPrefabFromResource(string postId)
    {
        if (string.IsNullOrEmpty(prefabResourcePath))
        {
            Debug.LogError("�������� Resources ��ΰ� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ���� �÷��̾��� Transform ã��
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform playerTransform = null;

        foreach (GameObject player in players)
        {
            PhotonView playerPhotonView = player.GetComponent<PhotonView>();
            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                playerTransform = player.transform;
                break;
            }
        }

        if (playerTransform == null)
        {
            Debug.LogError("���� �÷��̾��� ��ġ�� ã�� �� �����ϴ�.");
            return;
        }

        Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;

        // �⺻ ȸ�� ���� (���� �÷��̾� �������� x�� -90�� ȸ�� ����)
        Quaternion spawnRotation = playerTransform.rotation * Quaternion.Euler(-90, 0, 0);

        // PhotonNetwork.Instantiate�� ����Ͽ� ��Ʈ��ũ ��ü ����
        GameObject loadedObject = PhotonNetwork.Instantiate(prefabResourcePath, spawnPosition, spawnRotation);

        // RPC ȣ��� ��� Ŭ���̾�Ʈ�� �±׿� postId, �׸��� ȸ������ �����ϵ��� ��û
        photonView.RPC("SetupSpawnedObject", RpcTarget.AllBuffered, loadedObject.GetComponent<PhotonView>().ViewID, postId, spawnRotation);
    }

    [PunRPC]
    private void SetupSpawnedObject(int viewID, string postId, Quaternion rotation)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            GameObject loadedObject = targetView.gameObject;

            // ������ ȸ������ ��� Ŭ���̾�Ʈ���� ����
            loadedObject.transform.rotation = rotation;

            // �������� 20���� ����
            loadedObject.transform.localScale = Vector3.one * 100;

            // �±� ����
            loadedObject.tag = "Item";

            // PrefabManager_KJS ������Ʈ�� �߰��ϰ� postId ����
            PrefabManager_KJS prefabManager = loadedObject.GetComponent<PrefabManager_KJS>();
            if (prefabManager == null)
            {
                prefabManager = loadedObject.AddComponent<PrefabManager_KJS>();
            }
            prefabManager.postId = postId;
            Debug.Log($"PrefabManager_KJS�� �Ҵ�� postId: {prefabManager.postId}");

            // Text ������Ʈ�� postId�� ǥ��
            Text prefabText = loadedObject.GetComponentInChildren<Text>();
            if (prefabText != null)
            {
                prefabText.text = postId;
            }
            else
            {
                Debug.LogWarning("������ �����տ� Text ������Ʈ�� �����ϴ�.");
            }

            // URP ���̴� ����
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