using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToolMgr_KJS : MonoBehaviour
{
    private void OnDisable()
    {
        // toolUI�� ��Ȱ��ȭ�� �� ĳ���� �̵� Ȱ��ȭ
        PlayerMove playerMove = FindObjectOfType<PlayerMove>();
        if (playerMove != null)
        {
            playerMove.EnableMoving(true); // ĳ���� �̵� Ȱ��ȭ
            Debug.Log("toolUI�� �������ϴ�. ĳ���� �̵��� �ٽ� Ȱ��ȭ�˴ϴ�.");
        }

        // toolUI�� ������ �� ī�޶� ���� �ʱ�ȭ
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            cameraManager.ResetCameraState(); // CameraManager�� ���� �ʱ�ȭ
            Debug.Log("CameraManager ���°� �ʱ�ȭ�Ǿ����ϴ�.");
        }
        AI_Movement_KJS aiMovement = FindObjectOfType<AI_Movement_KJS>();
        if (aiMovement != null)
        {
            // NavMeshAgent Ȱ��ȭ �� ���� �ʱ�ȭ
            aiMovement.ResetAgentState();
            Debug.Log("Tool UI�� �������ϴ�. AI_Movement_KJS ���¸� �ʱ�ȭ�߽��ϴ�.");
        }
    }
}
