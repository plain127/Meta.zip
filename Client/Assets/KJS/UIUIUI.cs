using UnityEngine;
using Photon.Pun;

public class UIUIUI : MonoBehaviour
{
    private PlayerMove playerMove;

    private void Start()
    {
        FindPlayerWithTag(); // �������� Player ������Ʈ�� ã��
    }

    private void OnEnable()
    {
        // UIUIUI�� Ȱ��ȭ�Ǹ� PlayerMove�� �̵� ��Ȱ��ȭ
        if (playerMove != null)
        {
            playerMove.EnableMoving(false);
        }
        else
        {
            Debug.LogWarning("PlayerMove ��ũ��Ʈ�� ã�� �� �����ϴ�. Player ������Ʈ�� �ٽ� �˻��մϴ�.");
            FindPlayerWithTag();
        }
    }

    private void OnDisable()
    {
        // UIUIUI�� ��Ȱ��ȭ�Ǹ� PlayerMove�� �̵� Ȱ��ȭ
        if (playerMove != null)
        {
            playerMove.EnableMoving(true);
        }
        else
        {
            Debug.LogWarning("PlayerMove ��ũ��Ʈ�� ã�� �� �����ϴ�. Player ������Ʈ�� �ٽ� �˻��մϴ�.");
            FindPlayerWithTag();
        }
    }

    private void FindPlayerWithTag()
    {
        // "Player" �±׸� ���� ������Ʈ �� ���� �÷��̾ ã�� ����
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.IsMine) // ���� �÷��̾��� ���
            {
                playerMove = player.GetComponent<PlayerMove>();
                if (playerMove != null)
                {
                    Debug.Log("PlayerMove ��ũ��Ʈ�� ���������� �����߽��ϴ�.");
                }
                else
                {
                    Debug.LogError("Player ������Ʈ���� PlayerMove ��ũ��Ʈ�� ã�� �� �����ϴ�.");
                }
                return;
            }
        }

        Debug.LogError("���� Player ������Ʈ�� ã�� �� �����ϴ�.");
    }
}
