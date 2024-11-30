using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer_KJS : MonoBehaviour
{
    //이동 속력
    public float moveSpeed;
    //걷는 속력
    public float walkSpeed = 5;
    //뛰는 속력
    public float runSpeed = 10;
    //움직이고 있는지?
    public bool isMoving;
    //Character Controller
    public CharacterController cc;

    // 점프파워
    public float jumPower = 3;
    // 중력
    float gravity = -9.81f;
    // Y 방향 속력
    float yVelocity;

    // 최대 점프 횟수
    public int jumpMaxcnt = 2;
    //현재 점프 횟수
    int jumpcurrCnt;

    //움직여야 하는 방향
    Vector3 moveDir;
    //움직여야 하는 거리
    float moveDist;

    void Start()
    {
        //이동 속력 =  걷는 속력으로 설정
        moveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        WASD_Move();


        void WASD_Move()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 dirH = transform.right * h;     // Vectr.right = 1,0,0
                                                    //transform.right= 0.0.1
            Vector3 dirV = transform.forward * v;
            Vector3 dir = dirH + dirV;

            // 움직이고 있는 지 판별
            isMoving = dir.sqrMagnitude > 0;

            //Normalize는 dir 크기 자체를 1로 만드는 함수
            //normalize는 크기가 1인 벡터가 포함되어있다. dir 원본의 크기를 유지하면서 1로 유지

            dir.Normalize();

            //dir에 스피드를 곱하자
            //dir *= movespeed;

            // 만약에 땅에 있다면 yVelocity 값을 0으로 초기화하자.
            if (cc.isGrounded)
            {
                yVelocity = 0;
                jumpcurrCnt = 0;
            }

            // 만약에 스페이스 바를 누르면
            if (Input.GetButtonDown("Jump"))
            //if(Input.GetKeyDown(KeyCode.Space))
            {   // 만약에 현재 점프 횟수가 최대 점프 횟수보다 작으면
                if (jumpcurrCnt < jumpMaxcnt)
                {
                    //yVelocity에 jumpPower를 셋팅
                    yVelocity = jumPower;
                    //현재 점프횟수를 증가 시키자
                    jumpcurrCnt++;
                }
            }

            // yVelocity를 중력값을 이용해서 감소시킨다.
            // v = v0 + at;
            yVelocity += gravity * Time.deltaTime;

            // dir.y값에 yVelocity를 셋팅
            dir.y = yVelocity;

            //transform.position += dir * moveSpeed * Time.deltaTime;
            cc.Move(dir * moveSpeed * Time.deltaTime);

        }
    }
}
