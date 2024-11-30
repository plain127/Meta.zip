using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;

public class ButtonPrefabSpawner2_KJS : MonoBehaviourPunCallbacks
{
    public string prefabResourcePath; // 각 버튼마다 로드할 프리팹의 Resources 경로
    private InventoryText_KJS inventoryText; // 부모의 InventoryText_KJS 스크립트 참조
    private Button button; // 버튼 컴포넌트 참조
    private static int currentPostIdIndex = 0; // POSTID 리스트 인덱스 추적
    private string assignedPostId; // 이 버튼에 할당된 postId

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
            Debug.LogError("InventoryText_KJS 스크립트를 부모 오브젝트에서 찾을 수 없습니다.");
            return;
        }

        AssignPostId();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private void AssignPostId()
    {
        //List<string> postIdList = inventoryText.inventoryPostIds;

        //if (postIdList == null || postIdList.Count == 0)
        //{
        //    Debug.LogWarning("POSTID 리스트가 비어있습니다.");
        //    return;
        //}

        //if (currentPostIdIndex < postIdList.Count)
        //{
        //    assignedPostId = postIdList[currentPostIdIndex];
        //    Debug.Log($"버튼 생성 순서에 따라 할당된 POSTID: {assignedPostId}");
        //    currentPostIdIndex++;
        //}
        //else
        //{
        //    Debug.LogWarning("POSTID 리스트의 모든 항목을 이미 사용했습니다.");
        //}
    }

    private void OnButtonClicked()
    {
        // 버튼을 클릭한 로컬 플레이어만 프리팹을 생성
        if (photonView.IsMine)
        {
            SpawnPrefabFromResource(assignedPostId);
        }
    }

    private void SpawnPrefabFromResource(string postId)
    {
        if (string.IsNullOrEmpty(prefabResourcePath))
        {
            Debug.LogError("프리팹의 Resources 경로가 설정되지 않았습니다.");
            return;
        }

        // 로컬 플레이어의 Transform 찾기
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
            Debug.LogError("로컬 플레이어의 위치를 찾을 수 없습니다.");
            return;
        }

        Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 0.5f;

        // 기본 회전 설정 (로컬 플레이어 기준으로 x축 -90도 회전 적용)
        Quaternion spawnRotation = playerTransform.rotation * Quaternion.Euler(-90, 0, 0);

        // PhotonNetwork.Instantiate를 사용하여 네트워크 객체 생성
        GameObject loadedObject = PhotonNetwork.Instantiate(prefabResourcePath, spawnPosition, spawnRotation);

        // RPC 호출로 모든 클라이언트에 태그와 postId, 그리고 회전값을 설정하도록 요청
        photonView.RPC("SetupSpawnedObject", RpcTarget.AllBuffered, loadedObject.GetComponent<PhotonView>().ViewID, postId, spawnRotation);
    }

    [PunRPC]
    private void SetupSpawnedObject(int viewID, string postId, Quaternion rotation)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView != null)
        {
            GameObject loadedObject = targetView.gameObject;

            // 설정된 회전값을 모든 클라이언트에서 적용
            loadedObject.transform.rotation = rotation;

            // 스케일을 20으로 설정
            loadedObject.transform.localScale = Vector3.one * 100;

            // 태그 설정
            loadedObject.tag = "Item";

            // PrefabManager_KJS 컴포넌트를 추가하고 postId 설정
            PrefabManager_KJS prefabManager = loadedObject.GetComponent<PrefabManager_KJS>();
            if (prefabManager == null)
            {
                prefabManager = loadedObject.AddComponent<PrefabManager_KJS>();
            }
            prefabManager.postId = postId;
            Debug.Log($"PrefabManager_KJS에 할당된 postId: {prefabManager.postId}");

            // Text 컴포넌트에 postId를 표시
            Text prefabText = loadedObject.GetComponentInChildren<Text>();
            if (prefabText != null)
            {
                prefabText.text = postId;
            }
            else
            {
                Debug.LogWarning("생성된 프리팹에 Text 컴포넌트가 없습니다.");
            }

            // URP 쉐이더 설정
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
                Debug.LogError("URP의 Lit 쉐이더를 찾을 수 없습니다. URP가 프로젝트에 적용되었는지 확인하세요.");
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