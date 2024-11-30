using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolMgr_KJS : MonoBehaviour
{
    public GameObject editorPanel; // 활성화/비활성화할 패널

    // 패널의 활성화 상태를 토글하는 함수
    public void OnClickTogglePanel()
    {
        bool isActive = editorPanel.activeSelf;
        editorPanel.SetActive(!isActive);

        // 패널을 다시 활성화시킬 때 위치를 (0, 0)으로 이동
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

