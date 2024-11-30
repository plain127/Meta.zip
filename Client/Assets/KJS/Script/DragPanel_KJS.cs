using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel_KJS : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    private bool isDragging = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>(); // 부모 캔버스를 가져옴
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 왼쪽 클릭일 때만 드래그 시작
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = true;
            originalPosition = rectTransform.anchoredPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 중이며 왼쪽 클릭일 때만 처리
        if (isDragging && canvas != null && eventData.button == PointerEventData.InputButton.Left)
        {
            // 캔버스의 렌더링 스케일을 고려한 드래그 처리
            Vector2 delta = eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition += delta;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 왼쪽 클릭에서 손을 뗐을 때 드래그 종료
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = false;
        }
    }
}
