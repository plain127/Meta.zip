using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPos;   // 플레이어 생성 위치
    
    private void Awake()
    {
        int code = AvatarManager.instance.avatarCode;

        spawnPos = GameObject.Find("SpawnPos").transform;

        // 플레이어를 생성 (현재 Room 에 접속 되어있는 친구들도 보이게)
        player = PhotonNetwork.Instantiate("Player_" + code, spawnPos.position, Quaternion.identity);
    }
}
