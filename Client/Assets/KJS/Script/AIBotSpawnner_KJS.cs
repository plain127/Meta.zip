using Photon.Pun;
using UnityEngine;

public class AIBotSpawner_KJS : MonoBehaviourPunCallbacks
{
    public string helperResourceName = "Helper"; // Resources 폴더에 있는 프리팹 이름

    private void Start()
    {
        SpawnHelper(); // 씬이 시작될 때 Helper 프리팹 생성
    }

    private void SpawnHelper()
    {
        // 로컬 플레이어의 위치를 찾음
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Vector3 spawnPosition = Vector3.zero; // 기본 스폰 위치
        bool foundLocalPlayer = false;

        foreach (GameObject player in players)
        {
            PhotonView playerPhotonView = player.GetComponent<PhotonView>();
            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                // 로컬 플레이어 위치 기준으로 x값 -1만큼 떨어진 위치 계산
                spawnPosition = player.transform.position + new Vector3(2f, 0f, 0f);
                foundLocalPlayer = true;
                break;
            }
        }

        // 로컬 플레이어를 찾았는지 확인
        if (foundLocalPlayer)
        {
            GameObject helperPrefab = (GameObject)Resources.Load(helperResourceName);
            if (helperPrefab != null)
            {
                // 로컬 플레이어 위치에서 x값 -1만큼 떨어진 위치에 Helper 프리팹을 네트워크 상에 생성
                PhotonNetwork.Instantiate(helperResourceName, spawnPosition, Quaternion.identity);
                Debug.Log($"Helper AI가 위치 {spawnPosition}에서 생성되었습니다.");
            }
            else
            {
                Debug.LogWarning("Helper 프리팹을 Resources 폴더에서 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("IsMine인 로컬 플레이어를 찾을 수 없습니다. Helper AI 생성 실패.");
        }
    }
}