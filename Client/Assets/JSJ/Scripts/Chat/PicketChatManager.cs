using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PicketChatManager : MonoBehaviourPun
{
    public static PicketChatManager instance;

    [Header("ä��")]
    public GameObject picketChatItemPrefab;   // ä�� Prefab
    public GameObject picketChatView;
    public RectTransform trChatView;
    public RectTransform trContent;
    public TMP_InputField inputPicketChat;

    // ä���� �߰��Ǳ� ���� Content �� H(����) ���� ������ �ִ� ����
    float prevContentH;

    // �г��� ����
    Color picket_nickNameColor;

    // ä�� ������
    bool isPicketChatting = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // �г��� ������ �����ϰ� ����
        picket_nickNameColor = Random.ColorHSV();

        if (inputPicketChat != null)
        {
            // inputPicketChat �� ���õ� �� ȣ��Ǵ� �Լ� ���
            inputPicketChat.onSelect.AddListener(OnSelect);
            // inputPicketChat �� ������ ����� �� ȣ��Ǵ� �Լ� ���
            inputPicketChat.onValueChanged.AddListener(OnValueChanged);
            // inputPicketChat ���͸� ���� �� ȣ��Ǵ� �Լ� ���
            inputPicketChat.onSubmit.AddListener(OnSubmit);
            // inputPicketChat �� ��Ŀ���� �Ҿ��� �� ȣ��Ǵ� �Լ� ���
            inputPicketChat.onDeselect.AddListener(OnDeselect);
        }
        
    }

    // -------------------------------------------------------------------------------------------------------------- [ OnSelect ]
    // inputChat �� ���õǾ��� �� ȣ��Ǵ� �Լ�
    void OnSelect(string s)
    {
        
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnValueChanged ]
    // ä�� ������ �Է� ���� ��
    void OnValueChanged(string s)
    {
        isPicketChatting = true;
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnSubmit ]
    // Enter�� ���� ��    
    void OnSubmit(string s)
    {
        // ���࿡ s �� ���̰� 0 �̸� �Լ��� ������.
        if (s.Length == 0) return;

        // ä�� ������ NickName : ä�� ����
        // "<collor=#ffffff> ���ϴ� ���� </color>"
        string nick = "<color=#" + ColorUtility.ToHtmlStringRGB(picket_nickNameColor) + ">" + PhotonNetwork.NickName + "</color>";
        string chat = nick + " : " + s;

        // AddChat RPC �Լ� ȣ��
        photonView.RPC(nameof(AddChat), RpcTarget.AllBuffered, chat);

        // inputChat �� �ִ� ������ �ʱ�ȭ
        inputPicketChat.text = "";

        // ������ inputChat �� Ȱ��ȭ ����
        inputPicketChat.ActivateInputField();
    }


    // -------------------------------------------------------------------------------------------------------------- [ Add Chat ]
    // ä�� �߰� �Լ�
    [PunRPC]
    void AddChat(string chat)
    {
        // ���ο� ä���� �߰��Ǳ� ���� Content �� H ���� ����
        prevContentH = trContent.sizeDelta.y;

        // ChatItem �ϳ� ������ (�θ� ChatView �� Content �� ����)
        GameObject go = Instantiate(picketChatItemPrefab, trContent);

        // ChatItem ������Ʈ ��������.
        PicketChatItem picketChatItem = go.GetComponent<PicketChatItem>();

        // ������ ������Ʈ�� SetText �Լ� ����
        picketChatItem.SetText(chat);

        // ������ ������Ʈ�� onAutoScroll ������ AutoScrollBottom �� ����
        picketChatItem.onAutoPicketScroll = AutoScrollBottom;
    }

    // ä�� �߰� �Ǿ��� �� �ǹ����� Content ��ġ�� �ű�� �Լ�
    public void AutoScrollBottom()
    {
        // chatView �� H ���� content �� H ���� ũ�ٸ� (��ũ���� ������ ���¶��)
        if (trContent.sizeDelta.y > trChatView.sizeDelta.y)
        {
            //trChatView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

            // ���� �ٴڿ� ����־��ٸ�
            if (prevContentH - trChatView.sizeDelta.y <= trContent.anchoredPosition.y)
            {
                // content �� y���� �缳���Ѵ�.
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trChatView.sizeDelta.y);
            }
        }
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnDeselect ]
    // inputChat �� ������ �Ҿ��� �� ȣ��Ǵ� �Լ�
    void OnDeselect(string s)
    {
        isPicketChatting = false;
       
    }

    // isChatting ���¸� ��ȯ�ϴ� �Լ�
    public bool IsPicketChatting()
    {
        return isPicketChatting;
    }

}
