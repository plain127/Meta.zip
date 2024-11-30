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
            // ������ ������ ���� �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            print("������ ������ ���� �Ǿ� ����");
        }
    }

    //----------------------------------------------------------------------- [ Lobby ]
    // Lobby ����
    public void JoinLobby(UserInfo userInfo)
    {
        PhotonNetwork.NickName = userInfo.nickName;
        print(PhotonNetwork.NickName);

        PhotonNetwork.JoinLobby();
    }

    // Lobby �� ������ �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ���� �Ϸ�!");

        JoinChannel();
    }


    //----------------------------------------------------------------------- [ Channel ]
    // Channel ����
    public void JoinChannel()
    {
        PhotonNetwork.LoadLevel("Meta_Channel_Scene");
    }


    //----------------------------------------------------------------------- [ Scrap Book ]
    // ScrapBook ���� �� ����
    public void JoinOrCreateRoom()
    {
        loadLevelName = "Meta_ScrapBook_Scene";

        string roomName = PhotonNetwork.NickName + "'s ScrapBook";

        // �� ���� �ɼ�
        RoomOptions roomOptions = new RoomOptions();
        // �濡 ��� �� �� �ִ� �ִ� �ο� ����
        roomOptions.MaxPlayers = 10;
        // �κ� ���� ���̰� �� ���̴�?
        roomOptions.IsVisible = true;
        // �濡 ������ �� �� �ִ�?
        roomOptions.IsOpen = true;

        // Room ���� or ����
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }


    //----------------------------------------------------------------------- [ Folder ]
    // Folder ����
    public void JoinFolder()
    {
        PhotonNetwork.LoadLevel("Meta_Folder_Scene");
    }


    //----------------------------------------------------------------------- [ Town ]
    // Town ���� �� ����
    public void JoinOrCreateTown()
    {
        loadLevelName = "Meta_Town_Scene";

        // �� ���� �ɼ�
        RoomOptions roomOptions = new RoomOptions();
        // �濡 ��� �� �� �ִ� �ִ� �ο� ����
        roomOptions.MaxPlayers = 20;
        // �κ� ���� ���̰� �� ���̴�?
        roomOptions.IsVisible = true;
        // �濡 ������ �� �� �ִ�?
        roomOptions.IsOpen = true;

        // Room ���� or ����
        PhotonNetwork.JoinOrCreateRoom("Town", roomOptions, TypedLobby.Default);
    }


    //----------------------------------------------------------------------- [ Magazine ]
    // Magazine ���� �� ����
    public void JoinOrCreateMagazine()
    {
        loadLevelName = "Meta_Magazine_Scene";

        // �� ���� �ɼ�
        RoomOptions roomOptions = new RoomOptions();
        // �濡 ��� �� �� �ִ� �ִ� �ο� ����
        roomOptions.MaxPlayers = 20;
        // �κ� ���� ���̰� �� ���̴�?
        roomOptions.IsVisible = true;
        // �濡 ������ �� �� �ִ�?
        roomOptions.IsOpen = true;

        // Room ���� or ����
        PhotonNetwork.JoinOrCreateRoom("TheEdit", roomOptions, TypedLobby.Default);
    }


    //----------------------------------------------------------------------- [ Create Room ]
    // �� ���� �������� �� ȣ��Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� ���� �Ϸ�");
    }

    // �� ���� �������� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");

        print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.LoadLevel(loadLevelName);
        // ��Ƽ�÷��� ������ ��� �� �ִ� ����
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

    
    // Room ���� ������ ȣ��Ǵ� �Լ�
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("�� ���ͼ� �̵� : " + roomNumber);

        // ������ ������ ���������� �ʴٸ�,
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            // ���� �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
        // �̹� ������ ������ �������ִٸ�,
        else
        {
            // Room �̵�
            RoomTransition();
        }
    }

    // ������ ������ �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("������ ������ �����߽��ϴ�.");

        // Room �̵�
        RoomTransition();
    }


    //----------------------------------------------------------------------- [ Room Transition ]
    // Room �̵� �Լ�
    public void RoomTransition()
    {
        // ���� ��ѹ��� 1�̰ų� 3�̸�, Channel �� �̵��Ѵ�.
        if (roomNumber == 1 || roomNumber == 3)
        {
            JoinChannel();
        }
        // ���� ��ѹ��� 2�̰ų� 5��, Folder �� �̵��Ѵ�.
        if (roomNumber == 2 || roomNumber == 5 )
        {
            JoinFolder();
        }
        // ���� ��ѹ��� 4�̸�, ScrapBook ���� �̵��Ѵ�.
        if (roomNumber == 4)
        {
            JoinOrCreateRoom();
        }
        // ���� ��ѹ��� 6�̸�, Magazine ���� �̵��Ѵ�.
        if (roomNumber == 6)
        {
            JoinOrCreateMagazine();
        }

        // �̵� �� �ʱ�ȭ
        roomNumber = 0;
    }
}
