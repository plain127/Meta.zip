using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get_KJS : MonoBehaviour
{
    public float detectionRange = 5.0f; // ���� ����
    private GameObject targetItem = null; // ���� ��� �ִ� food ������Ʈ
    private bool isCarrying = false; // ������Ʈ�� ��� �ִ��� ����
    private float originalYPosition = 0.0f; // ������Ʈ�� �ʱ� Y�� ����

    void Update()
    {
        // G Ű�� ������ ������ ������Ʈ�� ���/���� ó��
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isCarrying)
                DetectAndPickUpFood(); // ���� �� ������Ʈ ���� �� ����
            else
                DropItem(); // ��� �ִ� ������Ʈ ����
        }

        // ������Ʈ�� ��� ���� ��, �÷��̾��� �������� �̵���Ŵ
        if (isCarrying && targetItem != null)
        {
            CarryFoodWithPlayer();
        }
    }

    // ���� ���� ���� ����� food ������Ʈ�� ã�� ���� �޼���
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
                targetItem = item; // ���� ����� food ����
            }
        }

        if (targetItem != null)
        {
            isCarrying = true;
            originalYPosition = targetItem.transform.position.y; // �ʱ� Y�� ����
            Debug.Log($"{targetItem.name}��(��) �������ϴ�.");
        }
    }

    // ��� �ִ� ������Ʈ�� �÷��̾��� �������� �̵���Ű�� �޼���
    void CarryFoodWithPlayer()
    {
        // �÷��̾ �ٶ󺸴� �������� �ణ �տ� ��ġ�ϵ��� ����
        Vector3 carryPosition = transform.position + transform.forward * 1f + new Vector3(0, 1, 0);
        targetItem.transform.position = carryPosition;

        // ���� X�� ȸ���� �����ϰ� Y, Z�� ȸ���� ĳ������ ȸ���� ������ ����
        Vector3 currentRotation = targetItem.transform.rotation.eulerAngles;
        targetItem.transform.rotation = Quaternion.Euler(currentRotation.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    // ��� �ִ� ������Ʈ�� �ʱ� Y������ ���� �޼���
    void DropItem()
    {
        if (targetItem != null)
        {
            // �ʱ� Y���� �����Ͽ� ������Ʈ�� ����
            Vector3 dropPosition = targetItem.transform.position;
            dropPosition.y = originalYPosition; // �ʱ� Y������ ����
            targetItem.transform.position = dropPosition;

            Debug.Log($"{targetItem.name}��(��) �������ҽ��ϴ�.");
            targetItem = null; // ���� ����
        }
        isCarrying = false;
    }
}