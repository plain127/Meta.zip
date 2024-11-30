using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get_KJS : MonoBehaviour
{
    public float detectionRange = 5.0f; // 감지 범위
    private GameObject targetItem = null; // 현재 들고 있는 food 오브젝트
    private bool isCarrying = false; // 오브젝트를 들고 있는지 여부
    private float originalYPosition = 0.0f; // 오브젝트의 초기 Y값 저장

    void Update()
    {
        // G 키가 눌리면 감지된 오브젝트를 들기/놓기 처리
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isCarrying)
                DetectAndPickUpFood(); // 범위 내 오브젝트 감지 및 집기
            else
                DropItem(); // 들고 있는 오브젝트 놓기
        }

        // 오브젝트를 들고 있을 때, 플레이어의 앞쪽으로 이동시킴
        if (isCarrying && targetItem != null)
        {
            CarryFoodWithPlayer();
        }
    }

    // 범위 내의 가장 가까운 food 오브젝트를 찾고 집는 메서드
    void DetectAndPickUpFood()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        float closestDistance = detectionRange;
        targetItem = null;

        foreach (GameObject item in items)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                targetItem = item; // 가장 가까운 food 저장
            }
        }

        if (targetItem != null)
        {
            isCarrying = true;
            originalYPosition = targetItem.transform.position.y; // 초기 Y값 저장
            Debug.Log($"{targetItem.name}을(를) 집었습니다.");
        }
    }

    // 들고 있는 오브젝트를 플레이어의 앞쪽으로 이동시키는 메서드
    void CarryFoodWithPlayer()
    {
        // 플레이어가 바라보는 방향으로 약간 앞에 위치하도록 설정
        Vector3 carryPosition = transform.position + transform.forward * 1f + new Vector3(0, 1, 0);
        targetItem.transform.position = carryPosition;

        // 기존 X축 회전을 유지하고 Y, Z축 회전만 캐릭터의 회전을 따르게 설정
        Vector3 currentRotation = targetItem.transform.rotation.eulerAngles;
        targetItem.transform.rotation = Quaternion.Euler(currentRotation.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    // 들고 있는 오브젝트를 초기 Y값으로 놓는 메서드
    void DropItem()
    {
        if (targetItem != null)
        {
            // 초기 Y값을 설정하여 오브젝트를 놓음
            Vector3 dropPosition = targetItem.transform.position;
            dropPosition.y = originalYPosition; // 초기 Y값으로 설정
            targetItem.transform.position = dropPosition;

            Debug.Log($"{targetItem.name}을(를) 내려놓았습니다.");
            targetItem = null; // 참조 해제
        }
        isCarrying = false;
    }
}