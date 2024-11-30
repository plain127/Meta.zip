using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastUI : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // ���콺 ������ ��ġ�� ���̸� �߻�
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // RaycastHit ����� ������ ����Ʈ
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();

        // UI ������Ʈ�� ����ĳ��Ʈ
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        // UI ������Ʈ�� �̸� ���
        foreach (var result in raycastResults)
        {
            Debug.Log("Hovered UI Object: " + result.gameObject.name);
        }
    }
}