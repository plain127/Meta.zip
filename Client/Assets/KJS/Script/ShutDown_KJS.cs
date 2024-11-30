using UnityEngine;
using UnityEngine.UI;

public class ShutDown_KJS : MonoBehaviour
{
    public GameObject chatUI;  // 비활성화할 Chat UI
    public GameObject toolUI;  // 비활성화할 Tool UI
    public Button closeButton; // UI를 끌 버튼

    void Start()
    {
        // closeButton에 클릭 이벤트 등록
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseUI);
        }
    }

    // UI를 비활성화하는 메서드
    private void CloseUI()
    {
        if (chatUI != null)
        {
            chatUI.SetActive(false);
        }

        if (toolUI != null)
        {
            toolUI.SetActive(false);
        }

        Debug.Log("UI가 비활성화되었습니다.");
    }
}

