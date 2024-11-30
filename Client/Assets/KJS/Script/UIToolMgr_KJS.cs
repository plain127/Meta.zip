using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToolMgr_KJS : MonoBehaviour
{
    private void OnDisable()
    {
        // toolUI가 비활성화될 때 캐릭터 이동 활성화
        PlayerMove playerMove = FindObjectOfType<PlayerMove>();
        if (playerMove != null)
        {
            playerMove.EnableMoving(true); // 캐릭터 이동 활성화
            Debug.Log("toolUI가 꺼졌습니다. 캐릭터 이동이 다시 활성화됩니다.");
        }

        // toolUI가 꺼졌을 때 카메라 상태 초기화
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            cameraManager.ResetCameraState(); // CameraManager의 상태 초기화
            Debug.Log("CameraManager 상태가 초기화되었습니다.");
        }
        AI_Movement_KJS aiMovement = FindObjectOfType<AI_Movement_KJS>();
        if (aiMovement != null)
        {
            // NavMeshAgent 활성화 및 상태 초기화
            aiMovement.ResetAgentState();
            Debug.Log("Tool UI가 꺼졌습니다. AI_Movement_KJS 상태를 초기화했습니다.");
        }
    }
}
