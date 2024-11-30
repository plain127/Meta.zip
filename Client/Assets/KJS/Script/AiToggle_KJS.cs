using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiToggle_KJS : MonoBehaviour
{
    public GameObject targetUI; // �Ѱų� �� UI ������Ʈ

    private void Update()
    {
        // 'H' Ű�� ���ȴ��� Ȯ��
        if (Input.GetKeyDown(KeyCode.H))
        {
            // targetUI�� Ȱ��ȭ ���¸� ������Ŵ
            if (targetUI != null)
            {
                bool isActive = targetUI.activeSelf;
                targetUI.SetActive(!isActive);
            }
        }
    }
}