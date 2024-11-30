using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용
using SFB;   // StandaloneFileBrowser 사용

public class ImageBox_Thumbnail : MonoBehaviour
{
    private Button button;  // 버튼 컴포넌트 참조
    public Image targetImage;  // 선택된 이미지를 표시할 Image 컴포넌트 (Inspector에서 할당)
    public TextMeshProUGUI placeholderText;  // 이미지가 할당되면 비활성화할 TextMeshProUGUI 컴포넌트 (Inspector에서 할당)

    void Start()
    {
        // 버튼 컴포넌트 가져오기
        button = GetComponent<Button>();

        if (button != null)
        {
            // 버튼 클릭 시 OpenFileExplorer 메서드 호출
            button.onClick.AddListener(OpenFileExplorer);
        }
        else
        {
            Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
        }

        if (targetImage == null)
        {
            Debug.LogError("Target Image가 할당되지 않았습니다. Inspector에서 Image 컴포넌트를 연결하세요.");
        }

        if (placeholderText == null)
        {
            Debug.LogError("Placeholder Text가 할당되지 않았습니다. Inspector에서 TextMeshProUGUI 컴포넌트를 연결하세요.");
        }
    }

    // 파일 탐색기를 여는 메서드
    void OpenFileExplorer()
    {
        // StandaloneFileBrowser를 사용하여 파일 탐색기 열기
        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            "Select an Image",                 // 창 제목
            "",                               // 초기 경로 (빈 문자열로 설정 시 기본 경로 사용)
            new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg") }, // 허용 확장자 필터
            false                             // 다중 선택 여부 (false: 단일 파일 선택)
        );

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            Debug.Log($"선택된 파일 경로: {paths[0]}");
            StartCoroutine(LoadImage(paths[0]));  // 선택된 경로로 이미지 로드
        }
        else
        {
            Debug.LogWarning("파일이 선택되지 않았습니다.");
        }
    }

    // 선택된 이미지 파일을 로드하여 Image 컴포넌트에 적용하는 코루틴
    IEnumerator LoadImage(string path)
    {
        // 파일 경로에서 이미지 데이터를 읽어오기
        byte[] imageData = File.ReadAllBytes(path);

        // Texture2D 생성
        Texture2D texture = new Texture2D(2, 2);  // 임시 크기 (로드 시 자동으로 조정됨)
        bool isLoaded = texture.LoadImage(imageData);  // 이미지 데이터 로드

        if (isLoaded)
        {
            // Texture2D를 Sprite로 변환
            Sprite newSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            // Image 컴포넌트에 새 Sprite 적용
            targetImage.sprite = newSprite;
            Debug.Log("이미지가 성공적으로 로드되었습니다.");

            // 이미지가 할당되었으므로 Text 비활성화
            if (placeholderText != null)
            {
                placeholderText.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("이미지 로드에 실패했습니다.");
        }

        yield return null;
    }
}