using UnityEngine;

public class AdjustParentSizeWithHorizontalPadding : MonoBehaviour
{
    public RectTransform parentRectTransform; // 부모 RectTransform
    public RectTransform childRectTransform;  // 자식 RectTransform

    private Vector2 fixedInitialSize = new Vector2(250, 150); // 부모의 고정 초기 크기
    private Vector2 lastChildSize;                            // 자식의 마지막 크기

    public float horizontalPadding = 20f; // 좌우 여백

    void Start()
    {
        if (parentRectTransform == null || childRectTransform == null)
        {
            Debug.LogError("부모 또는 자식 RectTransform이 설정되지 않았습니다.");
            return;
        }

        // 부모의 크기를 고정 초기 크기로 설정
        parentRectTransform.sizeDelta = fixedInitialSize;

        // 자식의 초기 크기를 저장
        lastChildSize = childRectTransform.sizeDelta;
    }

    void Update()
    {
        // 자식의 현재 크기를 가져옴
        Vector2 currentChildSize = childRectTransform.sizeDelta;

        // 자식 크기가 변경되었는지 확인
        if (currentChildSize != lastChildSize)
        {
            // 자식 크기가 변경되었으므로 부모 크기를 업데이트
            UpdateParentSize(currentChildSize);

            // 마지막 자식 크기를 업데이트
            lastChildSize = currentChildSize;
        }
    }

    // 부모의 크기를 자식 크기에 맞게 업데이트하는 메서드
    void UpdateParentSize(Vector2 newChildSize)
    {
        Vector2 parentSize = parentRectTransform.sizeDelta;

        // 부모의 크기를 자식 크기 + 좌우 여백으로 업데이트
        parentSize.x = Mathf.Max(fixedInitialSize.x, newChildSize.x + horizontalPadding * 2); // 좌우 여백 적용
        parentSize.y = Mathf.Max(fixedInitialSize.y, newChildSize.y); // 높이는 그대로 유지 (여백 없음)

        parentRectTransform.sizeDelta = parentSize;

        Debug.Log($"부모 크기 업데이트 (좌우 여백 적용): {parentRectTransform.sizeDelta}");
    }

    // 오브젝트가 꺼질 때 부모 크기를 고정 초기값으로 리셋
    void OnDisable()
    {
        ResetParentSize();
    }

    // 오브젝트가 삭제될 때 부모 크기를 고정 초기값으로 리셋
    void OnDestroy()
    {
        ResetParentSize();
    }

    // 부모의 크기를 고정 초기값으로 리셋하는 메서드
    void ResetParentSize()
    {
        if (parentRectTransform != null)
        {
            parentRectTransform.sizeDelta = fixedInitialSize;
            Debug.Log($"부모 크기가 고정된 초기값으로 리셋되었습니다: {fixedInitialSize}");
        }
    }
}