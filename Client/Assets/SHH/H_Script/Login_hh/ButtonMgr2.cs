using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[FirestoreData]
public class H_userinfo
{
    [FirestoreProperty]
    public string name { get; set; }
    [FirestoreProperty]
    public string nickname { get; set; }
    [FirestoreProperty]
   public int birthdate { get; set; }
}
public class ButtonMgr2 : MonoBehaviour
{
    public InputField Joinemail;
    public InputField Joinpassword;
    //public InputField Joinpassword2;
    public GameObject Joinbox;
    public GameObject Accountbox;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnclicksignUp()
    {
        MetazipAuth.instance.SignUp(Joinemail.text, Joinpassword.text);
      // if (Joinpassword.text != Joinpassword2.text)
      // {
      //     print("일치하지 않아용");
      //     return;
      // }
      // else
      // {
      // print("가입계정이 등록되었쪙");
      // }
      //
        Joinbox.SetActive(false);
        Accountbox.SetActive(true);
    }

    public void OnClickSaveAccount()
    {
        //가상 데이터 만들자.
        H_userinfo info = new H_userinfo();
        info.name = "서하현";
        info.nickname = "서하";
        info.birthdate = 960606;

        MetazipAccountDB.instance.SaveUserInfo(info);
        print("정보 저장!");
    }
}
