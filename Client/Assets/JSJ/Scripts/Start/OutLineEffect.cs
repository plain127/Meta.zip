using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineEffect : MonoBehaviour
{
    public Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();

        // �ƿ����� ��Ȱ��ȭ
        outline.enabled = false;
    }

    // Ŀ���� ������Ʈ�� �÷��� ��
    public void OnMouseEnter()
    {
        // �ƿ����� Ȱ��ȭ
        outline.enabled = true;
    }

    // Ŀ���� ������Ʈ���� ������ ��
    public void OnMouseExit()
    {
        // �ƿ����� ��Ȱ��ȭ
        outline.enabled = false;
    }
}
