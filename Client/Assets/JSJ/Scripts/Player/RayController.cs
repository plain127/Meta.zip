using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RayController : MonoBehaviour
{
    public Transform targetPos; // "InteractNotice" ������Ʈ�� Transform
    public GameObject rawImage; // Canvas_Interactive�� ù ��° �ڽ��� RawImage ������Ʈ
    public float noticeDistance = 15f; // �˸� Ȱ��ȭ �Ÿ�

    void Start()
    {
        // "Meta_ScrapBook_Scene"�� ��� Canvas�� ã��
        if (SceneManager.GetActiveScene().name == "Meta_ScrapBook_Scene")
        {
            GameObject canvasNotice = GameObject.Find("Canvas_Interactive");
            if (canvasNotice != null && canvasNotice.transform.childCount > 0)
            {
                rawImage = canvasNotice.transform.GetChild(0).gameObject; // ù ��° �ڽ��� RawImage�� �Ҵ�
            }
        }

        // RawImage �ʱ� ��Ȱ��ȭ
        if (rawImage != null)
        {
            rawImage.SetActive(false);
            Debug.Log("[RayController] RawImage ��Ȱ��ȭ ����"); // ����� �޽���
        }

        // "InteractNotice" ������Ʈ�� ã�� targetPos�� �Ҵ�
        GameObject interactNotice = GameObject.Find("InteractNotice");
        if (interactNotice != null)
        {
            targetPos = interactNotice.transform;
        }
    }

    void Update()
    {
        // �Ÿ� ������� RawImage Ȱ��ȭ/��Ȱ��ȭ
        CheckDistance();
    }

    public void CheckDistance()
    {
        if (targetPos != null && rawImage != null)
        {
            // ���� ��ġ�� InteractNotice ��ġ �� �Ÿ� ���
            float distanceToNotice = Vector3.Distance(transform.position, targetPos.position);

            // �Ÿ� ���ǿ� ���� RawImage Ȱ��ȭ/��Ȱ��ȭ
            if (distanceToNotice <= noticeDistance)
            {
                if (!rawImage.activeSelf) // �̹� Ȱ��ȭ ���°� �ƴ϶�� Ȱ��ȭ
                {
                    rawImage.SetActive(true);
                }
            }
            else
            {
                if (rawImage.activeSelf) // �̹� ��Ȱ��ȭ ���°� �ƴ϶�� ��Ȱ��ȭ
                {
                    rawImage.SetActive(false);
                }
            }
        }
    }
}