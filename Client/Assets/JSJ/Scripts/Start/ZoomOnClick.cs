using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOnClick : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject canvasGlitch;
    public GameObject canvasSignIn;
   
    public Transform targetPos;

    public float zoomSpeed = 2f;

    public bool isZoom = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // Zoom 가능 상태이고 ZoomObject 를 클릭했다면,
                if (isZoom == true && hitInfo.collider.gameObject.layer == 20)
                {
                    StartCoroutine(ZoomInTarget());
                }
            }
        }
        
    }

    // 카메라 줌인 함수
    IEnumerator ZoomInTarget()
    {
        while (Vector3.Distance(mainCamera.transform.position, targetPos.position) > 0.01f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos.position, Time.deltaTime * zoomSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // 글리치 로고 비활성화
        canvasGlitch.SetActive(false);

        // 로그인 UI 활성화
        canvasSignIn.SetActive(true);

        // Zoom 불가능 상태
        isZoom = false;
    }
}
