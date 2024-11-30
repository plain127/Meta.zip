using System.Collections;
using System.Collections.Generic;
using UniGLTF;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConnection : MonoBehaviour
{
    //public GameObject MagCanvas;
    //public GameObject Magazine;
    //public GameObject ChannelCanvas;
    public GameObject notice;
    public GameObject profile;
    public Button btnnotice;
    public Button btnprofile;
    public Button btnPoweroff;
    public Button btnexit;
    void Start()
    {
     
  
        notice.SetActive(false);
    
        

        btnPoweroff.onClick.AddListener(Onclickpoweroff);  
        btnprofile.onClick.AddListener(OnClickprofile);
        btnnotice.onClick.AddListener(OnClicknotice);
        btnexit.onClick.AddListener(OnClicknoticeExit);
    }
    private void Update()
    {
        ExitKey();
    }
    #region 버튼클릭함수
    public void OnButtonClick()
    {
        // 캔버스 활성화/비활성화
        //MagCanvas.SetActive(!MagCanvas.activeSelf);
       //ChannelCanvas.SetActive(false);
    }

    public void OnClicknotice()
    {
        notice.SetActive(true);
        btnnotice.gameObject.SetActive(false);
        btnprofile.gameObject.SetActive(false);
    }
    public void ExitKey() 
    {
        if (profile != null && Input.GetKeyDown(KeyCode.Escape))
        {
            profile.SetActive(false);
            btnnotice.gameObject.SetActive(true);
            btnprofile.gameObject.SetActive(true);
        }
    }
    public void OnClickprofile()
    {
        profile.SetActive(true);
        btnnotice.gameObject.SetActive(false);
        btnprofile.gameObject.SetActive(false);



    }
    public void OnClicknoticeExit()
    {
        notice.SetActive(false);
        btnnotice.gameObject.SetActive(true);
        btnprofile.gameObject.SetActive(true);
    }
    public void Onclickpoweroff()
    {
        Application.Quit();
        print("다 끈다");
    }

    // public void OnButtonTown()
    // {
    //     Magazine.SetActive(true);
    // }
    // public void OnButtonChannel()
    // {
    //     ChannelCanvas.SetActive(true);
    // }
    #endregion
}


