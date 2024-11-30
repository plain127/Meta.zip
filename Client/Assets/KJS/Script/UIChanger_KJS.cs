using UnityEngine;

public class AlternateUIController : MonoBehaviour
{
    public GameObject chatUI;    // ù ��° UI (Chat UI)
    public GameObject toolUI;    // �� ��° UI (Tool UI)
    public GameObject anotherUI; // �� ��° UI (Ư�� UI)

    private void Start()
    {
        // �ʱ� ���� ����: chatUI�� Ȱ��ȭ�ϰ� �������� ��Ȱ��ȭ
        if (chatUI != null) chatUI.SetActive(true);
        if (toolUI != null) toolUI.SetActive(false);
        if (anotherUI != null) anotherUI.SetActive(false);
    }

    private void Update()
    {
        // chatUI�� Ȱ��ȭ�Ǿ��� �� toolUI�� anotherUI�� ��Ȱ��ȭ
        if (chatUI != null && chatUI.activeSelf)
        {
            if (toolUI != null) toolUI.SetActive(false);
            if (anotherUI != null) anotherUI.SetActive(false);
        }
        // toolUI�� Ȱ��ȭ�Ǿ��� �� chatUI�� anotherUI�� ��Ȱ��ȭ
        else if (toolUI != null && toolUI.activeSelf)
        {
            if (chatUI != null) chatUI.SetActive(false);
            if (anotherUI != null) anotherUI.SetActive(false);
        }
        // anotherUI�� Ȱ��ȭ�Ǿ��� �� toolUI�� ��Ȱ��ȭ
        else if (anotherUI != null && anotherUI.activeSelf)
        {
            if (toolUI != null) toolUI.SetActive(false);
        }
        // ��� UI�� ��Ȱ��ȭ ������ ��� chatUI�� �ڵ����� �ٽ� ��
        else if (chatUI != null && toolUI != null && anotherUI != null &&
                 !chatUI.activeSelf && !toolUI.activeSelf && !anotherUI.activeSelf)
        {
            chatUI.SetActive(true);
        }
    }
}