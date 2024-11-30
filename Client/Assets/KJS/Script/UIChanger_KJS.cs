using UnityEngine;

public class AlternateUIController : MonoBehaviour
{
    public GameObject chatUI;    // 첫 번째 UI (Chat UI)
    public GameObject toolUI;    // 두 번째 UI (Tool UI)
    public GameObject anotherUI; // 세 번째 UI (특정 UI)

    private void Start()
    {
        // 초기 상태 설정: chatUI를 활성화하고 나머지는 비활성화
        if (chatUI != null) chatUI.SetActive(true);
        if (toolUI != null) toolUI.SetActive(false);
        if (anotherUI != null) anotherUI.SetActive(false);
    }

    private void Update()
    {
        // chatUI가 활성화되었을 때 toolUI와 anotherUI를 비활성화
        if (chatUI != null && chatUI.activeSelf)
        {
            if (toolUI != null) toolUI.SetActive(false);
            if (anotherUI != null) anotherUI.SetActive(false);
        }
        // toolUI가 활성화되었을 때 chatUI와 anotherUI를 비활성화
        else if (toolUI != null && toolUI.activeSelf)
        {
            if (chatUI != null) chatUI.SetActive(false);
            if (anotherUI != null) anotherUI.SetActive(false);
        }
        // anotherUI가 활성화되었을 때 toolUI만 비활성화
        else if (anotherUI != null && anotherUI.activeSelf)
        {
            if (toolUI != null) toolUI.SetActive(false);
        }
        // 모든 UI가 비활성화 상태일 경우 chatUI를 자동으로 다시 켬
        else if (chatUI != null && toolUI != null && anotherUI != null &&
                 !chatUI.activeSelf && !toolUI.activeSelf && !anotherUI.activeSelf)
        {
            chatUI.SetActive(true);
        }
    }
}