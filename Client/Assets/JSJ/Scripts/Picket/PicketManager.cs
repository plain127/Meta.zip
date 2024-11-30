using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicketManager : MonoBehaviourPun
{
    [Header("GameObject")]
    public GameObject player;
    public GameObject nowMakePicket;   // 현재 생성된 Picket
    public GameObject panelLinkNews;   // Picket이랑 기사 링크 여부 Panel

    [Header("Button")]
    public Button btn_Picket;   // Picket 생성 버튼

    int picketIdCounter = 0;   // Picket ID를 부여하기 위한 카운터

    public PicketUIManager picketUIManager;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;

        btn_Picket.onClick.AddListener(MakePicket);

        // 기사 Panel 비활성화
        panelLinkNews.SetActive(false);
    }

    // Picket 생성 함수
    public void MakePicket()
    {
        StartCoroutine(CoMakePicket());

        // 내 것이라면,
        if (player.GetComponent<PhotonView>().IsMine)
        {
            // 기사 Panel 활성화
            panelLinkNews.SetActive(true);
        }
    }

   IEnumerator CoMakePicket()
    {
        Ray ray = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // 'PicketZone' 에만 Picket 이 생성
            if (hit.collider.gameObject.layer == 17)
            {
                // Picket 생성 위치
                Vector3 picketPos = player.transform.position + player.transform.forward * 3f;
                // Picket 생성 방향
                Quaternion picketRot = Quaternion.LookRotation(player.transform.forward);

                // Picket 생성
                GameObject obj = PhotonNetwork.Instantiate("Picket", picketPos, picketRot);

                yield return new WaitUntil(() => obj != null);


                // Picket UI Manager에 현재 생성된 Picket 연결
                picketUIManager.currentPicket = obj;
                nowMakePicket = obj;

                // Picket ID 부여
                PicketId_KJS picketIdComponent = obj.GetComponent<PicketId_KJS>();
                if (picketIdComponent != null)
                {
                    picketIdComponent.SetPicketId("Picket" + picketIdCounter); // ID 설정
                    Debug.Log("Picket ID가 설정되었습니다: " + "Picket" + picketIdCounter);
                }

                picketIdCounter++; // ID 카운터 증가
            }
        }
    }
}