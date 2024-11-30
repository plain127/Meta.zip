using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerWithPicket : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas_PicketNotice;

    public Button btn_OK;

    bool enterPicketZone = false;
    

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;

        btn_OK.onClick.AddListener(PicketNoticeOK);
    }

    // Trigger ������ Player �� ���� ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                canvas_PicketNotice.SetActive(true);
            }
        }
    }

    public void PicketNoticeOK()
    {
        canvas_PicketNotice.SetActive(false);
    }
}
