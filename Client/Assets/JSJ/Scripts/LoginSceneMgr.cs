using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneMgr : MonoBehaviour
{
    [Header("Button")]
    public Button btn_SignIn;   // 로그인 버튼
    //public Button btn_Quit;     // 게임 종료 버튼
    public Button btn_Join;     // 회원가입 UI 버튼
    public Button btn_Next;     // 넘기기 버튼
    public Button btn_SignUp;   // 회원가입 진행 버튼

    [Header("Canvas")]
    public GameObject canvas_Signin;        // 로그인 UI
    public GameObject canvas_Join;          // 회원가입 UI
    public GameObject canvas_Account;       // User 정보기입 창

    [Header("GameObject")]
    public GameObject img_SignInFail;      // 로그인 실패 UI
    public GameObject img_SignUpSuccess;   // 회원 가입 성공 UI

    [Header("로그인")]
    public TMP_InputField signInEmail;       // 로그인 이메일
    public TMP_InputField signInPassword;    // 로그인 비밀번호

    [Header("회원 가입")]
    public TMP_InputField signUpEmail;      // 회원가입 이메일   
    public TMP_InputField signUpPassword;   // 회원가입 비밀번호

    [Header("회원 정보")]
    public TMP_InputField userName;       
    public TMP_InputField userNickName;  
    public TMP_InputField userBirth;    
    

    void Start()
    {
        signInEmail.onValueChanged.AddListener(SignInValueChanged);
        signInPassword.onValueChanged.AddListener(SignInValueChanged);

        // 회원 이메일 / 비밀번호 기입 UI 비활성화
        canvas_Join.SetActive(false);
        // 회원 정보 기입 UI 비활성화
        canvas_Account.SetActive(false);
    }


    // 내용 바뀌면 호출되는 함수
    void SignInValueChanged(string s)
    {
        btn_SignIn.interactable = s.Length > 0;
    }


    // [로그인]-----------------------------------------------------------------------------------
    public void OnClickSignIn()
    {
        FireAuthManager.instance.OnSignIn(signInEmail.text, signInPassword.text);

        // 아바타 랜덤 코드 생성
        AvatarManager.instance.RandomAvatartCode();
    }

    // 로그인 실패 UI
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


    // [종료]---------------------------------------------------------------------------------------
    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    // [회원가입]------------------------------------------------------------------------------------
    public void OnClickJoin()
    {
        // 로그인 UI 비활성화
        canvas_Signin.SetActive(false);
        
        // 회원가입 UI 활성화
        canvas_Join.SetActive(true);
    }

    // 다음
    public void OnClickNext()
    {
        // 회원가입 UI 비활성화
        canvas_Join.SetActive(false);

        // User 정보기입 창 활성화
        canvas_Account.SetActive(true);
    }

    // 회원 가입
    public void OnClickSignUp()
    {
        UserInfo userInfo = new UserInfo();
        userInfo.name = userName.text;
        userInfo.nickName = userNickName.text;
        userInfo.userBirth = int.Parse(userBirth.text);

        FireAuthManager.instance.OnSignUp(signUpEmail.text, signUpPassword.text, userInfo);
    }

    // 회원 가입 성공 UI
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
        // 회원가입 / 정보 기입 UI 비활성화
        canvas_Join.SetActive(false);
        canvas_Account.SetActive(false);

        // 로그인 UI 활성화
        canvas_Signin.SetActive(true);
    }
}
