using UnityEngine;
using Photon.Pun;

public class UIUIUI : MonoBehaviour
{
    private PlayerMove playerMove;

    private void Start()
    {
        FindPlayerWithTag(); // 동적으로 Player 오브젝트를 찾음
    }

    private void OnEnable()
    {
        // UIUIUI가 활성화되면 PlayerMove의 이동 비활성화
        if (playerMove != null)
        {
            playerMove.EnableMoving(false);
        }
        else
        {
            Debug.LogWarning("PlayerMove 스크립트를 찾을 수 없습니다. Player 오브젝트를 다시 검색합니다.");
            FindPlayerWithTag();
        }
    }

    private void OnDisable()
    {
        // UIUIUI가 비활성화되면 PlayerMove의 이동 활성화
        if (playerMove != null)
        {
            playerMove.EnableMoving(true);
        }
        else
        {
            Debug.LogWarning("PlayerMove 스크립트를 찾을 수 없습니다. Player 오브젝트를 다시 검색합니다.");
            FindPlayerWithTag();
        }
    }

    private void FindPlayerWithTag()
    {
        // "Player" 태그를 가진 오브젝트 중 로컬 플레이어를 찾아 참조
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine) // 로컬 플레이어인 경우
            {
                playerMove = player.GetComponent<PlayerMove>();
                if (playerMove != null)
                {
                    Debug.Log("PlayerMove 스크립트를 성공적으로 참조했습니다.");
                }
                else
                {
                    Debug.LogError("Player 오브젝트에서 PlayerMove 스크립트를 찾을 수 없습니다.");
                }
                return;
            }
        }

        Debug.LogError("로컬 Player 오브젝트를 찾을 수 없습니다.");
    }
}
