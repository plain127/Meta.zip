using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll_Mgr_KJS : MonoBehaviour
{
    public ScrollRect scrollRect; // ScrollRect ������Ʈ ����

    private int numberOfSteps;    // ��ũ�� �������� �� ���� ��
    private float stepSize;       // �� ���� �̵��� ���� ũ��

    private void Start()
    {
        if (scrollRect == null)
        {
            Debug.LogError("ScrollRect�� ������� �ʾҽ��ϴ�.");
            return;
        }

        // ScrollRect�� Content ũ��� Viewport ũ�⸦ ������� ���� ũ�� ���
        UpdateStepSize();
    }

    private void UpdateStepSize()
    {
        // ScrollRect�� numberOfSteps�� �������� ��� (���⼭�� Content�� �ڽ� ������ �������� ���)
        numberOfSteps = scrollRect.content.childCount; // �ڽ� ������Ʈ �� ��� ���� ��� (����)
        if (numberOfSteps <= 1) numberOfSteps = 1;     // �ּ� 1�� ����

        // ���� ũ�� ��� (1 / (���� �� - 1))
        stepSize = 1f / (numberOfSteps - 1);
    }

    private void Update()
    {
        if (scrollRect == null) return;

        // ���� ȭ��ǥ �Է�
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ScrollSteps(-1); // �� ���� �������� �̵�
        }

        // ������ ȭ��ǥ �Է�
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ScrollSteps(1); // �� ���� ���������� �̵�
        }
    }

    /// <summary>
    /// ScrollRect�� ������ ���� ����ŭ �̵��մϴ�.
    /// </summary>
    /// <param name="stepCount">�̵��� ���� �� (���: ������, ����: ����)</param>
    public void ScrollSteps(int stepCount)
    {
        // ScrollRect�� ���� ���� ��ġ�� ���� ũ�⸦ ���� ����ŭ �̵�
        float newPosition = scrollRect.horizontalNormalizedPosition + (stepSize * stepCount);

        // ��ũ�� ���� 0 ~ 1�� ����
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(newPosition);
    }

    /// <summary>
    /// Content ũ�Ⱑ ����Ǿ��� �� ȣ���Ͽ� ���� ũ�⸦ ������Ʈ�մϴ�.
    /// </summary>
    public void Refresh()
    {
        UpdateStepSize();
    }
}
