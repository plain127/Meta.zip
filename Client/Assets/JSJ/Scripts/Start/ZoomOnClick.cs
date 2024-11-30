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
                // Zoom ���� �����̰� ZoomObject �� Ŭ���ߴٸ�,
                if (isZoom == true && hitInfo.collider.gameObject.layer == 20)
                {
                    StartCoroutine(ZoomInTarget());
                }
            }
        }
        
    }

    // ī�޶� ���� �Լ�
    IEnumerator ZoomInTarget()
    {
        while (Vector3.Distance(mainCamera.transform.position, targetPos.position) > 0.01f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos.position, Time.deltaTime * zoomSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // �۸�ġ �ΰ� ��Ȱ��ȭ
        canvasGlitch.SetActive(false);

        // �α��� UI Ȱ��ȭ
        canvasSignIn.SetActive(true);

        // Zoom �Ұ��� ����
        isZoom = false;
    }
}
