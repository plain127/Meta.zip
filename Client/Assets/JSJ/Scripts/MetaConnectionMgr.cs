using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;

public class MetaConnectionMgr : MonoBehaviourPunCallbacks
{
    public static MetaConnectionMgr instance;

    public string loadLevelName;

    int roomNumber = 0;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            // 마스터 서버에 접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            print("마스터 서버에 접속 되어 있음");
        }
    }

    //----------------------------------------------------------------------- [ Lobby ]
    // Lobby 입장
    public void JoinLobby(UserInfo userInfo)
    {
        PhotonNetwork.NickName = userInfo.nickName;
        print(PhotonNetwork.NickName);

        PhotonNetwork.JoinLobby();
    }

    // Lobby 에 접속을 성공하면 호출되는 함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 입장 완료!");

        JoinChannel();
    }


    //----------------------------------------------------------------------- [ Channel ]
    // Channel 입장
    public void JoinChannel()
    {
        PhotonNetwork.LoadLevel("Meta_Channel_Scene");
    }


    //----------------------------------------------------------------------- [ Scrap Book ]
    // ScrapBook 생성 후 입장
    public void JoinOrCreateRoom()
    {
        loadLevelName = "Meta_ScrapBook_Scene";

        string roomName = PhotonNetwork.NickName + "'s ScrapBook";

        // 방 생성 옵션
        RoomOptions roomOptions = new RoomOptions();
        // 방에 들어 올 수 있는 최대 인원 설정
        roomOptions.MaxPlayers = 10;
        // 로비에 방을 보이게 할 것이니?
        roomOptions.IsVisible = true;
        // 방에 참여를 할 수 있니?
        roomOptions.IsOpen = true;

        // Room 참여 or 생성
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }


    //----------------------------------------------------------------------- [ Folder ]
    // Folder 입장
    public void JoinFolder()
    {
        PhotonNetwork.LoadLevel("Meta_Folder_Scene");
    }


    //----------------------------------------------------------------------- [ Town ]
    // Town 생성 후 입장
    public void JoinOrCreateTown()
    {
        loadLevelName = "Meta_Town_Scene";

        // 방 생성 옵션
        RoomOptions roomOptions = new RoomOptions();
        // 방에 들어 올 수 있는 최대 인원 설정
        roomOptions.MaxPlayers = 20;
        // 로비에 방을 보이게 할 것이니?
        roomOptions.IsVisible = true;
        // 방에 참여를 할 수 있니?
        roomOptions.IsOpen = true;

        // Room 참여 or 생성
        PhotonNetwork.JoinOrCreateRoom("Town", roomOptions, TypedLobby.Default);
    }


    //----------------------------------------------------------------------- [ Magazine ]
    // Magazine 생성 후 입장
    public void JoinOrCreateMagazine()
    {
        loadLevelName = "Meta_Magazine_Scene";

        // 방 생성 옵션
        RoomOptions roomOptions = new RoomOptions();
        // 방에 들어 올 수 있는 최대 인원 설정
        roomOptions.MaxPlayers = 20;
        // 로비에 방을 보이게 할 것이니?
        roomOptions.IsVisible = true;
        // 방에 참여를 할 수 있니?
        roomOptions.IsOpen = true;

        // Room 참여 or 생성
        PhotonNetwork.JoinOrCreateRoom("TheEdit", roomOptions, TypedLobby.Default);
    }


    //----------------------------------------------------------------------- [ Create Room ]
    // 방 생성 성공했을 때 호출되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    // 방 입장 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");

        print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.LoadLevel(loadLevelName);
        // 멀티플레이 컨텐츠 즐길 수 있는 상태
    }


    //----------------------------------------------------------------------- [ Room Exit ]
    // ScrapBook -> Channel
    public void ScrapBookToChannel()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 1;
    }

    // ScrapBook -> Folder
    public void ScrapBookToFolder()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 2;
    }

    // Town -> Channel
    public void TownToChannel()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 3;
    }

    // Town -> ScrapBook
    public void TownToScrapBook()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 4;
    }

    // Town -> Folder
    public void TownToFolder()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 5;
    }

    // Town -> EyesMagazine
    public void TownToMagazine()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 6;
    }

    
    // Room 에서 나오면 호출되는 함수
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("방 나와서 이동 : " + roomNumber);

        // 마스터 서버에 접속해있지 않다면,
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            // 접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
        // 이미 마스터 서버에 접속해있다면,
        else
        {
            // Room 이동
            RoomTransition();
        }
    }

    // 마스터 서버에 접속하면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("마스터 서버에 접속했습니다.");

        // Room 이동
        RoomTransition();
    }


    //----------------------------------------------------------------------- [ Room Transition ]
    // Room 이동 함수
    public void RoomTransition()
    {
        // 만약 룸넘버가 1이거나 3이면, Channel 로 이동한다.
        if (roomNumber == 1 || roomNumber == 3)
        {
            JoinChannel();
        }
        // 만약 룸넘버가 2이거나 5면, Folder 로 이동한다.
        if (roomNumber == 2 || roomNumber == 5 )
        {
            JoinFolder();
        }
        // 만약 룸넘버가 4이면, ScrapBook 으로 이동한다.
        if (roomNumber == 4)
        {
            JoinOrCreateRoom();
        }
        // 만약 룸넘버가 6이면, Magazine 으로 이동한다.
        if (roomNumber == 6)
        {
            JoinOrCreateMagazine();
        }

        // 이동 후 초기화
        roomNumber = 0;
    }
}
