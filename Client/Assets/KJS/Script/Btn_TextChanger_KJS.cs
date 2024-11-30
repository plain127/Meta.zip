using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Btn_TextChanger_KJS : MonoBehaviour
{
    // 4개의 버튼 (비활성화 상태로 시작)
    public Button[] targetButtons;

    // 각 버튼이 활성화되는 기준 텍스트 (텍스트 입력값과 매칭)
    public string[] activationTexts;

    // ChatInputField와 입력 버튼
    public TMP_InputField chatInputField;
    public Button inputButton;

    private void Start()
    {
        // 입력 버튼에 클릭 메서드 연결
        if (inputButton != null)
        {
            inputButton.onClick.AddListener(OnInputButtonClick);
        }

        // 모든 버튼을 초기 상태에서 비활성화
        foreach (var button in targetButtons)
        {
            if (button != null)
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    // 입력 버튼 클릭 시 실행되는 메서드
    private void OnInputButtonClick()
    {
        if (chatInputField != null && !string.IsNullOrEmpty(chatInputField.text))
        {
            string inputText = chatInputField.text.Trim(); // 공백 제거

            // 모든 버튼 비활성화
            foreach (var button in targetButtons)
            {
                if (button != null)
                {
                    button.gameObject.SetActive(false);
                }
            }

            // 입력된 텍스트와 매칭되는 버튼만 활성화
            for (int i = 0; i < activationTexts.Length; i++)
            {
                if (inputText.Equals(activationTexts[i], System.StringComparison.OrdinalIgnoreCase) && i < targetButtons.Length)
                {
                    targetButtons[i].gameObject.SetActive(true); // 버튼 활성화
                    Debug.Log($"버튼 {i + 1} 활성화됨: {activationTexts[i]}"); // 디버그 로그 출력
                    break; // 하나의 버튼만 활성화
                }
            }
        }
        else
        {
            Debug.LogWarning("ChatInputField가 비어있거나 유효하지 않습니다.");
        }
    }
}