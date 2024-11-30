using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FireAuthManager : MonoBehaviour
{
    public static FireAuthManager instance;

    public FirebaseAuth auth;

    public LoginSceneMgr loginSceneMgr;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // 로그인 상태 체크 이벤트 등록
        auth.StateChanged += OnChangedAuthState;
    }

    void Update()
    {
        // 숫자키 1번이 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Alpha9)) 
        {
            string email = "emflazlwm@naver.com";
            string password = "111111";
            OnSignIn(email, password);
        }
    }

    void OnChangedAuthState(object sender, EventArgs e)
    {
        // 만약에 유저 정보가 있다면
        if (auth.CurrentUser != null)
        {
            // 로그인
            print("로그인 상태");
        }
        // 그렇지 않다면
        else
        {
            // 로그아웃
            print("로그아웃 상태");
        }
    }

    // 로그인 진행
    public void OnSignIn(string email, string password)
    {
        StartCoroutine(SignIn(email, password));
    }

    IEnumerator SignIn(string email, string password)
    {
        // 로그인 시도
        Task<AuthResult> task = auth.SignInWithEmailAndPasswordAsync(email, password);
        // 통신이 완료될 때까지 기다린다.
        yield return new WaitUntil(() => task.IsCompleted);
        // 만약에 예외가 없다면
        if (task.Exception == null)
        {
            print("로그인 성공");

            // 회원 정보 불러오기
            FireStore.instance.LoadUserInfo();
        }
        else
        {
            print("로그인 실패 : " + task.Exception);

            // 로그인 실패 UI 활성화
            loginSceneMgr.ShowSignInFail();
        }
    }

    // 회원 가입 진행
    public void OnSignUp(string email, string password, UserInfo userInfo)
    {
        StartCoroutine(SignUp(email, password, userInfo));
    }

    IEnumerator SignUp(string email, string password, UserInfo userInfo)
    {
        // 회원가입 시도
        Task<AuthResult> task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        // 통신이 완료될 때까지 기다린다.
        yield return new WaitUntil(() => task.IsCompleted);
        // 만약에 예외가 없다면
        if (task.Exception == null)
        {
            print("회원가입 성공");
            string userId = auth.CurrentUser.UserId;
            userInfo.userId = userId;

            // 회원 정보 저장
            FireStore.instance.SaveUserInfo(userInfo);

            // 회원 가입 성공 UI 활성화
            loginSceneMgr.ShowSignUpSuccess();
        }
        else
        {
            print("회원가입 실패 : " + task.Exception);
        }
    }
}