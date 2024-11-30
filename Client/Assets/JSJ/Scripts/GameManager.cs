using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPos;   // �÷��̾� ���� ��ġ
    
    private void Awake()
    {
        int code = AvatarManager.instance.avatarCode;

        spawnPos = GameObject.Find("SpawnPos").transform;

        // �÷��̾ ���� (���� Room �� ���� �Ǿ��ִ� ģ���鵵 ���̰�)
        player = PhotonNetwork.Instantiate("Player_" + code, spawnPos.position, Quaternion.identity);
    }
}
