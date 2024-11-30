using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraManager : MonoBehaviourPun
{
    public Camera mainCamera;
    


    public float followSpeed = 5f;
    public float zoomInSpeed = 2f; // 줌인 속도
    public float zoomInDistance = 10f; // 플레이어와의 줌인 거리

    private Vector3 offset;
    private PlayerMove playerMoveScript; // PlayerMove 스크립트를 참조

    private bool stopCameraMoving = false; // CameraMoving 중단 플래그
    private bool isZoomingIn = false; // MoveCameraToPosition에서 줌인 활성화 플래그
    private Vector3 targetPosition; // 줌인 목표 위치


    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        offset = mainCamera.transform.position - transform.position;

        // PlayerMove 스크립트 참조
        playerMoveScript = GetComponent<PlayerMove>();
    }

    void Update()
    {
        
        if (BillBoardView.inst != null && (BillBoardView.inst.isZooming || BillBoardView.inst.isZoomedIn)) return;
        // 내 것일 때만 카메라 컨트롤
        if (photonView.IsMine)
        {
            // ESC 키로 MoveCameraToPosition(줌인) 동작 중단
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isZoomingIn = false; // MoveCameraToPosition 줌인 중단
                stopCameraMoving = false; // CameraMoving 다시 활성화

                // PlayerMove 활성화
                if (playerMoveScript != null)
                {
                    playerMoveScript.EnableMoving(true);
                }
            }

            if (isZoomingIn)
            {
                ZoomInToPlayer(); // 줌인 진행
            }
            else if (!stopCameraMoving)
            {
                CameraMoving(); // 플레이어를 따라가는 기본 로직
            }
        }
    }

    // 카메라 이동 (플레이어 따라가기)
    public void CameraMoving()
    {
        Vector3 playerTargetPos = transform.position + offset;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, playerTargetPos, followSpeed * Time.deltaTime);
    }

    // 플레이어를 향해 줌인 시작
    public void MoveCameraToPosition()
    {
        if (mainCamera != null)
        {
            stopCameraMoving = true; // CameraMoving 중단
            isZoomingIn = true; // 줌인 활성화

            // PlayerMove 비활성화
            if (playerMoveScript != null)
            {
                playerMoveScript.EnableMoving(false);
            }

            // 줌인 목표 위치 설정 (플레이어를 향한 방향으로 이동)
            Vector3 directionToPlayer = (transform.position - mainCamera.transform.position).normalized; // 카메라에서 플레이어 방향 계산
            targetPosition = (transform.position - Vector3.right * 3)
                        - directionToPlayer * zoomInDistance
                        + Vector3.up * 2.0f;
        }
    }

    // 줌인 동작
    private void ZoomInToPlayer()
    {
        // 카메라가 목표 위치로 Lerp를 사용해 이동
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, zoomInSpeed * Time.deltaTime);

        // 목표 위치에 도달하면 줌인 중단
        if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.01f)
        {
            isZoomingIn = false; // 줌인 중단
        }

        if (playerMoveScript != null)
        {
            playerMoveScript.EnableMoving(false); // 이동 비활성화
            playerMoveScript.StartRotateToHelper(); // Helper 방향으로 회전
        }
    }
    public void ResetCameraState()
    {
        isZoomingIn = false; // MoveCameraToPosition 줌인 중단
        stopCameraMoving = false; // CameraMoving 다시 활성화

        // PlayerMove 활성화
        if (playerMoveScript != null)
        {
            playerMoveScript.EnableMoving(true);
        }

        Debug.Log("CameraManager 상태가 초기화되었습니다. (줌인 중단, 이동 활성화)");
    }
}