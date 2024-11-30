using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class ScreenCaptureManager : MonoBehaviour
{
    // 드래그 영역을 보여주는 사각형
    private Rect dragArea;

    // 드래그 확인 변수
    private bool isDragging = false;

    // 드래그 시작 지점 위치
    private Vector2 startPosition;

    // 캡처된 이미지를 저장 변수
    public Texture2D capturedTexture;

    // 캡처된 이미지 유니티 화면에 미리보기
    public RawImage displayImage;
    private bool isCapturing = false;

    // 스크린캡쳐 후 성공 이미지
    public RawImage sucessImage;

    // 오디오 클립
    public AudioClip captureClip; // 캡처 완료 후 재생할 오디오 클립

    // 오디오 볼륨 (인스펙터에서 조정 가능)
    [Range(0f, 1f)] // 슬라이더로 볼륨 조정
    public float audioVolume = 1f;

    private AudioSource audioSource; // 오디오 소스를 내부적으로 사용

    void Awake()
    {
        // AudioSource를 컴포넌트로 추가하거나 가져오기
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 자동 재생 비활성화
    }

    void Update()
    {
        // 마우스의 왼쪽 버튼을 눌렀을 때 드래그 시작
        if (Input.GetMouseButtonDown(0) && isCapturing)
        {
            StartDrag();
            sucessImage.gameObject.SetActive(false);
        }

        // 드래그 중
        if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateDrag();
            sucessImage.gameObject.SetActive(false);
        }

        // 마우스를 떼면 드래그 끝
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            EndDrag();
            StartCoroutine(CaptureScreen());
        }
    }

    private void StartDrag()
    {
        isDragging = true; // 드래그 활성화
        startPosition = Input.mousePosition; // 현재 마우스 위치 저장
        startPosition.y = Screen.height - startPosition.y; // Y 좌표 반전
    }

    private void UpdateDrag()
    {
        Vector2 currentMousePosition = Input.mousePosition; // 현재 마우스 위치 저장
        currentMousePosition.y = Screen.height - currentMousePosition.y; // Y 좌표 반전

        // 드래그 영역 업데이트
        dragArea = Rect.MinMaxRect(
            Mathf.Min(startPosition.x, currentMousePosition.x),
            Mathf.Min(startPosition.y, currentMousePosition.y),
            Mathf.Max(startPosition.x, currentMousePosition.x),
            Mathf.Max(startPosition.y, currentMousePosition.y)
        );
    }

    private void EndDrag()
    {
        isDragging = false; // 드래그 비활성화
        isCapturing = false;
    }

    private IEnumerator CaptureScreen()
    {
        // 잠시 대기하여 드래그가 완료된 후 캡처
        yield return new WaitForEndOfFrame();
        print("캡쳐성공!");

        // 드래그 영역의 크기와 위치를 기반으로 텍스처 생성
        capturedTexture = new Texture2D((int)dragArea.width, (int)dragArea.height, TextureFormat.RGB24, false);

        // ReadPixels 메서드의 y 좌표 반전을 위해 현재 드래그 영역의 Y 좌표를 조정
        capturedTexture.ReadPixels(new Rect(dragArea.x, Screen.height - dragArea.yMax, dragArea.width, dragArea.height), 0, 0);
        capturedTexture.Apply();

        // RawImage에 캡처된 텍스처를 보여준다
        if (displayImage != null)
        {
            displayImage.texture = capturedTexture;
        }

        // 결과를 파일로 저장
        byte[] bytes = capturedTexture.EncodeToPNG();
        string now = DateTime.Now.ToString();
        now = now.Replace(":", "-");
        System.IO.File.WriteAllBytes(Path.Combine(Application.dataPath, now + ".png"), bytes); // 경로 설정
        print("저장성공!");

        // 스크린캡처 후 성공 이미지를 보여준다
        sucessImage.gameObject.SetActive(true);
        Invoke("HideDragSuccessUI", 2.0f); // 2초 후에 UI를 숨김

        // 오디오 클립 12~13초 구간만 재생
        if (captureClip != null)
        {
            PlayClipSegment(12f, 13f); // 12초부터 13초까지 재생
        }
    }

    // 특정 구간(12~13초)을 재생하는 메서드
    private void PlayClipSegment(float startTime, float endTime)
    {
        if (audioSource.isPlaying)
            audioSource.Stop(); // 이미 재생 중이면 정지

        audioSource.clip = captureClip; // 오디오 클립 할당
        audioSource.volume = audioVolume; // 볼륨 설정 (인스펙터에서 설정 가능)
        audioSource.time = startTime; // 재생 시작 지점 설정
        audioSource.Play(); // 재생 시작

        // endTime에서 멈추도록 코루틴 실행
        StartCoroutine(StopAudioAfterDelay(endTime - startTime));
    }

    // 일정 시간 후 오디오 재생을 멈추는 코루틴
    private IEnumerator StopAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기
        audioSource.Stop(); // 오디오 정지
    }

    // 드래그 성공 UI를 숨기는 메서드
    void HideDragSuccessUI()
    {
        sucessImage.gameObject.SetActive(false);
    }

    public void OnClickedButton()
    {
        isCapturing = true;
    }

    private void OnGUI()
    {
        // 드래그 영역 색상 및 모양
        if (isDragging)
        {
            GUI.color = new Color(1, 1, 0, 0.5f); // 반투명 노란색
            GUI.DrawTexture(dragArea, Texture2D.whiteTexture);
        }
    }
}
