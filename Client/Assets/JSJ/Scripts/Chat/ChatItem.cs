using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChatItem : MonoBehaviour
{
    // Text
    TMP_Text chatText;

    // 매개없는 함수 담을 변수
    public Action onAutoScroll;

    private void Awake()
    {
        // Text 컴포넌트 가져오자
        chatText = GetComponent<TMP_Text>();
    }

    public void SetText(string s)
    {
        // 텍스트 갱신
        chatText.text = s;

        // 사이즈 조절 코루틴 실행
        StartCoroutine(UpdateSize());
    }

    IEnumerator UpdateSize()
    {
        yield return null;

        // 텍스트의 내용에 맞춰서 크기를 조절
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);

        yield return null;

        // 만약에 onAutoScroll 에 함수가 들어있다면
        if(onAutoScroll != null)
        {
            onAutoScroll();
        }

        #region ChatView 게임 오브젝트 찾자
        //GameObject go = GameObject.Find("ChatView");
        //// 찾은 오브젝트에서 ChatManager 컴포넌트 가져오자
        //ChatManager cm = go.GetComponent<ChatManager>();
        //// 가져온 컴포넌트 AutoScrollBottom 함수 호출
        //cm.AutoScrollBottom();
        #endregion 
    }
}
