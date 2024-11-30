using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolMgr_KJS : MonoBehaviour
{
    public GameObject editorPanel; // Ȱ��ȭ/��Ȱ��ȭ�� �г�

    // �г��� Ȱ��ȭ ���¸� ����ϴ� �Լ�
    public void OnClickTogglePanel()
    {
        bool isActive = editorPanel.activeSelf;
        editorPanel.SetActive(!isActive);

        // �г��� �ٽ� Ȱ��ȭ��ų �� ��ġ�� (0, 0)���� �̵�
        if (!isActive)
        {
            RectTransform rectTransform = editorPanel.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.zero;
            }
        }
    }
}

