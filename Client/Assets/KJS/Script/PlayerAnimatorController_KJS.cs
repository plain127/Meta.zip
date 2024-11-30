using Photon.Pun;
using UnityEngine;

public class PlayerAnimationController_KJS : MonoBehaviourPunCallbacks
{
    private Animator animator;
    private PhotonView photonView;
    private bool isWalking;

    private void Start()
    {
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            bool walking = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

            if (walking != isWalking)
            {
                isWalking = walking;
                photonView.RPC("SetWalkingAnimation", RpcTarget.All, isWalking);
            }
        }
    }

    [PunRPC]
    private void SetWalkingAnimation(bool walking)
    {
        animator.SetBool("isWalking", walking);
    }
}