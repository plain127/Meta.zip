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
        canvas = GetComponentInParent<Canvas>(); // �θ� ĵ������ ������
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ���� Ŭ���� ���� �巡�� ����
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = true;
            originalPosition = rectTransform.anchoredPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �巡�� ���̸� ���� Ŭ���� ���� ó��
        if (isDragging && canvas != null && eventData.button == PointerEventData.InputButton.Left)
        {
            // ĵ������ ������ �������� ����� �巡�� ó��
            Vector2 delta = eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition += delta;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ���� Ŭ������ ���� ���� �� �巡�� ����
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = false;
        }
    }
}
