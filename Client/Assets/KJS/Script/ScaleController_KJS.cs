using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScaleController_KJS : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 initialScale;      // 스케일 시작 값
    private Vector3 dragStartPos;      // 마우스 드래그 시작 위치
    private bool isDragging = false;   // 스케일 조정 중인지 여부

    private GraphicRaycaster raycaster;  // UI용 Raycaster
    private EventSystem eventSystem;     // UI 이벤트 시스템

    void Awake()
    {
        raycaster = GetComponentInParent<GraphicRaycaster>();  // 부모 Canvas에서 Raycaster 가져오기
        eventSystem = EventSystem.current;  // 현재 EventSystem 참조
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 우클릭이면 스케일 조정 시작
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (IsPointerOverUIObject())  // 이 오브젝트가 클릭된 경우에만
            {
                dragStartPos = Input.mousePosition;
                initialScale = transform.localScale;
                isDragging = true;
            }
        }
    }

    void Update()
    {
        if (isDragging)  // 스케일 조정 중일 때
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 dragDelta = currentMousePos - dragStartPos;

            // X축 이동 -> X축 스케일 조정 (최소 0.1 이상만 허용)
            float scaleX = Mathf.Max(0.1f, initialScale.x * (1 + dragDelta.x / 500f));
            // Y축 이동 -> Y축 스케일 조정 (최소 0.1 이상만 허용)
            float scaleY = Mathf.Max(0.1f, initialScale.y * (1 + dragDelta.y / 500f));

            // 새로운 스케일 적용 (Z축은 고정)
            transform.localScale = new Vector3(scaleX, scaleY, initialScale.z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 우클릭이 해제되면 스케일 조정 종료
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isDragging = false;
        }
    }

    // UI 오브젝트가 클릭되었는지 확인하는 함수
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
            if (result.gameObject == gameObject)  // 이 오브젝트가 클릭된 경우
                return true;
        }

        return false;
    }
}
