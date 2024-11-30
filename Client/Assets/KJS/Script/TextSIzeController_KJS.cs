using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class TextSizeController_KJS : MonoBehaviour
{
    public TextMeshProUGUI targetText; // 대상 TMP 텍스트
    private RectTransform rectTransform;

    private float initialHeight; // 초기 텍스트 박스 높이
    private float previousHeight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (targetText == null)
        {
            Debug.LogError("Target TextMeshProUGUI를 할당해주세요.");
            return;
        }

        // 초기 텍스트 박스 크기 저장
        initialHeight = rectTransform.rect.height;
        previousHeight = targetText.rectTransform.rect.height;

        Debug.Log($"초기 텍스트 박스 높이: {initialHeight}");
    }

    private void Start()
    {
        // 텍스트의 초기 높이를 설정
        AdjustHeight(initialHeight);
    }

    private void Update()
    {
        float currentHeight = targetText.rectTransform.rect.height;

        // 텍스트 높이가 변경되었는지 확인
        if (Mathf.Abs(currentHeight - previousHeight) > Mathf.Epsilon)
        {
            AdjustHeight(currentHeight); // 새로운 높이로 텍스트 박스 크기 조정
            previousHeight = currentHeight;
        }
    }

    private void AdjustHeight(float newHeight)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;

        // 새 텍스트 높이를 초기 높이와 비교하여 조정
        sizeDelta.y = Mathf.Max(initialHeight, newHeight);
        rectTransform.sizeDelta = sizeDelta;

        Debug.Log($"텍스트 박스 크기 조정됨: {sizeDelta.y}");

        // 자식 오브젝트의 크기를 동기화
        AdjustChildrenSize(sizeDelta.y);
    }

    private void AdjustChildrenSize(float newHeight)
    {
        foreach (RectTransform child in rectTransform)
        {
            Vector2 childSizeDelta = child.sizeDelta;

            // 자식 크기를 부모의 높이에 맞게 조정 (여기선 높이만 조정)
            childSizeDelta.y = newHeight;

            // 자식 RectTransform 업데이트
            child.sizeDelta = childSizeDelta;

            Debug.Log($"자식 오브젝트 크기 조정됨: {child.name}, 높이: {childSizeDelta.y}");
        }
    }
}