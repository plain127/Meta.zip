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
        // �ؽ�Ʈ�� ���� ũ�� ��������
        Vector2 textSize = text_Chat.GetPreferredValues();

        // �ؽ�Ʈ �ڽ��� ���� ũ�� ����
        img_Bubble.sizeDelta = new Vector2(textSize.x + padding, img_Bubble.sizeDelta.y);

        // �ǹ��� (0, 0.5)�̹Ƿ�, �ؽ�Ʈ �ڽ��� ���� �� ��ġ ����
        img_Bubble.anchoredPosition = new Vector2(0, img_Bubble.anchoredPosition.y);
    }
}
