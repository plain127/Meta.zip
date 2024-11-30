using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer_KJS : MonoBehaviour
{
    //�̵� �ӷ�
    public float moveSpeed;
    //�ȴ� �ӷ�
    public float walkSpeed = 5;
    //�ٴ� �ӷ�
    public float runSpeed = 10;
    //�����̰� �ִ���?
    public bool isMoving;
    //Character Controller
    public CharacterController cc;

    // �����Ŀ�
    public float jumPower = 3;
    // �߷�
    float gravity = -9.81f;
    // Y ���� �ӷ�
    float yVelocity;

    // �ִ� ���� Ƚ��
    public int jumpMaxcnt = 2;
    //���� ���� Ƚ��
    int jumpcurrCnt;

    //�������� �ϴ� ����
    Vector3 moveDir;
    //�������� �ϴ� �Ÿ�
    float moveDist;

    void Start()
    {
        //�̵� �ӷ� =  �ȴ� �ӷ����� ����
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

            // �����̰� �ִ� �� �Ǻ�
            isMoving = dir.sqrMagnitude > 0;

            //Normalize�� dir ũ�� ��ü�� 1�� ����� �Լ�
            //normalize�� ũ�Ⱑ 1�� ���Ͱ� ���ԵǾ��ִ�. dir ������ ũ�⸦ �����ϸ鼭 1�� ����

            dir.Normalize();

            //dir�� ���ǵ带 ������
            //dir *= movespeed;

            // ���࿡ ���� �ִٸ� yVelocity ���� 0���� �ʱ�ȭ����.
            if (cc.isGrounded)
            {
                yVelocity = 0;
                jumpcurrCnt = 0;
            }

            // ���࿡ �����̽� �ٸ� ������
            if (Input.GetButtonDown("Jump"))
            //if(Input.GetKeyDown(KeyCode.Space))
            {   // ���࿡ ���� ���� Ƚ���� �ִ� ���� Ƚ������ ������
                if (jumpcurrCnt < jumpMaxcnt)
                {
                    //yVelocity�� jumpPower�� ����
                    yVelocity = jumPower;
                    //���� ����Ƚ���� ���� ��Ű��
                    jumpcurrCnt++;
                }
            }

            // yVelocity�� �߷°��� �̿��ؼ� ���ҽ�Ų��.
            // v = v0 + at;
            yVelocity += gravity * Time.deltaTime;

            // dir.y���� yVelocity�� ����
            dir.y = yVelocity;

            //transform.position += dir * moveSpeed * Time.deltaTime;
            cc.Move(dir * moveSpeed * Time.deltaTime);

        }
    }
}
