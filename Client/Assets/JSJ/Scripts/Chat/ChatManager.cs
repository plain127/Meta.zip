using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ChatManager : MonoBehaviourPun
{
    public static ChatManager instance;

    public GameObject player;

    public Camera mainCamera;


    [Header("채팅")]
    public GameObject chatItemFactory;   // 채팅 Prefab
    public GameObject chatView;
    public RectTransform trChatView;
    public RectTransform trContent;
    public TMP_InputField inputChat;


    [Header("말풍선")]
    public GameObject chatBubble;   // 말풍선 Prefab
    

    // 현재 활성화된 말풍선
    GameObject currentBubble;

    // 채팅이 추가되기 전의 Content 의 H(높이) 값을 가지고 있는 변수
    float prevContentH;

    // 채팅 중인지
    bool isChatting = false;

    // 닉네임 색상
    Color nickNameColor;

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
        player = FindObjectOfType<GameManager>().player;

        mainCamera = Camera.main;

        // 닉네임 색상을 랜덤하게 설정
        nickNameColor = Random.ColorHSV();

        // ChatView 가 할당되었다면,
        if (chatView != null)
        {
            // chatView 비활성화
            chatView.SetActive(false);
        }

        // inputChat 이 할당되었다면,
        if (inputChat != null)
        {
            // inputChat 이 선택될 때 호출되는 함수 등록
            inputChat.onSelect.AddListener(OnSelect);
            // inputChat 의 내용이 변경될 때 호출되는 함수 등록
            inputChat.onValueChanged.AddListener(OnValueChanged);
            // inputChat 엔터를 쳤을 때 호출되는 함수 등록
            inputChat.onSubmit.AddListener(OnSubmit);
            // inputChat 이 포커스를 잃었을 때 호출되는 함수 등록
            inputChat.onDeselect.AddListener(OnDeselect);
        }
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnSelect ]
    // inputChat 이 선택되었을 때 호출되는 함수
    void OnSelect(string s)
    {
        // chatView 활성화
        chatView.SetActive(true);
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnValueChanged ]
    // 채팅 내용을 입력 중일 때
    void OnValueChanged(string s)
    {
        isChatting = true;
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnSubmit ]
    // Enter를 쳤을 때    
    void OnSubmit(string s)
    {
        // 만약에 s 의 길이가 0 이면 함수를 나가자.
        if (s.Length == 0) return;

        // 채팅 내용을 NickName : 채팅 내용
        // "<collor=#ffffff> 원하는 내용 </color>"
        //string nick = "<color=#" + ColorUtility.ToHtmlStringRGB(nickNameColor) + ">" + PhotonNetwork.NickName + "</color>";
        string nick = "<color=#" + ColorUtility.ToHtmlStringRGB(nickNameColor) + ">" + PhotonNetwork.NickName + "</color>";
        string chat = nick + " : " + s;
        
        // 현재 플레이어의 PhotonView ID 를 가져옴
        int photonviewID = player.GetComponent<PhotonView>().ViewID;
        
        // AddChat RPC 함수 호출
        photonView.RPC(nameof(AddChat), RpcTarget.AllBuffered, chat);
        
        // AddBubble RPC 함수 호출
        photonView.RPC(nameof(AddBubble), RpcTarget.All, chat, photonviewID);
        
        // inputChat 에 있는 내용을 초기화
        inputChat.text = "";

        // 강제로 inputChat 을 활성화 하자
        inputChat.ActivateInputField();
    }


    // -------------------------------------------------------------------------------------------------------------- [ Add Chat ]
    // 채팅 추가 함수
    [PunRPC]
    void AddChat(string chat)
    {
        // chatView 활성화
        chatView.SetActive(true);

        // 새로운 채팅이 추가되기 전의 Content 의 H 값을 저장
        prevContentH = trContent.sizeDelta.y;

        // ChatItem 하나 만들자 (부모를 ChatView 의 Content 로 하자)
        GameObject go = Instantiate(chatItemFactory, trContent);
        
        // ChatItem 컴포넌트 가져오자.
        ChatItem chatItem = go.GetComponent<ChatItem>();
       
        // 가져온 컴포넌트의 SetText 함수 실행
        chatItem.SetText(chat);
        
        // 가져온 컴포넌트의 onAutoScroll 변수에 AutoScrollBottom 을 설정
        chatItem.onAutoScroll = AutoScrollBottom;
    }

    // 채팅 추가 되었을 때 맨밑으로 Content 위치를 옮기는 함수
    public void AutoScrollBottom()
    {
        // chatView 의 H 보다 content 의 H 값이 크다면 (스크롤이 가능한 상태라면)
        if(trContent.sizeDelta.y > trChatView.sizeDelta.y)
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


    // -------------------------------------------------------------------------------------------------------------- [ Add Bubble ]
    // 말풍선 함수
    [PunRPC]
    void AddBubble(string chat, int photonviewID)
    {
        // PhotonView ID 로 캐릭터 식별
        PhotonView targetView = PhotonView.Find(photonviewID);

        Transform targetPlayer = targetView.transform;

        // 말풍선 부모 확인
        Transform targetBubbleParent = targetPlayer.Find("bubbleParent");

        // 현재 활성화된 말풍선이 있다면,
        if (currentBubble != null)
        {
            // 삭제하라.
            Destroy(currentBubble);
        }

        // 새로운 말풍선 생성
        currentBubble = Instantiate(chatBubble, targetBubbleParent, false);

        // 채팅 내용 설정
        TMP_Text chatText = currentBubble.GetComponentInChildren<TMP_Text>();
        chatText.text = chat;


        // 플레이어 위치
        Vector3 playerPos = targetPlayer.position;
        // 플레이어 방향
        Vector3 forwardDirection = targetPlayer.forward;

        // 만약, 플레이어가 앞을 바라보고 있다면
        if (Vector3.Dot(forwardDirection, Vector3.forward) > 0.9f)
        {
            Debug.Log("플레이어가 앞을 보고 있습니다.");

            Vector3 leftPos = playerPos - transform.right * 1f + new Vector3(0, 3f, 0); 

            // 말풍선은 플레이어의 왼쪽에 위치한다.
            currentBubble.transform.position = leftPos;
            
            // 카메라를 향해 말풍선 회전
            if (mainCamera != null)
            {
                Vector3 directionToCamera = mainCamera.transform.position - currentBubble.transform.position;
                currentBubble.transform.rotation = Quaternion.LookRotation(directionToCamera);
            }

            // 말풍선 text 회전
            chatText.rectTransform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        // 아니면 만약, 플레이어가 뒤를 바라보고 있다면
        else if (Vector3.Dot(forwardDirection, Vector3.forward) < -0.9f)
        {
            Debug.Log("플레이어가 뒤를 보고 있습니다.");

            Vector3 rightPos = playerPos + transform.right * 1f + new Vector3(0, 3f, 0);

            // 말풍선은 플레이어의 오른쪽에 위치한다.
            currentBubble.transform.position = rightPos;

            // 카메라를 향해 말풍선 회전
            if (mainCamera != null)
            {
                Vector3 directionToCamera = currentBubble.transform.position - mainCamera.transform.position;
                currentBubble.transform.rotation = Quaternion.LookRotation(directionToCamera);
            }

            // 말풍선 text 회전
            chatText.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
        // 3초 후에 말풍선 삭제
        StartCoroutine(DestroyBubble(5f));
    }

    // 말풍선 삭제 함수
    IEnumerator DestroyBubble(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentBubble != null)
        {
            Destroy(currentBubble);
        }
    }


    // -------------------------------------------------------------------------------------------------------------- [ OnDeselect ]
    // inputChat 이 선택을 잃었을 때 호출되는 함수
    void OnDeselect(string s)
    {
        // chatView 비활성화
        chatView.SetActive(false);

        isChatting = false;
    }


    // isChatting 상태를 반환하는 함수
    public bool IsChatting()
    {
        return isChatting;
    }
}
