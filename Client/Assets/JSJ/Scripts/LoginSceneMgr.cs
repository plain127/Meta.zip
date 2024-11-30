using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneMgr : MonoBehaviour
{
    [Header("Button")]
    public Button btn_SignIn;   // �α��� ��ư
    //public Button btn_Quit;     // ���� ���� ��ư
    public Button btn_Join;     // ȸ������ UI ��ư
    public Button btn_Next;     // �ѱ�� ��ư
    public Button btn_SignUp;   // ȸ������ ���� ��ư

    [Header("Canvas")]
    public GameObject canvas_Signin;        // �α��� UI
    public GameObject canvas_Join;          // ȸ������ UI
    public GameObject canvas_Account;       // User �������� â

    [Header("GameObject")]
    public GameObject img_SignInFail;      // �α��� ���� UI
    public GameObject img_SignUpSuccess;   // ȸ�� ���� ���� UI

    [Header("�α���")]
    public TMP_InputField signInEmail;       // �α��� �̸���
    public TMP_InputField signInPassword;    // �α��� ��й�ȣ

    [Header("ȸ�� ����")]
    public TMP_InputField signUpEmail;      // ȸ������ �̸���   
    public TMP_InputField signUpPassword;   // ȸ������ ��й�ȣ

    [Header("ȸ�� ����")]
    public TMP_InputField userName;       
    public TMP_InputField userNickName;  
    public TMP_InputField userBirth;    
    

    void Start()
    {
        signInEmail.onValueChanged.AddListener(SignInValueChanged);
        signInPassword.onValueChanged.AddListener(SignInValueChanged);

        // ȸ�� �̸��� / ��й�ȣ ���� UI ��Ȱ��ȭ
        canvas_Join.SetActive(false);
        // ȸ�� ���� ���� UI ��Ȱ��ȭ
        canvas_Account.SetActive(false);
    }


    // ���� �ٲ�� ȣ��Ǵ� �Լ�
    void SignInValueChanged(string s)
    {
        btn_SignIn.interactable = s.Length > 0;
    }


    // [�α���]-----------------------------------------------------------------------------------
    public void OnClickSignIn()
    {
        FireAuthManager.instance.OnSignIn(signInEmail.text, signInPassword.text);

        // �ƹ�Ÿ ���� �ڵ� ����
        AvatarManager.instance.RandomAvatartCode();
    }

    // �α��� ���� UI
    public void ShowSignInFail()
    {
        StartCoroutine(SignInFail(1.5f));
    }

    IEnumerator SignInFail(float duration)
    {
        img_SignInFail.SetActive(true);
        yield return new WaitForSeconds(duration);
        img_SignInFail.SetActive(false);
    }


    // [����]---------------------------------------------------------------------------------------
    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    // [ȸ������]------------------------------------------------------------------------------------
    public void OnClickJoin()
    {
        // �α��� UI ��Ȱ��ȭ
        canvas_Signin.SetActive(false);
        
        // ȸ������ UI Ȱ��ȭ
        canvas_Join.SetActive(true);
    }

    // ����
    public void OnClickNext()
    {
        // ȸ������ UI ��Ȱ��ȭ
        canvas_Join.SetActive(false);

        // User �������� â Ȱ��ȭ
        canvas_Account.SetActive(true);
    }

    // ȸ�� ����
    public void OnClickSignUp()
    {
        UserInfo userInfo = new UserInfo();
        userInfo.name = userName.text;
        userInfo.nickName = userNickName.text;
        userInfo.userBirth = int.Parse(userBirth.text);

        FireAuthManager.instance.OnSignUp(signUpEmail.text, signUpPassword.text, userInfo);
    }

    // ȸ�� ���� ���� UI
    public void ShowSignUpSuccess()
    {
        StartCoroutine(SignUpSuccess(1.5f));
    }

    IEnumerator SignUpSuccess(float duration)
    {
        img_SignUpSuccess.SetActive(true);
        yield return new WaitForSeconds(duration);
        img_SignUpSuccess.SetActive(false);
    }


    // [X]-------------------------------------------------------------------------------------------
    public void OnClickX()
    {
        // ȸ������ / ���� ���� UI ��Ȱ��ȭ
        canvas_Join.SetActive(false);
        canvas_Account.SetActive(false);

        // �α��� UI Ȱ��ȭ
        canvas_Signin.SetActive(true);
    }
}
