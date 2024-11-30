using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class TextSizeController_KJS : MonoBehaviour
{
    public TextMeshProUGUI targetText; // ��� TMP �ؽ�Ʈ
    private RectTransform rectTransform;

    private float initialHeight; // �ʱ� �ؽ�Ʈ �ڽ� ����
    private float previousHeight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (targetText == null)
        {
            Debug.LogError("Target TextMeshProUGUI�� �Ҵ����ּ���.");
            return;
        }

        // �ʱ� �ؽ�Ʈ �ڽ� ũ�� ����
        initialHeight = rectTransform.rect.height;
        previousHeight = targetText.rectTransform.rect.height;

        Debug.Log($"�ʱ� �ؽ�Ʈ �ڽ� ����: {initialHeight}");
    }

    private void Start()
    {
        // �ؽ�Ʈ�� �ʱ� ���̸� ����
        AdjustHeight(initialHeight);
    }

    private void Update()
    {
        float currentHeight = targetText.rectTransform.rect.height;

        // �ؽ�Ʈ ���̰� ����Ǿ����� Ȯ��
        if (Mathf.Abs(currentHeight - previousHeight) > Mathf.Epsilon)
        {
            AdjustHeight(currentHeight); // ���ο� ���̷� �ؽ�Ʈ �ڽ� ũ�� ����
            previousHeight = currentHeight;
        }
    }

    private void AdjustHeight(float newHeight)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;

        // �� �ؽ�Ʈ ���̸� �ʱ� ���̿� ���Ͽ� ����
        sizeDelta.y = Mathf.Max(initialHeight, newHeight);
        rectTransform.sizeDelta = sizeDelta;

        Debug.Log($"�ؽ�Ʈ �ڽ� ũ�� ������: {sizeDelta.y}");

        // �ڽ� ������Ʈ�� ũ�⸦ ����ȭ
        AdjustChildrenSize(sizeDelta.y);
    }

    private void AdjustChildrenSize(float newHeight)
    {
        foreach (RectTransform child in rectTransform)
        {
            Vector2 childSizeDelta = child.sizeDelta;

            // �ڽ� ũ�⸦ �θ��� ���̿� �°� ���� (���⼱ ���̸� ����)
            childSizeDelta.y = newHeight;

            // �ڽ� RectTransform ������Ʈ
            child.sizeDelta = childSizeDelta;

            Debug.Log($"�ڽ� ������Ʈ ũ�� ������: {child.name}, ����: {childSizeDelta.y}");
        }
    }
}