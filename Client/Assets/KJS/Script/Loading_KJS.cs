using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using System.Collections;
using UnityEngine.UI;
using Photon.Pun; // Photon 네임스페이스 추가

public class Loading_KJS : MonoBehaviour
{
    public RawImage[] rawImages; // 원형으로 움직일 이미지 배열 (4개)
    public float radius = 100f;  // 원의 반지름
    public float speed = 1f;     // 이동 속도 (초당 회전 속도)
    public float angleOffset = 90f; // 각 이미지 간의 각도 차이 (4개일 경우 기본적으로 360 / 4 = 90도)

    public TextMeshProUGUI displayText; // TextMeshProUGUI 컴포넌트
    public string message = "잠시 기다려줘 삐약!"; // 기본 메시지
    private string ellipsis = "..."; // "..." 출력 부분
    public float typingSpeed = 0.1f; // 한 글자씩 출력하는 속도
    public float ellipsisSpeed = 0.5f; // "..."이 한 단계씩 출력되는 속도

    private RectTransform[] rectTransforms;
    private float[] angles; // 각 이미지의 현재 각도 (라디안 단위로 저장)
    private bool isDisplaying = false; // 텍스트 출력 중인지 확인

    [Header("Photon Settings")]
    public string prefabName = "YourPrefabName"; // 생성할 프리팹의 이름
    public Vector3 prefabScale = new Vector3(20f, 20f, 20f); // 프리팹의 스케일
    public float prefabRotationX = -90f; // 프리팹의 x축 회전값
    public float forwardOffset = 0.5f; // 플레이어 앞쪽의 스폰 거리

    private void OnEnable()
    {
        // Photon 프리팹 생성 (주석 처리)
        /*
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            // 로컬 플레이어가 소유한 "Player" 태그의 오브젝트 검색
            GameObject playerObject = FindLocalPlayerObject();

            if (playerObject != null)
            {
                Transform playerTransform = playerObject.transform;

                // 스폰 위치 계산
                Vector3 spawnPosition = playerTransform.position + playerTransform.forward * forwardOffset;

                // 스폰 회전값 계산
                Quaternion spawnRotation = Quaternion.Euler(prefabRotationX, playerTransform.rotation.eulerAngles.y, 0);

                // Photon 프리팹 생성
                GameObject spawnedObject = PhotonNetwork.Instantiate(prefabName, spawnPosition, spawnRotation);

                // 생성된 프리팹의 스케일 설정
                spawnedObject.transform.localScale = prefabScale;
            }
            else
            {
                Debug.LogError("로컬 플레이어 오브젝트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Photon에 연결되어 있지 않거나 룸에 입장하지 않았습니다.");
        }
        */
    }

    //private GameObject FindLocalPlayerObject()
    //{
    //    // "Player" 태그를 가진 모든 오브젝트 검색
    //    GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

    //    foreach (GameObject obj in playerObjects)
    //    {
    //        PhotonView photonView = obj.GetComponent<PhotonView>();

    //        // isMine이 true인 오브젝트를 반환
    //        if (photonView != null && photonView.IsMine)
    //        {
    //            return obj;
    //        }
    //    }

    //    return null; // 로컬 플레이어 오브젝트를 찾지 못한 경우
    //}

    void Start()
    {
        // 배열 초기화
        rectTransforms = new RectTransform[rawImages.Length];
        angles = new float[rawImages.Length];

        // 각 RawImage의 RectTransform 컴포넌트를 가져오고 초기 각도 설정
        for (int i = 0; i < rawImages.Length; i++)
        {
            if (rawImages[i] != null)
            {
                rectTransforms[i] = rawImages[i].GetComponent<RectTransform>();
                angles[i] = Mathf.Deg2Rad * (i * angleOffset); // 각도 차이를 라디안으로 변환하여 초기화
            }
        }

        // 텍스트 출력 시작
        if (displayText != null)
        {
            StartCoroutine(DisplayMessage());
        }
    }

    void Update()
    {
        for (int i = 0; i < rectTransforms.Length; i++)
        {
            if (rectTransforms[i] != null)
            {
                // 시간에 따라 각도를 감소하여 시계 방향으로 이동
                angles[i] -= speed * Time.deltaTime;

                // 원형 경로 계산
                float x = Mathf.Cos(angles[i]) * radius;
                float y = Mathf.Sin(angles[i]) * radius;

                // 이미지 위치 갱신
                rectTransforms[i].anchoredPosition = new Vector2(x, y);
            }
        }
    }

    IEnumerator DisplayMessage()
    {
        while (true)
        {
            // 한 글자씩 메시지 출력
            displayText.text = "";
            foreach (char c in message)
            {
                displayText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            // "..." 출력 반복
            for (int i = 0; i < 3; i++) // 3번 반복
            {
                displayText.text = message + ellipsis.Substring(0, (i % 3) + 1); // "잠시 기다려줘 삐약!.", "..", "..."
                yield return new WaitForSeconds(ellipsisSpeed);
            }

            // 초기화 후 다시 반복
            yield return new WaitForSeconds(ellipsisSpeed);
        }
    }
}