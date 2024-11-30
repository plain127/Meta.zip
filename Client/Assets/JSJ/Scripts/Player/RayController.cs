using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RayController : MonoBehaviour
{
    public Transform targetPos; // "InteractNotice" 오브젝트의 Transform
    public GameObject rawImage; // Canvas_Interactive의 첫 번째 자식인 RawImage 오브젝트
    public float noticeDistance = 15f; // 알림 활성화 거리

    void Start()
    {
        // "Meta_ScrapBook_Scene"일 경우 Canvas를 찾음
        if (SceneManager.GetActiveScene().name == "Meta_ScrapBook_Scene")
        {
            GameObject canvasNotice = GameObject.Find("Canvas_Interactive");
            if (canvasNotice != null && canvasNotice.transform.childCount > 0)
            {
                rawImage = canvasNotice.transform.GetChild(0).gameObject; // 첫 번째 자식을 RawImage로 할당
            }
        }

        // RawImage 초기 비활성화
        if (rawImage != null)
        {
            rawImage.SetActive(false);
            Debug.Log("[RayController] RawImage 비활성화 성공"); // 디버그 메시지
        }

        // "InteractNotice" 오브젝트를 찾아 targetPos에 할당
        GameObject interactNotice = GameObject.Find("InteractNotice");
        if (interactNotice != null)
        {
            targetPos = interactNotice.transform;
        }
    }

    void Update()
    {
        // 거리 기반으로 RawImage 활성화/비활성화
        CheckDistance();
    }

    public void CheckDistance()
    {
        if (targetPos != null && rawImage != null)
        {
            // 현재 위치와 InteractNotice 위치 간 거리 계산
            float distanceToNotice = Vector3.Distance(transform.position, targetPos.position);

            // 거리 조건에 따라 RawImage 활성화/비활성화
            if (distanceToNotice <= noticeDistance)
            {
                if (!rawImage.activeSelf) // 이미 활성화 상태가 아니라면 활성화
                {
                    rawImage.SetActive(true);
                }
            }
            else
            {
                if (rawImage.activeSelf) // 이미 비활성화 상태가 아니라면 비활성화
                {
                    rawImage.SetActive(false);
                }
            }
        }
    }
}