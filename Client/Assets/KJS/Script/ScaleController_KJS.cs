using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleController_KJS : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 initialScale;      // ������ ���� ��
    private Vector3 dragStartPos;      // ���콺 �巡�� ���� ��ġ
    private bool isDragging = false;   // ������ ���� ������ ����

    private GraphicRaycaster raycaster;  // UI�� Raycaster
    private EventSystem eventSystem;     // UI �̺�Ʈ �ý���

    void Awake()
    {
        raycaster = GetComponentInParent<GraphicRaycaster>();  // �θ� Canvas���� Raycaster ��������
        eventSystem = EventSystem.current;  // ���� EventSystem ����
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // ��Ŭ���̸� ������ ���� ����
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (IsPointerOverUIObject())  // �� ������Ʈ�� Ŭ���� ��쿡��
            {
                dragStartPos = Input.mousePosition;
                initialScale = transform.localScale;
                isDragging = true;
            }
        }
    }

    void Update()
    {
        if (isDragging)  // ������ ���� ���� ��
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 dragDelta = currentMousePos - dragStartPos;

            // X�� �̵� -> X�� ������ ���� (�ּ� 0.1 �̻� ���)
            float scaleX = Mathf.Max(0.1f, initialScale.x * (1 + dragDelta.x / 500f));
            // Y�� �̵� -> Y�� ������ ���� (�ּ� 0.1 �̻� ���)
            float scaleY = Mathf.Max(0.1f, initialScale.y * (1 + dragDelta.y / 500f));

            // ���ο� ������ ���� (Z���� ����)
            transform.localScale = new Vector3(scaleX, scaleY, initialScale.z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // ��Ŭ���� �����Ǹ� ������ ���� ����
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isDragging = false;
        }
    }

    // UI ������Ʈ�� Ŭ���Ǿ����� Ȯ���ϴ� �Լ�
    private bool IsPointerOverUIObject()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (var result in results)
        {
            if (result.gameObject == gameObject)  // �� ������Ʈ�� Ŭ���� ���
                return true;
        }

        return false;
    }
}
