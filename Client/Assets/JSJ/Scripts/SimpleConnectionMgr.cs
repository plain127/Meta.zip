using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleConnectionMgr : MonoBehaviourPunCallbacks
{
    void Start()
    {
        // Photon ȯ�漳���� ������� ������ ������ ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Update()
    {

    }

    
    // ������ ������ ������ �Ǹ� ȣ��Ǵ� �Լ�
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("������ ������ ����");

        // �κ� ����
        JoinLobby();
    }

    public void JoinLobby()
    {
        // �г��� ����
        PhotonNetwork.NickName = "������";
        // �⺻ Lobby ����
        PhotonNetwork.JoinLobby();
    }

    // �κ� ������ �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ���� �Ϸ�");

        JoinOrCreateRoom();
    }

    // Room �� ��������. ���࿡ �ش� Room�� ������ Room ����ڴ�.
    public void JoinOrCreateRoom()
    {
        // �� ���� �ɼ�
        RoomOptions roomOptions = new RoomOptions();
        // �濡 ��� �� �� �ִ� �ִ� �ο� ����
        roomOptions.MaxPlayers = 20;
        // �κ� ���� ���̰� �� ���̴�?
        roomOptions.IsVisible = true;
        // �濡 ������ �� �� �ִ�?
        roomOptions.IsOpen = true;

        // Room ���� or ����
        PhotonNetwork.JoinOrCreateRoom("Meta4rd_Meta.zip_Room", roomOptions, TypedLobby.Default);
    }

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

        // ��Ƽ�÷��� ������ ��� �� �ִ� ����
        // ChannelScene���� �̵�!
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Meta_Town_Scene");
        }
    }

}
