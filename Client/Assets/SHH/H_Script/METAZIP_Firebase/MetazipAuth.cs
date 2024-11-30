using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MetazipAuth : MonoBehaviour
{
    public static MetazipAuth instance;
    public FirebaseAuth auth;

    public void Awake()
    {
        instance = this;

        auth = FirebaseAuth.DefaultInstance;

        //로그인 상태 체크
        auth.StateChanged += OnChangeAuthState;



    }
    void OnChangeAuthState(object sender, EventArgs e)
    {
        //만약 내 정보가 있다면 (로그인)
        if(auth.CurrentUser !=null)
        {
            print(auth.CurrentUser.Email + "," + auth.CurrentUser.UserId);
            print("로그인!");
           
        }
        //그렇지않으면 (로그아웃)
        else
        {
            print("로그아웃!");
        }
    }
    public void SignUp(string email, string password)
    {
        StartCoroutine(CoSignUp(email, password));
    }

    IEnumerator CoSignUp(string email, string password)
    {
       //회원가입 시도
       Task<AuthResult> task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //통신이 완료될때까지 기다려!
        yield return new WaitUntil(() => { return task.IsCompleted; });

        if(task.Exception == null)
        {
            print("회원가입성공!");
        }
        else //비밀번호는 무조건 6자리 이상
        {
            print("회원가입 실패! :" + task.Exception);
        }
    }
  //  public void Checkpassword()
  //  {
  //      
  //  }
    void Start()
    {
    }
    public void SignIn(string email, string password)
    {
        StartCoroutine(CoSignIn(email, password));
    }

    IEnumerator CoSignIn(string email, string password)
    {
        //로그인 시도
        Task<AuthResult> task = auth.SignInWithEmailAndPasswordAsync(email, password);
        //통신이 완료될때까지 기다려!
        yield return new WaitUntil(() => { return task.IsCompleted; });

        if (task.Exception == null)
        {
            print("로그인성공!");
        }
        else 
        {
            print("실패! :" + task.Exception);
        }
    }
    
    public void SignOut()
    {
        auth.SignOut();
    }
}
