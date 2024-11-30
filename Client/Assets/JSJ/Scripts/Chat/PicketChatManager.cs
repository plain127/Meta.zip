using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PicketChatManager : MonoBehaviourPun
{
    public static PicketChatManager instance;

    [Header("채팅")]
    public GameObject picketChatItemPrefab;   // 채팅 Prefab
    public GameObject picketChatView;
    public RectTransform trChatView;
    public RectTransform trContent;
    public TMP_InputField inputPicketChat;

    // 채팅이 추가되기 전의 Content 의 H(높이) 값을 가지고 있는 변수
    float prevContentH;

    // 닉네임 색상
    Color picket_nickNameColor;

    // 채팅 중인지
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
        // 닉네임 색상을 랜덤하게 설정
        picket_nickNameColor = Random.ColorHSV();

        if (inputPicketChat != null)
        {
            // inputPicketChat 이 선택될 때 호출되는 함수 등록
            inputPicketChat.onSelect.AddListener(OnSelect);
            // inputPicketChat 의 내용이 변경될 때 호출되는 함수 등록
            inputPicketChat.onValueChanged.AddListener(OnValueChanged);
            // inputPicketChat 엔터를 쳤을 때 호출되는 함수 등록
            inputPicketChat.onSubmit.AddListener(OnSubmit);
            // inputPicketChat 이 포커스를 잃었을 때 호출되는 함수 등록
            inputPicketChat.onDeselect.AddListener(OnDeselect);
        }
        
    }

    // -------------------------------------------------------------------------------------------------------------- [ OnSelect ]
    // inputChat 이 선택되었을 때 호출되는 함수
    void OnSelect(string s)
    {
        
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnValueChanged ]
    // 채팅 내용을 입력 중일 때
    void OnValueChanged(string s)
    {
        isPicketChatting = true;
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnSubmit ]
    // Enter를 쳤을 때    
    void OnSubmit(string s)
    {
        // 만약에 s 의 길이가 0 이면 함수를 나가자.
        if (s.Length == 0) return;

        // 채팅 내용을 NickName : 채팅 내용
        // "<collor=#ffffff> 원하는 내용 </color>"
        string nick = "<color=#" + ColorUtility.ToHtmlStringRGB(picket_nickNameColor) + ">" + PhotonNetwork.NickName + "</color>";
        string chat = nick + " : " + s;

        // AddChat RPC 함수 호출
        photonView.RPC(nameof(AddChat), RpcTarget.AllBuffered, chat);

        // inputChat 에 있는 내용을 초기화
        inputPicketChat.text = "";

        // 강제로 inputChat 을 활성화 하자
        inputPicketChat.ActivateInputField();
    }


    // -------------------------------------------------------------------------------------------------------------- [ Add Chat ]
    // 채팅 추가 함수
    [PunRPC]
    void AddChat(string chat)
    {
        // 새로운 채팅이 추가되기 전의 Content 의 H 값을 저장
        prevContentH = trContent.sizeDelta.y;

        // ChatItem 하나 만들자 (부모를 ChatView 의 Content 로 하자)
        GameObject go = Instantiate(picketChatItemPrefab, trContent);

        // ChatItem 컴포넌트 가져오자.
        PicketChatItem picketChatItem = go.GetComponent<PicketChatItem>();

        // 가져온 컴포넌트의 SetText 함수 실행
        picketChatItem.SetText(chat);

        // 가져온 컴포넌트의 onAutoScroll 변수에 AutoScrollBottom 을 설정
        picketChatItem.onAutoPicketScroll = AutoScrollBottom;
    }

    // 채팅 추가 되었을 때 맨밑으로 Content 위치를 옮기는 함수
    public void AutoScrollBottom()
    {
        // chatView 의 H 보다 content 의 H 값이 크다면 (스크롤이 가능한 상태라면)
        if (trContent.sizeDelta.y > trChatView.sizeDelta.y)
        {
            //trChatView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

            // 이전 바닥에 닿아있었다면
            if (prevContentH - trChatView.sizeDelta.y <= trContent.anchoredPosition.y)
            {
                // content 의 y값을 재설정한다.
                trContent.anchoredPosition = new Vector2(0, trContent.sizeDelta.y - trChatView.sizeDelta.y);
            }
        }
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnDeselect ]
    // inputChat 이 선택을 잃었을 때 호출되는 함수
    void OnDeselect(string s)
    {
        isPicketChatting = false;
       
    }

    // isChatting 상태를 반환하는 함수
    public bool IsPicketChatting()
    {
        return isPicketChatting;
    }

}
