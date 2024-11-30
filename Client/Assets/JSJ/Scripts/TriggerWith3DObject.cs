using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWith3DObject : MonoBehaviour
{
    public GameObject player;
    public GameObject canvas_3DObject;
    public GameObject panel_3DObject;

    bool isPlayerInside = false;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;
        canvas_3DObject = transform.GetChild(1).gameObject;
        panel_3DObject = canvas_3DObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetMouseButtonDown(1))
        {
            panel_3DObject.SetActive(true);
        }
    }

    // Trigger ������ Player �� ���� ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    // Trigger ������ Player �� ������ ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            panel_3DObject.SetActive(false);
        }
    }
}
