using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AI_Movement_KJS : MonoBehaviourPun
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    private Vector3 offset = new Vector3(1, 3, -1);

    // Scene에서 tool UI를 저장하기 위한 변수
    private GameObject Chat;

    public List<GameObject> list_ui = new List<GameObject>();

    private bool isAgentEnabled = true; // NavMeshAgent 활성화 상태 플래그
    private bool isRotatingToPlayer = false; // 로컬 플레이어 방향으로 회전 플래그
    private Transform localPlayerTransform; // 로컬 플레이어의 Transform

    public float rotationSpeed = 2f; // 회전 속도

    // 오디오 관련 변수
    private AudioSource audioSource; // 오디오 소스
    public AudioClip clickSound; // 클릭 시 재생할 소리

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // AudioSource 컴포넌트 추가 및 설정
        audioSource = gameObject.AddComponent<AudioSource>();

        // 클라이언트의 로컬 플레이어 찾기
        FindLocalPlayer();

        // "MagazineView" 오브젝트 안에 있는 "Tool" UI를 찾기
        GameObject magazineView = GameObject.Find("MagazineView");
        if (magazineView != null)
        {
            Chat = magazineView.transform.Find("Chat")?.gameObject;
            if (Chat != null)
            {
                // tool UI, NPC를 비활성화된 상태로 초기화
                Chat.SetActive(false);
            }
            else
            {
                Debug.LogError("Tool UI not found within MagazineView.");
            }
        }
        else
        {
            Debug.LogError("MagazineView object not found in the scene.");
        }

        list_ui.Add(GameObject.Find("MagazineView").transform.GetChild(0).gameObject);
        list_ui.Add(GameObject.Find("MagazineView").transform.GetChild(1).gameObject);
        list_ui.Add(GameObject.Find("MagazineView 2").transform.GetChild(0).gameObject);
        list_ui.Add(GameObject.Find("Canvas_Inventory").transform.GetChild(0).gameObject);
    }

    void Update()
    {
        // AI가 이 클라이언트의 로컬 플레이어만 따라가도록 설정
        if (playerTransform != null && photonView.IsMine && isAgentEnabled)
        {
            // 플레이어의 회전을 적용한 상대 위치 계산
            Vector3 rotatedOffset = playerTransform.rotation * offset;
            Vector3 targetPosition = playerTransform.position + rotatedOffset;

            agent.SetDestination(targetPosition);
        }

        // 로컬 플레이어 방향으로 회전
        if (isRotatingToPlayer && localPlayerTransform != null)
        {
            RotateToPlayer();
        }

        if (Input.GetMouseButtonDown(0) && photonView.IsMine) // 마우스 왼쪽 클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 이 스크립트가 부착된 오브젝트를 클릭했는지 확인
                if (hit.transform == transform)
                {
                    if (Chat != null)
                    {
                        OnMouseDown();
                    }
                }
            }
        }

        // ESC 키가 눌렸을 때 tool UI를 비활성화하고 NavMeshAgent를 다시 활성화
        if (Input.GetKeyDown(KeyCode.Escape) && Chat != null)
        {
            Chat.SetActive(false);

            if (agent != null && !isAgentEnabled)
            {
                agent.enabled = true; // NavMeshAgent 다시 활성화
                isAgentEnabled = true;
                isRotatingToPlayer = false; // 로컬 플레이어 방향 회전 중단
            }
        }
    }

    private void FindLocalPlayer()
    {
        // 모든 플레이어 오브젝트를 찾아 로컬 플레이어를 설정
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();

            // 이 클라이언트에 속한 플레이어인지 확인
            if (photonView != null && photonView.IsMine)
            {
                playerTransform = player.transform; // AI가 따라다니는 플레이어 설정
                localPlayerTransform = player.transform; // 로컬 플레이어의 Transform 저장
                break;
            }
        }

        if (playerTransform == null)
        {
            Debug.LogError("Local player object with tag 'Player' not found.");
        }
    }

    public void OnMouseDown()
    {
        foreach (GameObject ui in list_ui)
        {
            if (ui.activeSelf == true)
            {
                return;
            }
        }

        // 이 스크립트가 할당된 오브젝트를 로컬 플레이어가 클릭한 경우 tool UI를 활성화
        if (photonView.IsMine && Chat != null)
        {
            Chat.SetActive(true);
        }

        // NavMeshAgent 비활성화
        if (agent != null && isAgentEnabled)
        {
            agent.enabled = false; // NavMeshAgent 비활성화
            isAgentEnabled = false;

            // 로컬 플레이어 방향으로 회전 시작
            isRotatingToPlayer = true;
        }

        // CameraManager의 MoveCameraToPosition 호출
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            cameraManager.MoveCameraToPosition();
        }

        // 클릭 사운드 재생
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private void RotateToPlayer()
    {
        if (localPlayerTransform == null) return;

        // 로컬 플레이어 방향 계산
        Vector3 directionToPlayer = localPlayerTransform.position - transform.position;
        directionToPlayer.y = 0; // Y축 고정 (수평 방향으로만 회전)

        // 현재 방향에서 로컬 플레이어 방향으로 점진적으로 회전
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 로컬 플레이어 방향으로 거의 회전 완료 시
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            isRotatingToPlayer = false; // 회전 중단
            Debug.Log("로컬 플레이어 방향으로 회전 완료!");
        }
    }

    public void ResetAgentState()
    {
        // NavMeshAgent 활성화
        if (agent != null && !isAgentEnabled)
        {
            agent.enabled = true; // NavMeshAgent 다시 활성화
            isAgentEnabled = true;
        }

        // 로컬 플레이어 방향 회전 중단
        isRotatingToPlayer = false;

        // Chat UI 비활성화 (안전 확인)
        if (Chat != null && Chat.activeSelf)
        {
            Chat.SetActive(false);
        }

        Debug.Log("AI_Movement_KJS: NavMeshAgent 및 상태가 초기화되었습니다.");
    }
}