using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using ReqRes;
using Photon.Pun;

public class AiChatMgr_KJS : MonoBehaviour
{
    public static AiChatMgr_KJS Instance { get; private set; }

    public TMP_InputField userInputField; // 사용자 입력 TMP 필드
    public TMP_Text chatResponseText;     // AI 응답 텍스트 TMP
    public APIManager apiManager;         // APIManager 인스턴스
    public Button sendButton;             // 전송 버튼
    public GameObject extraUI;            // 추가적인 UI (일시적 표시)

    private GameObject chatUI;            // MagazineView 안의 Chat UI
    private GameObject toolUI;            // MagazineView 안의 Tool UI
    public GameObject thumbnailUI;        // 썸네일 UI를 제어하기 위한 이미지 UI 오브젝트

    private AudioSource audioSource;      // 오디오 소스
    public AudioClip typingSound;         // 타이핑 효과음
    private float lastSoundPlayTime = 0f; // 마지막으로 재생된 시간
    private float typingSoundDelay = 0.5f; // 타이핑 사운드 재생 간격 (초)
    private Coroutine typeTextCoroutine;

    [Header("Photon Settings")]
    public string prefabName;             // 생성하려는 프리팹 이름 (인스펙터에서 할당)
    public Vector3 prefabScale = new Vector3(20f, 20f, 20f); // 프리팹의 스케일
    public float prefabRotationX = -90f; // 프리팹의 x축 회전값
    public float forwardOffset = 0.5f; // 플레이어 앞쪽의 스폰 거리
    public float yOffset = 0f; // Y축 오프셋 (Inspector에서 조정 가능)

    private ScrollRect chatScrollRect;    // 스크롤을 조정하기 위한 ScrollRect

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        audioSource = gameObject.AddComponent<AudioSource>();

        // ScrollRect를 초기화
        chatScrollRect = chatResponseText.GetComponentInParent<ScrollRect>();
        if (chatScrollRect == null)
        {
            Debug.LogWarning("ScrollRect를 찾을 수 없습니다.");
        }
    }

    void Start()
    {
        GameObject magazineView = GameObject.Find("MagazineView");
        if (magazineView != null)
        {
            chatUI = magazineView.transform.Find("Chat")?.gameObject;
            toolUI = magazineView.transform.Find("Tool")?.gameObject;

            if (chatUI != null)
            {
                chatUI.SetActive(false);
            }
            else
            {
                Debug.LogError("Chat UI not found within MagazineView.");
            }

            if (toolUI != null)
            {
                toolUI.SetActive(false);
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

        if (extraUI != null)
        {
            extraUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Extra UI가 설정되지 않았습니다.");
        }
        if (thumbnailUI != null)
        {
            thumbnailUI.SetActive(false);
        }

        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    private void OnMouseDown()
    {
        if (chatUI != null)
        {
            chatUI.SetActive(true);
            Debug.Log("Chat UI 활성화되었습니다.");
        }
    }

    void OnSendButtonClicked()
    {
        string userMessage = userInputField.text;

        if (!string.IsNullOrEmpty(userMessage))
        {
            if (userMessage.Contains("폭설에 대한 3D 오브젝트를 만들고 싶어"))
            {
                // 추가 UI 활성화 후 메시지 출력
                StartCoroutine(ActivateExtraUIWithDelayAndResponse(2f, "오브젝트를 만들었어 삐약!\n 마우스 오른쪽키를 눌러서 확인해봐."));
            }
            else if (userMessage.Contains("폭설에 대한 기사 썸네일 만들어줘"))
            {
                // 썸네일 UI 활성화 후 메시지 출력
                StartCoroutine(ActivateExtraUIWithThumbnailAndResponse(2f, 2f, "썸네일을 만들었어 삐약! \n 표지 썸네일버튼을 눌러서 확인할 수 있어!"));
            }
            else if (userMessage.Contains("기사창 열어줘"))
            {
                StartCoroutine(ActivateToolUIWithDelay(2f)); // Tool UI 활성화
            }
            else
            {
                // 기본 AI API 호출
                apiManager.LLM(userMessage);
            }

            // 입력 필드 초기화
            userInputField.text = "";
        }
    }


    public void UpdateChatResponse(string responseText)
    {
        if (typeTextCoroutine != null)
        {
            StopCoroutine(typeTextCoroutine);
        }

        // 응답 텍스트를 출력
        typeTextCoroutine = StartCoroutine(TypeText(responseText));
    }

    private IEnumerator ActivateExtraUIWithThumbnailAndResponse(float extraUIDelay, float thumbnailUIDelay, string response)
    {
        // Extra UI 활성화
        yield return new WaitForSeconds(extraUIDelay);
        if (extraUI != null)
        {
            extraUI.SetActive(true);
            Debug.Log("Extra UI가 활성화되었습니다.");

            // 5초 뒤 Extra UI 비활성화
            yield return new WaitForSeconds(5f);
            extraUI.SetActive(false);
            Debug.Log("Extra UI가 비활성화되었습니다.");
        }

        // Thumbnail UI 활성화
        yield return new WaitForSeconds(thumbnailUIDelay);
        if (thumbnailUI != null)
        {
            thumbnailUI.SetActive(true); // Thumbnail UI 활성화
            Debug.Log("Thumbnail UI가 활성화되었습니다.");

            // 5초 뒤 Thumbnail UI 비활성화
            yield return new WaitForSeconds(5f);
            thumbnailUI.SetActive(false);
            Debug.Log("Thumbnail UI가 비활성화되었습니다.");
        }

        // 모든 UI 활성화 작업이 끝난 후 메시지 출력
        UpdateChatResponse(response);
    }

    private IEnumerator ActivateExtraUIWithDelayAndResponse(float delay, string response)
    {
        yield return new WaitForSeconds(delay); // 지정된 대기 시간 후

        if (extraUI != null)
        {
            extraUI.SetActive(true);
            Debug.Log("Extra UI가 활성화되었습니다.");

            // 5초 뒤 Extra UI 비활성화
            yield return new WaitForSeconds(5f);
            extraUI.SetActive(false);
            Debug.Log("Extra UI가 비활성화되었습니다.");
        }
        SpawnPrefabOnNetwork(); // 오브젝트 생성
        // 딜레이가 끝난 후 메시지 출력
        UpdateChatResponse(response);
    }

    public void SpawnPrefabOnNetwork()
    {
        if (string.IsNullOrEmpty(prefabName))
        {
            Debug.LogWarning("생성하려는 프리팹 이름이 설정되지 않았습니다.");
            return;
        }

        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            // 로컬 플레이어 오브젝트를 찾음
            GameObject playerObject = FindLocalPlayerObject();

            if (playerObject != null)
            {
                Transform playerTransform = playerObject.transform;

                // 스폰 위치 계산 (플레이어 앞쪽 + Y 오프셋 적용)
                Vector3 spawnPosition = playerTransform.position + playerTransform.forward * forwardOffset;
                spawnPosition.y += yOffset; // Y축 조정 추가

                // 스폰 회전값 계산
                Quaternion spawnRotation = Quaternion.Euler(prefabRotationX, playerTransform.rotation.eulerAngles.y, 0);

                // Photon 프리팹 생성
                GameObject spawnedObject = PhotonNetwork.Instantiate(prefabName, spawnPosition, spawnRotation);

                // 생성된 프리팹의 스케일 설정
                spawnedObject.transform.localScale = prefabScale;

                Debug.Log($"{prefabName} 프리팹이 생성되었습니다. 위치: {spawnPosition}, 회전: {spawnRotation.eulerAngles}, 스케일: {prefabScale}");
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
    }

    // 로컬 플레이어 오브젝트를 찾는 메서드
    private GameObject FindLocalPlayerObject()
    {
        // "Player" 태그를 가진 모든 오브젝트 검색
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject obj in playerObjects)
        {
            PhotonView photonView = obj.GetComponent<PhotonView>();

            // isMine이 true인 오브젝트를 반환
            if (photonView != null && photonView.IsMine)
            {
                return obj;
            }
        }

        return null; // 로컬 플레이어 오브젝트를 찾지 못한 경우
    }

    private IEnumerator ActivateToolUIWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // LLM 메시지 출력 완료 후 1초 대기
        if (toolUI != null)
        {
            toolUI.SetActive(true);
            Debug.Log("Tool UI가 1초 후 활성화되었습니다.");
        }

        // 이 스크립트가 할당된 UI 비활성화
        if (gameObject != null)
        {
            gameObject.SetActive(false);
            Debug.Log("이 스크립트가 할당된 UI가 비활성화되었습니다.");
        }
    }

    private IEnumerator TypeText(string text)
    {
        chatResponseText.text = "";
        foreach (char c in text)
        {
            chatResponseText.text += c;

            // 사운드 재생 간격을 확인
            if (typingSound != null && audioSource != null)
            {
                if (Time.time - lastSoundPlayTime >= typingSoundDelay)
                {
                    audioSource.PlayOneShot(typingSound);
                    lastSoundPlayTime = Time.time; // 마지막 재생 시간 업데이트
                }
            }

            // 텍스트 출력 시 스크롤을 아래로 내림
            if (chatScrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                chatScrollRect.verticalNormalizedPosition = 0f;
                Canvas.ForceUpdateCanvases();
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
    private void OnDisable()
    {
        // 스크립트가 비활성화될 때 텍스트 초기화
        if (chatResponseText != null)
        {
            chatResponseText.text = "";
            Debug.Log("스크립트가 비활성화되어 텍스트가 초기화되었습니다.");
        }

        if (userInputField != null)
        {
            userInputField.text = "";
            Debug.Log("스크립트가 비활성화되어 입력 필드가 초기화되었습니다.");
        }
    }
}