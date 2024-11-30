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

        //�α��� ���� üũ
        auth.StateChanged += OnChangeAuthState;



    }
    void OnChangeAuthState(object sender, EventArgs e)
    {
        //���� �� ������ �ִٸ� (�α���)
        if(auth.CurrentUser !=null)
        {
            print(auth.CurrentUser.Email + "," + auth.CurrentUser.UserId);
            print("�α���!");
           
        }
        //�׷��������� (�α׾ƿ�)
        else
        {
            print("�α׾ƿ�!");
        }
    }
    public void SignUp(string email, string password)
    {
        StartCoroutine(CoSignUp(email, password));
    }

    IEnumerator CoSignUp(string email, string password)
    {
       //ȸ������ �õ�
       Task<AuthResult> task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        //����� �Ϸ�ɶ����� ��ٷ�!
        yield return new WaitUntil(() => { return task.IsCompleted; });

        if(task.Exception == null)
        {
            print("ȸ�����Լ���!");
        }
        else //��й�ȣ�� ������ 6�ڸ� �̻�
        {
            print("ȸ������ ����! :" + task.Exception);
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
        //�α��� �õ�
        Task<AuthResult> task = auth.SignInWithEmailAndPasswordAsync(email, password);
        //����� �Ϸ�ɶ����� ��ٷ�!
        yield return new WaitUntil(() => { return task.IsCompleted; });

        if (task.Exception == null)
        {
            print("�α��μ���!");
        }
        else 
        {
            print("����! :" + task.Exception);
        }
    }
    
    public void SignOut()
    {
        auth.SignOut();
    }
}
