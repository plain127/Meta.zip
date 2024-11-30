using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChatToggle_KJS : MonoBehaviour
{
    public GameObject targetUI; // ����� UI ������Ʈ

    // ��ư�� ������ �Լ�
    public void ToggleUI()
    {
        if (targetUI != null)
        {
            bool isActive = targetUI.activeSelf;
            targetUI.SetActive(!isActive);
        }
    }
}
