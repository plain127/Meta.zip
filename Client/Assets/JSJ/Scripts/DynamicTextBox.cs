using TMPro;
using UnityEngine;

public class DynamicTextBox : MonoBehaviour
{
    public RectTransform img_Bubble;
    public TextMeshProUGUI text_Chat; 
    public float padding = 2f; 

    void Update()
    {
        AdjustTextBoxSize();
    }

    void AdjustTextBoxSize()
    {
        // 텍스트의 실제 크기 가져오기
        Vector2 textSize = text_Chat.GetPreferredValues();

        // 텍스트 박스의 가로 크기 조정
        img_Bubble.sizeDelta = new Vector2(textSize.x + padding, img_Bubble.sizeDelta.y);

        // 피벗이 (0, 0.5)이므로, 텍스트 박스의 왼쪽 끝 위치 유지
        img_Bubble.anchoredPosition = new Vector2(0, img_Bubble.anchoredPosition.y);
    }
}
