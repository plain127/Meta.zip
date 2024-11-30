using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProximityUI : MonoBehaviour
{
    public string playerTag = "Player"; // Player 태그
    public float activationDistance = 5.0f; // UI 활성화 거리
    public GameObject uiElement; // 활성화할 UI 요소

    private void Update()
    {
        // 씬 내 모든 "Player" 태그를 가진 오브젝트 찾기
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

        // UI 활성화/비활성화
        if (uiElement != null)
        {
            uiElement.SetActive(shouldActivateUI);
        }
    }
}
