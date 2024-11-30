using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll_Mgr_KJS : MonoBehaviour
{
    public ScrollRect scrollRect; // ScrollRect 컴포넌트 참조

    private int numberOfSteps;    // 스크롤 콘텐츠의 총 스텝 수
    private float stepSize;       // 한 번에 이동할 스텝 크기

    private void Start()
    {
        if (scrollRect == null)
        {
            Debug.LogError("ScrollRect가 연결되지 않았습니다.");
            return;
        }

        // ScrollRect의 Content 크기와 Viewport 크기를 기반으로 스텝 크기 계산
        UpdateStepSize();
    }

    private void UpdateStepSize()
    {
        // ScrollRect의 numberOfSteps를 동적으로 계산 (여기서는 Content의 자식 개수를 기준으로 계산)
        numberOfSteps = scrollRect.content.childCount; // 자식 오브젝트 수 기반 스텝 계산 (예시)
        if (numberOfSteps <= 1) numberOfSteps = 1;     // 최소 1로 설정

        // 스텝 크기 계산 (1 / (스텝 수 - 1))
        stepSize = 1f / (numberOfSteps - 1);
    }

    private void Update()
    {
        if (scrollRect == null) return;

        // 왼쪽 화살표 입력
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ScrollSteps(-1); // 한 스텝 왼쪽으로 이동
        }

        // 오른쪽 화살표 입력
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ScrollSteps(1); // 한 스텝 오른쪽으로 이동
        }
    }

    /// <summary>
    /// ScrollRect를 지정된 스텝 수만큼 이동합니다.
    /// </summary>
    /// <param name="stepCount">이동할 스텝 수 (양수: 오른쪽, 음수: 왼쪽)</param>
    public void ScrollSteps(int stepCount)
    {
        // ScrollRect의 현재 가로 위치에 스텝 크기를 곱한 값만큼 이동
        float newPosition = scrollRect.horizontalNormalizedPosition + (stepSize * stepCount);

        // 스크롤 값을 0 ~ 1로 제한
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(newPosition);
    }

    /// <summary>
    /// Content 크기가 변경되었을 때 호출하여 스텝 크기를 업데이트합니다.
    /// </summary>
    public void Refresh()
    {
        UpdateStepSize();
    }
}
