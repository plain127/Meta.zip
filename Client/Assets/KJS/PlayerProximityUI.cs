using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProximityUI : MonoBehaviour
{
    public string playerTag = "Player"; // Player �±�
    public float activationDistance = 5.0f; // UI Ȱ��ȭ �Ÿ�
    public GameObject uiElement; // Ȱ��ȭ�� UI ���

    private void Update()
    {
        // �� �� ��� "Player" �±׸� ���� ������Ʈ ã��
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

        bool shouldActivateUI = false;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= activationDistance)
            {
                shouldActivateUI = true;
                break;
            }
        }

        // UI Ȱ��ȭ/��Ȱ��ȭ
        if (uiElement != null)
        {
            uiElement.SetActive(shouldActivateUI);
        }
    }
}
