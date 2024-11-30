using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWithSnowmen : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas_News;

    bool enterNews = false;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;
    }

    // Trigger 영역에 Player 가 들어갔을 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !enterNews)
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                canvas_News.SetActive(true);
            }

            enterNews = true;
        }
    }

    // Trigger 영역에 Player 가 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enterNews)
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                canvas_News.SetActive(false);
            }

            enterNews = false;
        }
    }
}
