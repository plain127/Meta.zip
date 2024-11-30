using System;
using System.IO;
using UnityEngine;
using TMPro; // TextMeshPro 사용
using Photon.Pun; // PhotonNetwork 사용
using Photon.Realtime; // PhotonRoom 사용

public class Billboard : MonoBehaviourPunCallbacks, IPunObservable
{
    private Transform camTransform; // 카메라 Transform

    [Header("TMP 텍스트 출력")]
    [SerializeField]
    private TextMeshProUGUI textMeshPro; // TMP 텍스트를 출력할 컴포넌트

    [Header("PicketId_KJS 참조")]
    [SerializeField]
    private PicketId_KJS picketIdComponent; // PicketId_KJS 컴포넌트 참조

    [Header("스크린샷 경로 확인용 (Debug)")]
    public string screenshotPathDebug; // PicketId_KJS의 경로를 Inspector에 노출

    private string lastScreenshotPath; // 이전 경로를 저장해 변경 감지
    private string formattedText; // 최종 텍스트 형식
    private string photonNickname; // PhotonNetwork 닉네임

    void Start()
    {
        // PicketId_KJS 컴포넌트 자동으로 찾기
        if (picketIdComponent == null)
        {
            picketIdComponent = FindObjectOfType<PicketId_KJS>();
        }

        if (picketIdComponent == null)
        {
            Debug.LogError("PicketId_KJS 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 초기 닉네임 및 경로 설정
        photonNickname = GetPhotonNickname();
        lastScreenshotPath = picketIdComponent.GetScreenshotPath();
        screenshotPathDebug = lastScreenshotPath;

        UpdateBillboardText();
    }

    void LateUpdate()
    {
        // 빌보드 기능: 오브젝트가 항상 카메라를 바라보도록 회전
        if (camTransform == null)
        {
            if (Camera.main != null)
            {
                camTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("Main Camera가 씬에 없습니다. 카메라를 수동으로 할당해야 합니다.");
                return;
            }
        }

        // 오브젝트의 forward 방향을 카메라의 방향으로 설정
        transform.forward = camTransform.forward;
    }

    void Update()
    {
        // PicketId_KJS의 경로 변경 감지
        string currentPath = picketIdComponent.GetScreenshotPath();
        if (currentPath != lastScreenshotPath)
        {
            lastScreenshotPath = currentPath;
            screenshotPathDebug = currentPath;

            // 경로 업데이트 및 동기화
            UpdateBillboardText();
            photonView.RPC("SyncBillboardData", RpcTarget.Others, photonNickname, screenshotPathDebug);
        }
    }

    /// <summary>
    /// 외부에서 경로를 직접 업데이트할 수 있도록 제공되는 메서드
    /// </summary>
    /// <param name="newPath">새로운 스크린샷 경로</param>
    public void UpdateScreenshotPath(string newPath)
    {
        if (newPath != lastScreenshotPath)
        {
            lastScreenshotPath = newPath;
            screenshotPathDebug = newPath;

            // 텍스트 업데이트 및 동기화
            UpdateBillboardText();
            photonView.RPC("SyncBillboardData", RpcTarget.Others, photonNickname, screenshotPathDebug);
        }
    }

    /// <summary>
    /// TMP 텍스트를 업데이트
    /// </summary>
    private void UpdateBillboardText()
    {
        if (!string.IsNullOrEmpty(screenshotPathDebug) && File.Exists(screenshotPathDebug))
        {
            try
            {
                // 스크린샷 파일의 생성 날짜 가져오기
                DateTime fileDate = File.GetCreationTime(screenshotPathDebug);
                string formattedDate = $"{fileDate.Month}월 {fileDate.Day}일"; // 날짜 포맷

                // 텍스트 구성
                formattedText = $"{photonNickname}의 피켓";

                // TMP 텍스트 업데이트
                if (textMeshPro != null)
                {
                    textMeshPro.text = formattedText;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("스크린샷 날짜를 읽는 중 오류 발생: " + ex.Message);
            }
        }
        else
        {
            formattedText = $"{photonNickname}의 스크랩 날짜를 찾을 수 없음";

            // TMP 텍스트 업데이트
            if (textMeshPro != null)
            {
                textMeshPro.text = formattedText;
            }
        }
    }

    /// <summary>
    /// PhotonNetwork에서 닉네임 가져오기
    /// </summary>
    private string GetPhotonNickname()
    {
        // PhotonNetwork가 연결된 상태에서 닉네임 반환
        if (PhotonNetwork.IsConnected)
        {
            return PhotonNetwork.NickName;
        }
        else
        {
            return "Guest"; // 연결되지 않았을 때 기본 닉네임
        }
    }

    /// <summary>
    /// RPC를 통해 빌보드 데이터를 동기화
    /// </summary>
    /// <param name="nickname">Photon 닉네임</param>
    /// <param name="screenshotPath">스크린샷 경로</param>
    [PunRPC]
    private void SyncBillboardData(string nickname, string screenshotPath)
    {
        photonNickname = nickname;
        screenshotPathDebug = screenshotPath;

        // 빌보드 텍스트 업데이트
        UpdateBillboardText();
    }

    /// <summary>
    /// Photon의 데이터 동기화 (IPunObservable 구현)
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 로컬 데이터 전송
            stream.SendNext(photonNickname);
            stream.SendNext(screenshotPathDebug);
        }
        else
        {
            // 원격 데이터 수신
            photonNickname = (string)stream.ReceiveNext();
            screenshotPathDebug = (string)stream.ReceiveNext();

            // 텍스트 업데이트
            UpdateBillboardText();
        }
    }
}