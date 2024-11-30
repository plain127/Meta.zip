using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerWithGameObject : MonoBehaviour
{
    public PhotonView pv;

    [Header("Game Object")]
    public GameObject player;
    public GameObject panel_MagazineNotice;

    public Button btn_No;

    // Panel 상태
    bool isPanelOpen = false;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;

        pv = player.GetComponent<PhotonView>();

        btn_No.onClick.AddListener(OnClickBtnNo);
    }

    // Trigger 영역에 Player 가 들어갔을 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPanelOpen)
        {
            if (pv.IsMine)
            {
                panel_MagazineNotice.SetActive(true);
            }
            
            isPanelOpen = true;
        }
    }

    // Trigger 영역에 Player 가 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPanelOpen)
        {
            if (pv.IsMine)
            {
                panel_MagazineNotice.SetActive(false);
            }
            
            isPanelOpen = false;
        }
    }

    // Panel 닫는 버튼
    public void OnClickBtnNo()
    {
        if (pv.IsMine)
        {
            panel_MagazineNotice.SetActive(false);
        }
        
        isPanelOpen = false;
    }
}
