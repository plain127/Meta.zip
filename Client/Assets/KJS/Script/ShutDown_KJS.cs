using UnityEngine;
using UnityEngine.UI;

public class ShutDown_KJS : MonoBehaviour
{
    public GameObject chatUI;  // ��Ȱ��ȭ�� Chat UI
    public GameObject toolUI;  // ��Ȱ��ȭ�� Tool UI
    public Button closeButton; // UI�� �� ��ư

    void Start()
    {
        // closeButton�� Ŭ�� �̺�Ʈ ���
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseUI);
        }
    }

    // UI�� ��Ȱ��ȭ�ϴ� �޼���
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

        Debug.Log("UI�� ��Ȱ��ȭ�Ǿ����ϴ�.");
    }
}

