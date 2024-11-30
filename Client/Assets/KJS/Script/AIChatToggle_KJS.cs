using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChatToggle_KJS : MonoBehaviour
{
    public GameObject targetUI; // 토글할 UI 오브젝트

    // 버튼에 연결할 함수
    public void ToggleUI()
    {
        if (targetUI != null)
        {
            bool isActive = targetUI.activeSelf;
            targetUI.SetActive(!isActive);
        }
    }
}
