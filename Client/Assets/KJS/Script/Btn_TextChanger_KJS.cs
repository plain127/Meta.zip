using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Btn_TextChanger_KJS : MonoBehaviour
{
    // 4���� ��ư (��Ȱ��ȭ ���·� ����)
    public Button[] targetButtons;

    // �� ��ư�� Ȱ��ȭ�Ǵ� ���� �ؽ�Ʈ (�ؽ�Ʈ �Է°��� ��Ī)
    public string[] activationTexts;

    // ChatInputField�� �Է� ��ư
    public TMP_InputField chatInputField;
    public Button inputButton;

    private void Start()
    {
        // �Է� ��ư�� Ŭ�� �޼��� ����
        if (inputButton != null)
        {
            inputButton.onClick.AddListener(OnInputButtonClick);
        }

        // ��� ��ư�� �ʱ� ���¿��� ��Ȱ��ȭ
        foreach (var button in targetButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    // �Է� ��ư Ŭ�� �� ����Ǵ� �޼���
    private void OnInputButtonClick()
    {
        if (chatInputField != null && !string.IsNullOrEmpty(chatInputField.text))
        {
            string inputText = chatInputField.text.Trim(); // ���� ����

            // ��� ��ư ��Ȱ��ȭ
            foreach (var button in targetButtons)
            {
                if (button != null)
                {
                    button.gameObject.SetActive(false);
                }
            }

            // �Էµ� �ؽ�Ʈ�� ��Ī�Ǵ� ��ư�� Ȱ��ȭ
            for (int i = 0; i < activationTexts.Length; i++)
            {
                if (inputText.Equals(activationTexts[i], System.StringComparison.OrdinalIgnoreCase) && i < targetButtons.Length)
                {
                    targetButtons[i].gameObject.SetActive(true); // ��ư Ȱ��ȭ
                    Debug.Log($"��ư {i + 1} Ȱ��ȭ��: {activationTexts[i]}"); // ����� �α� ���
                    break; // �ϳ��� ��ư�� Ȱ��ȭ
                }
            }
        }
        else
        {
            Debug.LogWarning("ChatInputField�� ����ְų� ��ȿ���� �ʽ��ϴ�.");
        }
    }
}