using Firebase.Auth;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MetazipAccountDB : MonoBehaviour
{
    public static MetazipAccountDB instance;
    public FirebaseFirestore store;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        store = FirebaseFirestore.DefaultInstance;
    }
    public void SaveUserInfo(H_userinfo info)
    {
        StartCoroutine(CoSaveUserInfo(info));
    }

    //USER->Email-> 이름 닉네임 생년월일 관심카테고리

    IEnumerator CoSaveUserInfo(H_userinfo info)
    {
        //저장 경로 USER/UserID/내정보
        string path = "USER/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        //정보 저장 요청
        Task task = store.Document(path).SetAsync(info);
        
        //통신이 완료될때까지 기다려!
        yield return new WaitUntil(() => { return task.IsCompleted; });

        if (task.Exception == null)
        {
            print("계정 정보 저장 성공!");
        }
        else //json파일
        {
            print("저장 실패! :" + task.Exception);
        }
    }
    void Update()
    {
        
    }
}
