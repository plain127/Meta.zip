using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeController : MonoBehaviour
{
    public GameObject canvas_Notice;

    bool isNoticeOpen = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isNoticeOpen)
        {
            canvas_Notice.SetActive(true);

            isNoticeOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isNoticeOpen)
        {
            canvas_Notice.SetActive(false);
        }
    }
}
