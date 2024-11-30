using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public class ScreenCaptureManager : MonoBehaviour
{
    // �巡�� ������ �����ִ� �簢��
    private Rect dragArea;

    // �巡�� Ȯ�� ����
    private bool isDragging = false;

    // �巡�� ���� ���� ��ġ
    private Vector2 startPosition;

    // ĸó�� �̹����� ���� ����
    public Texture2D capturedTexture;

    // ĸó�� �̹��� ����Ƽ ȭ�鿡 �̸�����
    public RawImage displayImage;
    private bool isCapturing = false;

    // ��ũ��ĸ�� �� ���� �̹���
    public RawImage sucessImage;

    // ����� Ŭ��
    public AudioClip captureClip; // ĸó �Ϸ� �� ����� ����� Ŭ��

    // ����� ���� (�ν����Ϳ��� ���� ����)
    [Range(0f, 1f)] // �����̴��� ���� ����
    public float audioVolume = 1f;

    private AudioSource audioSource; // ����� �ҽ��� ���������� ���

    void Awake()
    {
        // AudioSource�� ������Ʈ�� �߰��ϰų� ��������
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // �ڵ� ��� ��Ȱ��ȭ
    }

    void Update()
    {
        // ���콺�� ���� ��ư�� ������ �� �巡�� ����
        if (Input.GetMouseButtonDown(0) && isCapturing)
        {
            StartDrag();
            sucessImage.gameObject.SetActive(false);
        }

        // �巡�� ��
        if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateDrag();
            sucessImage.gameObject.SetActive(false);
        }

        // ���콺�� ���� �巡�� ��
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            EndDrag();
            StartCoroutine(CaptureScreen());
        }
    }

    private void StartDrag()
    {
        isDragging = true; // �巡�� Ȱ��ȭ
        startPosition = Input.mousePosition; // ���� ���콺 ��ġ ����
        startPosition.y = Screen.height - startPosition.y; // Y ��ǥ ����
    }

    private void UpdateDrag()
    {
        Vector2 currentMousePosition = Input.mousePosition; // ���� ���콺 ��ġ ����
        currentMousePosition.y = Screen.height - currentMousePosition.y; // Y ��ǥ ����

        // �巡�� ���� ������Ʈ
        dragArea = Rect.MinMaxRect(
            Mathf.Min(startPosition.x, currentMousePosition.x),
            Mathf.Min(startPosition.y, currentMousePosition.y),
            Mathf.Max(startPosition.x, currentMousePosition.x),
            Mathf.Max(startPosition.y, currentMousePosition.y)
        );
    }

    private void EndDrag()
    {
        isDragging = false; // �巡�� ��Ȱ��ȭ
        isCapturing = false;
    }

    private IEnumerator CaptureScreen()
    {
        // ��� ����Ͽ� �巡�װ� �Ϸ�� �� ĸó
        yield return new WaitForEndOfFrame();
        print("ĸ�ļ���!");

        // �巡�� ������ ũ��� ��ġ�� ������� �ؽ�ó ����
        capturedTexture = new Texture2D((int)dragArea.width, (int)dragArea.height, TextureFormat.RGB24, false);

        // ReadPixels �޼����� y ��ǥ ������ ���� ���� �巡�� ������ Y ��ǥ�� ����
        capturedTexture.ReadPixels(new Rect(dragArea.x, Screen.height - dragArea.yMax, dragArea.width, dragArea.height), 0, 0);
        capturedTexture.Apply();

        // RawImage�� ĸó�� �ؽ�ó�� �����ش�
        if (displayImage != null)
        {
            displayImage.texture = capturedTexture;
        }

        // ����� ���Ϸ� ����
        byte[] bytes = capturedTexture.EncodeToPNG();
        string now = DateTime.Now.ToString();
        now = now.Replace(":", "-");
        System.IO.File.WriteAllBytes(Path.Combine(Application.dataPath, now + ".png"), bytes); // ��� ����
        print("���强��!");

        // ��ũ��ĸó �� ���� �̹����� �����ش�
        sucessImage.gameObject.SetActive(true);
        Invoke("HideDragSuccessUI", 2.0f); // 2�� �Ŀ� UI�� ����

        // ����� Ŭ�� 12~13�� ������ ���
        if (captureClip != null)
        {
            PlayClipSegment(12f, 13f); // 12�ʺ��� 13�ʱ��� ���
        }
    }

    // Ư�� ����(12~13��)�� ����ϴ� �޼���
    private void PlayClipSegment(float startTime, float endTime)
    {
        if (audioSource.isPlaying)
            audioSource.Stop(); // �̹� ��� ���̸� ����

        audioSource.clip = captureClip; // ����� Ŭ�� �Ҵ�
        audioSource.volume = audioVolume; // ���� ���� (�ν����Ϳ��� ���� ����)
        audioSource.time = startTime; // ��� ���� ���� ����
        audioSource.Play(); // ��� ����

        // endTime���� ���ߵ��� �ڷ�ƾ ����
        StartCoroutine(StopAudioAfterDelay(endTime - startTime));
    }

    // ���� �ð� �� ����� ����� ���ߴ� �ڷ�ƾ
    private IEnumerator StopAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð���ŭ ���
        audioSource.Stop(); // ����� ����
    }

    // �巡�� ���� UI�� ����� �޼���
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
        // �巡�� ���� ���� �� ���
        if (isDragging)
        {
            GUI.color = new Color(1, 1, 0, 0.5f); // ������ �����
            GUI.DrawTexture(dragArea, Texture2D.whiteTexture);
        }
    }
}
