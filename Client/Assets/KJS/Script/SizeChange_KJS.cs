using UnityEngine;

public class AdjustParentSizeWithHorizontalPadding : MonoBehaviour
{
    public RectTransform parentRectTransform; // �θ� RectTransform
    public RectTransform childRectTransform;  // �ڽ� RectTransform

    private Vector2 fixedInitialSize = new Vector2(250, 150); // �θ��� ���� �ʱ� ũ��
    private Vector2 lastChildSize;                            // �ڽ��� ������ ũ��

    public float horizontalPadding = 20f; // �¿� ����

    void Start()
    {
        if (parentRectTransform == null || childRectTransform == null)
        {
            Debug.LogError("�θ� �Ǵ� �ڽ� RectTransform�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // �θ��� ũ�⸦ ���� �ʱ� ũ��� ����
        parentRectTransform.sizeDelta = fixedInitialSize;

        // �ڽ��� �ʱ� ũ�⸦ ����
        lastChildSize = childRectTransform.sizeDelta;
    }

    void Update()
    {
        // �ڽ��� ���� ũ�⸦ ������
        Vector2 currentChildSize = childRectTransform.sizeDelta;

        // �ڽ� ũ�Ⱑ ����Ǿ����� Ȯ��
        if (currentChildSize != lastChildSize)
        {
            // �ڽ� ũ�Ⱑ ����Ǿ����Ƿ� �θ� ũ�⸦ ������Ʈ
            UpdateParentSize(currentChildSize);

            // ������ �ڽ� ũ�⸦ ������Ʈ
            lastChildSize = currentChildSize;
        }
    }

    // �θ��� ũ�⸦ �ڽ� ũ�⿡ �°� ������Ʈ�ϴ� �޼���
    void UpdateParentSize(Vector2 newChildSize)
    {
        Vector2 parentSize = parentRectTransform.sizeDelta;

        // �θ��� ũ�⸦ �ڽ� ũ�� + �¿� �������� ������Ʈ
        parentSize.x = Mathf.Max(fixedInitialSize.x, newChildSize.x + horizontalPadding * 2); // �¿� ���� ����
        parentSize.y = Mathf.Max(fixedInitialSize.y, newChildSize.y); // ���̴� �״�� ���� (���� ����)

        parentRectTransform.sizeDelta = parentSize;

        Debug.Log($"�θ� ũ�� ������Ʈ (�¿� ���� ����): {parentRectTransform.sizeDelta}");
    }

    // ������Ʈ�� ���� �� �θ� ũ�⸦ ���� �ʱⰪ���� ����
    void OnDisable()
    {
        ResetParentSize();
    }

    // ������Ʈ�� ������ �� �θ� ũ�⸦ ���� �ʱⰪ���� ����
    void OnDestroy()
    {
        ResetParentSize();
    }

    // �θ��� ũ�⸦ ���� �ʱⰪ���� �����ϴ� �޼���
    void ResetParentSize()
    {
        if (parentRectTransform != null)
        {
            parentRectTransform.sizeDelta = fixedInitialSize;
            Debug.Log($"�θ� ũ�Ⱑ ������ �ʱⰪ���� ���µǾ����ϴ�: {fixedInitialSize}");
        }
    }
}