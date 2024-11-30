using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Net.Sockets;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    [Header("닉네임")]
    public GameObject canvasPlayerNickName;
    public TMP_Text playerNickName;

    [Header("이동")]
    public float moveSpeed = 5f;
    public float runSpeed = 10f;

    [Header("회전")]
    public float rotSpeed = 200f;

    [Header("점프")]
    public float jumpPower = 3f;
    public int jumpMaxCount = 1;

    float yPos;
    int jumpCurrentCount;

    Vector3 lastPosition;   // 추락 전 플레이어 마지막 위치
    float fallHeight = -10f;

    float moveState = 0;
    float currentSpeed;
    bool isRunning = false;

    Animator animator;
    CharacterController cc;

    private bool canMove = true;   // `Moving` 활성화 여부를 제어하는 플래그
    private bool isRotatingToHelper = false;   // Helper 방향 회전 활성화 플래그
    private Transform helperTarget;   // Helper 오브젝트의 Transform

    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        canvasPlayerNickName = transform.GetChild(0).gameObject;
        playerNickName = canvasPlayerNickName.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();

        // 플레이어 닉네임 부여
        playerNickName.text = photonView.Owner.NickName;

        // 초기 위치 설정
        lastPosition = transform.position;
    }

    void Update()
    {
        // 닉네임을 카메라를 향해 회전
        canvasPlayerNickName.transform.rotation = Quaternion.LookRotation(canvasPlayerNickName.transform.position - Camera.main.transform.position);

        // Helper 방향으로 회전 중이라면
        if (isRotatingToHelper && helperTarget != null)
        {
            // Helper 방향으로 회전
            RotateToHelper(); 
        }
        else if (canMove)
        {
            if (photonView.IsMine)
            {
                // 일반 이동 처리
                Moving(); 
            }
        }

        // 플레이어가 달리기 모드라면,
        if (isRunning)
        {
            currentSpeed = runSpeed;

            // Player Run Animation
            animator.SetBool("isRunning", true);
        }
        // 플레이어가 달리기 모드가 아니라면,
        else
        {
            currentSpeed = moveSpeed;

            // Player Walk Animation
            animator.SetFloat("Moving", moveState);
            // Player Run Animation
            animator.SetBool("isRunning", false);
        }

        // 추락 감지 및 리스폰 처리
        if (transform.position.y < fallHeight)
        {
            // 리스폰
            PlayerRespawn();
        }
    }


    // -------------------------------------------------------------------------------------------------------------------------------------- [ Player Move ]
    // 플레이어 이동 관련 함수
    public void Moving()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        moveState = dir.magnitude;


        // 플레이어가 움직이는 상태이고 Left Shift를 눌렀다면,
        if (Input.GetKeyDown(KeyCode.LeftShift) && moveState > 0)
        {
            // 달리기 모드 Toggle
            isRunning = !isRunning;
        }
        
        // 플레이어가 움직이는 상태가 아니라면,
        if (h == 0 && v == 0)
        {
            // 달리기 모드는 해제된다.
            isRunning = false;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        }

        // 중력 적용
        yPos += Physics.gravity.y * Time.deltaTime;

        // 바닥에 닿았을 때
        if (cc.collisionFlags == CollisionFlags.CollidedBelow)
        {
            yPos = 0;
            jumpCurrentCount = 0;

            // 바닥에 닿으면 마지막 위치를 갱신
            lastPosition = transform.position;
        }

        // 채팅 중이 아니라면,
        if (!ChatManager.instance.IsChatting() && !PicketChatManager.instance.IsPicketChatting())
        {
            // 점프
            if (Input.GetKeyDown(KeyCode.Space) && jumpCurrentCount < jumpMaxCount)
            {
                yPos = jumpPower;
                jumpCurrentCount++;
            }
        }
        
        dir.y = yPos;

        cc.Move(dir * currentSpeed * Time.deltaTime);
    }


    // -------------------------------------------------------------------------------------------------------------------------------------- [ Player Respawn ]
    // Player 리스폰 함수
    public void PlayerRespawn()
    {
        transform.position = lastPosition;
        yPos = 0;
        jumpCurrentCount = 0;
    }


    // -------------------------------------------------------------------------------------------------------------------------------------- [ KJS ]
    // `Moving` 기능 활성화/비활성화
    public void EnableMoving(bool enable)
    {
        canMove = enable;

        // 이동 중단 시 애니메이션도 초기화
        if (!enable)
        {
            animator.SetFloat("Move", 0f);
        }
    }


    // Helper 방향으로 회전 시작
    public void StartRotateToHelper()
    {
        // "Helper" 태그를 가진 오브젝트 찾기
        GameObject helperObject = GameObject.FindGameObjectWithTag("Helper");

        if (helperObject != null)
        {
            helperTarget = helperObject.transform; // Helper의 Transform 저장
            isRotatingToHelper = true; // 회전 활성화
            EnableMoving(false); // 이동 중단
        }
        else
        {
            Debug.LogWarning("Helper 오브젝트를 찾을 수 없습니다.");
        }
    }


    // Helper 방향으로 회전
    private void RotateToHelper()
    {
        if (helperTarget == null) return;

        // Helper의 방향 계산
        Vector3 directionToHelper = helperTarget.position - transform.position;
        directionToHelper.y = 0; // Y축 고정 (수평 방향 회전만 적용)

        // 현재 방향에서 Helper 방향으로 점진적으로 회전
        Quaternion targetRotation = Quaternion.LookRotation(directionToHelper);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

        // Helper 방향으로 거의 회전 완료 시
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            isRotatingToHelper = false; // 회전 중단
            Debug.Log("Helper 방향으로 회전 완료!");
        }
    }


    // -------------------------------------------------------------------------------------------------------------------------------------- [ Player 동기화 ]
    // 플레이어 이동 / 애니메이션 동기화
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(moveState);   
            stream.SendNext(isRunning);
        }
        else
        {
            moveState = (float)stream.ReceiveNext();
            isRunning = (bool)stream.ReceiveNext();
        }
    }
}