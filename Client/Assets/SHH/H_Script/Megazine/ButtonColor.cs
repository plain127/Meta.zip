using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
    
{
    //버튼 컴포넌트를 저장할 변수
    private Button button;
    //원래 버튼 컬러
    private Color originalColor = Color.white;
    //스크랩 클릭했을 때 바뀌는 색상
    public Color clickColor = Color.red;



    public void Start()
    {
        button = GetComponent<Button>();
    }
    public void Update()
    {
        // 왼쪽마우스로 스크랩 버튼을 클릭하면 색상이 변함
        if (Input.GetMouseButtonDown(0))
        {
         //   ColorChange();
        }
    }

    public void ColorChange()
    {
        var buttonImage = button.GetComponent<Image>();

        // 만약에 buttonImage의 색이 하얀색이니?
        if(buttonImage.color == Color.white)
        {
            // 버튼 색상을 빨간색
            buttonImage.color = clickColor;
        }        

        // 그렇지 않고 만약에 buttonImage의 색이 빨간색이니?
        else if(buttonImage.color == Color.red)
        {
            // 버튼 색상을 하얀색
            buttonImage.color = originalColor;
            
        }
    }
}
    

