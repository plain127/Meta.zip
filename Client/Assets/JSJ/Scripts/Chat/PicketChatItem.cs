using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PicketChatItem : MonoBehaviour
{
    // Text
    TMP_Text chatText;

    // �Ű����� �Լ� ���� ����
    public Action onAutoPicketScroll;

    private void Awake()
    {
        // Text ������Ʈ ��������
        chatText = GetComponent<TMP_Text>();
    }

    public void SetText(string s)
    {
        // �ؽ�Ʈ ����
        chatText.text = s;

        // ������ ���� �ڷ�ƾ ����
        StartCoroutine(UpdateSize());
    }

    IEnumerator UpdateSize()
    {
        yield return null;

        // �ؽ�Ʈ�� ���뿡 ���缭 ũ�⸦ ����
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, chatText.preferredHeight);

        yield return null;

        // ���࿡ onAutoScroll �� �Լ��� ����ִٸ�
        if (onAutoPicketScroll != null)
        {
            onAutoPicketScroll();
        }
    }
}
