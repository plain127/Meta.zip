using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CtrlC_KJS : MonoBehaviour
{
    [SerializeField] private TMP_InputField targetInputField; // TMP InputField ������Ʈ

    // �ϵ��ڵ��� �ؽ�Ʈ
    private string hardcodedText = "����û�� ����(28��) ���� 2�� �������� ���� ���� ������ �뼳���Ǻ� �� �߷��ߴ�.\r\n������ �־� ö���� ���� ���� ��ó�� �ִٸ� �̷��� ������ �ּ�ȭ�� �� �ִ�.\r\n�̹� �ܿ�, ������ ����� �� �ִ� ������ ������ �Ұ�.\r\n\r\n���� �ּ�ȭ\r\n���� ������ �ִٸ� ������ �����ϴ� ���� �����ϴ�. �Ұ����ϰ� �����ؾ� �Ѵٸ�, ��� ������ �̸� üũ�ϰ�, \r\n���� ��Ȳ�� �ľ��ϰ� ���� ������ �ʼ�\r\n\r\n���ο����� ����\r\n�ӵ��� ���̰�, ���� �� ����� �Ÿ��� Ȯ���ϰ� �����ü���� �̸� ����\r\n\r\n����� ����\r\n�̲������� �ʵ��� ö���� ����� �ָӴϿ� ���� ���� �ʰ� ���� �а� ���� õõ�� �ȴ� ����\r\n�̲������� ����\r\n\r\n���� �� ��ó��\r\n�� ġ���� ���� ���� �������̸� ��õ\r\n";

    private void Update()
    {
        // Ű������ "1" Ű�� ����
        if (Input.GetKeyDown(KeyCode.Alpha1)) // ���� Ű "1"
        {
            SetInputFieldText();
        }
    }

    private void SetInputFieldText()
    {
        if (targetInputField != null)
        {
            targetInputField.text = hardcodedText; // TMP_InputField�� �ϵ��ڵ��� �ؽ�Ʈ ����
        }
    }
}