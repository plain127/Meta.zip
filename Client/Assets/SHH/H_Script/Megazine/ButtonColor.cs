using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
    
{
    //��ư ������Ʈ�� ������ ����
    private Button button;
    //���� ��ư �÷�
    private Color originalColor = Color.white;
    //��ũ�� Ŭ������ �� �ٲ�� ����
    public Color clickColor = Color.red;



    public void Start()
    {
        button = GetComponent<Button>();
    }
    public void Update()
    {
        // ���ʸ��콺�� ��ũ�� ��ư�� Ŭ���ϸ� ������ ����
        if (Input.GetMouseButtonDown(0))
        {
         //   ColorChange();
        }
    }

    public void ColorChange()
    {
        var buttonImage = button.GetComponent<Image>();

        // ���࿡ buttonImage�� ���� �Ͼ���̴�?
        if(buttonImage.color == Color.white)
        {
            // ��ư ������ ������
            buttonImage.color = clickColor;
        }        

        // �׷��� �ʰ� ���࿡ buttonImage�� ���� �������̴�?
        else if(buttonImage.color == Color.red)
        {
            // ��ư ������ �Ͼ��
            buttonImage.color = originalColor;
            
        }
    }
}
    

