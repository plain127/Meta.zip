using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class EditorMgr_KJS : MonoBehaviour
{
    public TMP_InputField inputField;         // 텍스트를 수정할 InputField
    public TMP_Dropdown fontSizeDropdown;
    public TMP_InputField fontSizeInputField;
    public Button boldButton, italicButton;
    public Button underlineButton, strikethroughButton;
    public Button leftAlignButton, centerAlignButton, rightAlignButton;

    public Color pressedColor = new Color(0.8f, 0.8f, 0.8f);
    public Color defaultColor = Color.white;

    private int fontSize = 40;
    private bool isBold = false;
    private bool isItalic = false;
    private bool isUnderline = false;
    private bool isStrikethrough = false;
    private Button selectedButton;  // 현재 선택된 버튼

    private const float FontSizeMultiplier = 1.5f;

    public void Start()
    {
        SetFunction_UI();

        // Dropdown 옵션 초기화
        fontSizeDropdown.ClearOptions();
        List<string> fontSizeOptions = new List<string>();
        for (int i = 10; i <= 98; i += 2)
        {
            fontSizeOptions.Add(i.ToString());
        }
        fontSizeDropdown.AddOptions(fontSizeOptions);
        fontSizeDropdown.value = fontSizeOptions.IndexOf(fontSize.ToString());

        // 이벤트 연결 (OnClick 사용)
        fontSizeDropdown.onValueChanged.AddListener(OnFontSizeDropdownChanged);
        fontSizeInputField.onEndEdit.AddListener(OnFontSizeInputFieldChanged);

        boldButton.onClick.AddListener(OnBoldButtonClicked);
        italicButton.onClick.AddListener(OnItalicButtonClicked);
        underlineButton.onClick.AddListener(OnUnderlineButtonClicked);
        strikethroughButton.onClick.AddListener(OnStrikethroughButtonClicked);

        // OnClick으로 정렬 이벤트 설정
        leftAlignButton.onClick.AddListener(() => OnAlignButtonClicked(TextAlignmentOptions.Left));
        centerAlignButton.onClick.AddListener(() => OnAlignButtonClicked(TextAlignmentOptions.Center));
        rightAlignButton.onClick.AddListener(() => OnAlignButtonClicked(TextAlignmentOptions.Right));

        inputField.onValueChanged.AddListener(OnInputFieldTextChanged);

        inputField.textComponent.fontSize = fontSize;
        fontSizeInputField.text = fontSize.ToString();
    }

    public void SetFunction_UI()
    {
        ResetFunction_UI();
    }

    public void ResetFunction_UI()
    {
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Input..";
        inputField.contentType = TMP_InputField.ContentType.Standard;
        inputField.lineType = TMP_InputField.LineType.MultiLineNewline;
    }

    public void OnFontSizeDropdownChanged(int index)
    {
        string selectedValue = fontSizeDropdown.options[index].text;
        if (int.TryParse(selectedValue, out int newSize))
        {
            fontSize = newSize;
            inputField.textComponent.fontSize = fontSize;
            fontSizeInputField.text = fontSize.ToString();
            UpdateButtonTextStyle();
        }
    }

    public void OnFontSizeInputFieldChanged(string input)
    {
        if (int.TryParse(input, out int newSize) && newSize >= 10 && newSize <= 100)
        {
            fontSize = newSize;
            inputField.textComponent.fontSize = fontSize;

            int dropdownIndex = fontSizeDropdown.options.FindIndex(option => option.text == fontSize.ToString());
            if (dropdownIndex != -1)
            {
                fontSizeDropdown.value = dropdownIndex;
            }

            UpdateButtonTextStyle();
        }
        else
        {
            fontSizeInputField.text = fontSize.ToString();
        }
    }

    public void OnBoldButtonClicked()
    {
        isBold = !isBold;
        UpdateTextStyle();
        boldButton.GetComponent<Image>().color = isBold ? pressedColor : defaultColor;
    }

    public void OnItalicButtonClicked()
    {
        isItalic = !isItalic;
        UpdateTextStyle();
        italicButton.GetComponent<Image>().color = isItalic ? pressedColor : defaultColor;
    }

    public void OnUnderlineButtonClicked()
    {
        isUnderline = !isUnderline;
        UpdateTextStyle();
        underlineButton.GetComponent<Image>().color = isUnderline ? pressedColor : defaultColor;
    }

    public void OnStrikethroughButtonClicked()
    {
        isStrikethrough = !isStrikethrough;
        UpdateTextStyle();
        strikethroughButton.GetComponent<Image>().color = isStrikethrough ? pressedColor : defaultColor;
    }

    private void UpdateTextStyle()
    {
        inputField.textComponent.fontStyle = FontStyles.Normal;

        if (isBold) inputField.textComponent.fontStyle |= FontStyles.Bold;
        if (isItalic) inputField.textComponent.fontStyle |= FontStyles.Italic;
        if (isUnderline) inputField.textComponent.fontStyle |= FontStyles.Underline;
        if (isStrikethrough) inputField.textComponent.fontStyle |= FontStyles.Strikethrough;

        UpdateButtonTextStyle();
    }

    public void OnAlignButtonClicked(TextAlignmentOptions alignment)
    {
        inputField.textComponent.alignment = alignment;
        UpdateButtonColors(alignment);
    }

    private void UpdateButtonColors(TextAlignmentOptions alignment)
    {
        // 버튼 색상 업데이트 (현재 정렬에 따라)
        leftAlignButton.GetComponent<Image>().color = alignment == TextAlignmentOptions.Left ? pressedColor : defaultColor;
        centerAlignButton.GetComponent<Image>().color = alignment == TextAlignmentOptions.Center ? pressedColor : defaultColor;
        rightAlignButton.GetComponent<Image>().color = alignment == TextAlignmentOptions.Right ? pressedColor : defaultColor;
    }

    public void SetInputFieldTextFromButton(Button button)
    {
        selectedButton = button;

        string buttonText = button.GetComponentInChildren<TextMeshProUGUI>().text;
        inputField.text = buttonText;

        TMP_Text buttonTextComponent = button.GetComponentInChildren<TMP_Text>();

        // 텍스트 크기를 2.5배로 설정
        inputField.textComponent.fontSize = (int)(buttonTextComponent.fontSize / FontSizeMultiplier * 1.5f);

        inputField.textComponent.fontStyle = buttonTextComponent.fontStyle;

        fontSizeInputField.text = ((int)(buttonTextComponent.fontSize / FontSizeMultiplier * 1.5f)).ToString();
    }

    public void OnInputFieldTextChanged(string newText)
    {
        if (selectedButton != null)
        {
            selectedButton.GetComponentInChildren<TextMeshProUGUI>().text = newText;
        }
    }

    private void UpdateButtonTextStyle()
    {
        if (selectedButton != null)
        {
            TMP_Text buttonTextComponent = selectedButton.GetComponentInChildren<TMP_Text>();
            buttonTextComponent.fontSize = inputField.textComponent.fontSize * FontSizeMultiplier;
            buttonTextComponent.fontStyle = inputField.textComponent.fontStyle;
        }
    }
}