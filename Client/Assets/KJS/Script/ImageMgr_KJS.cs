using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshPro 사용 시 필요
using SFB;   // StandaloneFileBrowser 네임스페이스 사용

public class ImageMgr_KJS : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();  // 동적으로 생성된 버튼들
    private HashSet<Button> buttonsWithHiddenText = new HashSet<Button>();  // 텍스트가 숨겨진 버튼들 추적

    // 새로 생성된 버튼을 리스트에 추가하고 이벤트 연결
    public void AddButton(Button newButton)
    {
        if (newButton != null)
        {
            buttons.Add(newButton);
            newButton.onClick.AddListener(() => OnButtonClick(newButton));
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    private void OnButtonClick(Button targetButton)
    {
        OpenFileExplorerAndSetImage(targetButton);  // 파일 탐색기를 열고 이미지 설정
    }

    // 파일 탐색기를 열고 이미지 파일을 선택하는 메서드
    private void OpenFileExplorerAndSetImage(Button targetButton)
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            "Select an Image", "",
            new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg", "gif", "bmp") }, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            SetImageToButton(targetButton, paths[0]);  // 이미지 할당
        }
    }

    // 이미지 파일을 버튼에 할당하는 메서드
    private void SetImageToButton(Button button, string imagePath)
    {
        byte[] imageBytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);

        Sprite newSprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );

        // 버튼의 Image 컴포넌트에 Sprite 할당
        button.GetComponent<Image>().sprite = newSprite;
    }

    // 매 프레임마다 버튼들을 체크하여 이미지가 할당된 경우 텍스트를 숨김
    private void Update()
    {
        for (int i = buttons.Count - 1; i >= 0; i--)  // 리스트를 역순으로 순회하여 삭제 안전성 확보
        {
            Button button = buttons[i];
            if (button == null)  // 버튼이 삭제된 경우 리스트에서 제거하고 다음으로
            {
                buttons.RemoveAt(i);
                continue;
            }

            Image buttonImage = button.GetComponent<Image>();

            // 이미지가 할당되어 있고, 아직 텍스트가 숨겨지지 않은 버튼만 처리
            if (buttonImage != null && buttonImage.sprite != null && !buttonsWithHiddenText.Contains(button))
            {
                HideButtonText(button);
                buttonsWithHiddenText.Add(button);  // 텍스트가 숨겨진 버튼으로 등록
            }
        }
    }

    // 버튼의 Text 또는 TMP_Text 컴포넌트를 숨기는 메서드
    private void HideButtonText(Button button)
    {
        Text uiText = button.GetComponentInChildren<Text>();
        TMP_Text tmpText = button.GetComponentInChildren<TMP_Text>();

        if (uiText != null) uiText.gameObject.SetActive(false);
        if (tmpText != null) tmpText.gameObject.SetActive(false);
    }
}