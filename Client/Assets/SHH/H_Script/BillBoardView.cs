using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BillBoardView : MonoBehaviour
{

    public static BillBoardView inst;
    public Camera mainCamera;
    public Vector3 targetPosition;
    public float targetFOV = 30f;       // �� �� ī�޶��� FOV (�ʵ� ���� ��)
    public float zoomSpeed = 2f;        // �� �ӵ�

    public bool isZoomedIn = false;    // ���� �� ���¸� ��Ÿ���� �÷���
    public bool isZooming = false;     // ���� �� ���� ������ ��Ÿ���� �÷���

    private Vector3 initialPosition;    // ī�޶��� ���� ��ġ ����
    private float initialFOV;           // ī�޶��� ���� FOV ����



    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        // �ʱ� ��ġ�� FOV ����
        if (mainCamera == null) mainCamera = Camera.main;
        initialPosition = mainCamera.transform.position;
        initialFOV = mainCamera.fieldOfView;
    }


    public void OnClickZoom()
    {
        // �� ���� �߿��� �� ������ �������� ����
        if (isZooming) return;

        if (isZoomedIn)
        {
            // �� �ƿ�
            StartCoroutine(ZoomCoroutine(initialPosition, initialFOV, false));
        }
        else
        {
            // �� ��
            StartCoroutine(ZoomCoroutine(targetPosition, targetFOV, true));
        }
    }

    private IEnumerator ZoomCoroutine(Vector3 targetPos, float targetFOV, bool zoomingIn)
    {
        isZooming = true; // �� ���� ����
        Vector3 startPos = mainCamera.transform.position;
        float startFOV = mainCamera.fieldOfView;

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * zoomSpeed;

            // ��ġ�� FOV�� �ε巴�� ��ȭ
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsed);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed);

            yield return null; // ���� ������ ���
        }

        // ���� ��ġ�� FOV ����
        mainCamera.transform.position = targetPos;
        mainCamera.fieldOfView = targetFOV;

        isZooming = false; // �� ���� �Ϸ�
        isZoomedIn = zoomingIn; // �� ���¸� �ִϸ��̼� �Ϸ� �� ����
    }
}


