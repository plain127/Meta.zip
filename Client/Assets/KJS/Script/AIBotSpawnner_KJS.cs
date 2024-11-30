using Photon.Pun;
using UnityEngine;

public class AIBotSpawner_KJS : MonoBehaviourPunCallbacks
{
    public string helperResourceName = "Helper"; // Resources ������ �ִ� ������ �̸�

    private void Start()
    {
        SpawnHelper(); // ���� ���۵� �� Helper ������ ����
    }

    private void SpawnHelper()
    {
        // ���� �÷��̾��� ��ġ�� ã��
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Vector3 spawnPosition = Vector3.zero; // �⺻ ���� ��ġ
        bool foundLocalPlayer = false;

        foreach (GameObject player in players)
        {
            PhotonView playerPhotonView = player.GetComponent<PhotonView>();
            if (playerPhotonView != null && playerPhotonView.IsMine)
            {
                // ���� �÷��̾� ��ġ �������� x�� -1��ŭ ������ ��ġ ���
                spawnPosition = player.transform.position + new Vector3(2f, 0f, 0f);
                foundLocalPlayer = true;
                break;
            }
        }

        // ���� �÷��̾ ã�Ҵ��� Ȯ��
        if (foundLocalPlayer)
        {
            GameObject helperPrefab = (GameObject)Resources.Load(helperResourceName);
            if (helperPrefab != null)
            {
                // ���� �÷��̾� ��ġ���� x�� -1��ŭ ������ ��ġ�� Helper �������� ��Ʈ��ũ �� ����
                PhotonNetwork.Instantiate(helperResourceName, spawnPosition, Quaternion.identity);
                Debug.Log($"Helper AI�� ��ġ {spawnPosition}���� �����Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogWarning("Helper �������� Resources �������� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("IsMine�� ���� �÷��̾ ã�� �� �����ϴ�. Helper AI ���� ����.");
        }
    }
}