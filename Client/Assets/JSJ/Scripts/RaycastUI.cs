using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastUI : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // 마우스 포인터 위치로 레이를 발사
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // RaycastHit 결과를 저장할 리스트
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();

        // UI 오브젝트에 레이캐스트
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        // UI 오브젝트의 이름 출력
        foreach (var result in raycastResults)
        {
            Debug.Log("Hovered UI Object: " + result.gameObject.name);
        }
    }
}