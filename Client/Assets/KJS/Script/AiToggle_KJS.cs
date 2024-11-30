using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiToggle_KJS : MonoBehaviour
{
    public GameObject targetUI; // 켜거나 끌 UI 오브젝트

    private void Update()
    {
        // 'H' 키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.H))
        {
            // targetUI의 활성화 상태를 반전시킴
            if (targetUI != null)
            {
                bool isActive = targetUI.activeSelf;
                targetUI.SetActive(!isActive);
            }
        }
    }
}