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
    public float targetFOV = 30f;       // 줌 후 카메라의 FOV (필드 오브 뷰)
    public float zoomSpeed = 2f;        // 줌 속도

    public bool isZoomedIn = false;    // 현재 줌 상태를 나타내는 플래그
    public bool isZooming = false;     // 현재 줌 동작 중인지 나타내는 플래그

    private Vector3 initialPosition;    // 카메라의 원래 위치 저장
    private float initialFOV;           // 카메라의 원래 FOV 저장



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
        // 초기 위치와 FOV 저장
        if (mainCamera == null) mainCamera = Camera.main;
        initialPosition = mainCamera.transform.position;
        initialFOV = mainCamera.fieldOfView;
    }


    public void OnClickZoom()
    {
        // 줌 동작 중에는 새 동작을 실행하지 않음
        if (isZooming) return;

        if (isZoomedIn)
        {
            // 줌 아웃
            StartCoroutine(ZoomCoroutine(initialPosition, initialFOV, false));
        }
        else
        {
            // 줌 인
            StartCoroutine(ZoomCoroutine(targetPosition, targetFOV, true));
        }
    }

    private IEnumerator ZoomCoroutine(Vector3 targetPos, float targetFOV, bool zoomingIn)
    {
        isZooming = true; // 줌 동작 시작
        Vector3 startPos = mainCamera.transform.position;
        float startFOV = mainCamera.fieldOfView;

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * zoomSpeed;

            // 위치와 FOV를 부드럽게 변화
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, elapsed);
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed);

            yield return null; // 다음 프레임 대기
        }

        // 최종 위치와 FOV 설정
        mainCamera.transform.position = targetPos;
        mainCamera.fieldOfView = targetFOV;

        isZooming = false; // 줌 동작 완료
        isZoomedIn = zoomingIn; // 줌 상태를 애니메이션 완료 후 설정
    }
}


