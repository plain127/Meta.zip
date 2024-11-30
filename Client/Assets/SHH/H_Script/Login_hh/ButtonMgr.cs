
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ButtonMgr : MonoBehaviour
{
    public InputField inputemail;
    public InputField inputpassword;
    public GameObject Joinbox;
    public GameObject Accountbox;

    void Start()
    {
        Joinbox.SetActive(false);
        Accountbox.SetActive(false);
    }

    void Update()
    {
        
    }
    public void OnClickJoinbox()
    {
        Joinbox.SetActive(true);
     
    }

    public void OnClickSignIn()
    {
        MetazipAuth.instance.SignIn(inputemail.text, inputpassword.text);
        if (string.IsNullOrEmpty(inputemail.text) || string.IsNullOrEmpty(inputpassword.text))
        {
            print("Ä­ÀÌ ºñ¾úÀ½");
            return;
        }
    }

    public void OnClickSignOut()
    {
        MetazipAuth.instance.SignOut();
    }

   

}
