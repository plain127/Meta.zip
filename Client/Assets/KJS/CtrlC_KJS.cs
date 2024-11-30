using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CtrlC_KJS : MonoBehaviour
{
    [SerializeField] private TMP_InputField targetInputField; // TMP InputField 컴포넌트

    // 하드코딩된 텍스트
    private string hardcodedText = "권위청은 오늘(28일) 오후 2시 기준으로 전국 여러 지역에 대설주의보 를 발령했다.\r\n폭설에 있어 철저한 대비와 빠른 대처만 있다면 이러한 위험을 최소화할 수 있다.\r\n이번 겨울, 폭설에 대비할 수 있는 안전한 팁들을 소개.\r\n\r\n외출 최소화\r\n폭설 예보가 있다면 외출을 자제하는 것이 좋습니다. 불가피하게 외출해야 한다면, 기상 정보를 미리 체크하고, \r\n도로 상황을 파악하고 차량 점검은 필수\r\n\r\n도로에서의 안전\r\n속도를 줄이고, 차량 간 충분한 거리를 확보하고 스노우체인을 미리 구매\r\n\r\n보행시 안전\r\n미끄러지지 않도록 철저히 대비해 주머니에 손을 넣지 않고 발을 넓게 디디며 천천히 걷는 것이\r\n미끄러짐을 예방\r\n\r\n폭설 후 대처법\r\n눈 치우기와 얼음 제거 스프레이를 추천\r\n";

    private void Update()
    {
        // 키보드의 "1" 키를 감지
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 숫자 키 "1"
        {
            SetInputFieldText();
        }
    }

    private void SetInputFieldText()
    {
        if (targetInputField != null)
        {
            targetInputField.text = hardcodedText; // TMP_InputField에 하드코딩된 텍스트 설정
        }
    }
}