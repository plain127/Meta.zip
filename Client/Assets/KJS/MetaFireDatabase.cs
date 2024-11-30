using Firebase.Auth;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class MetaFireDatabase : MonoBehaviour
{
    public static MetaFireDatabase instance;
    public FirebaseFirestore store;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        store = FirebaseFirestore.DefaultInstance;
    }

    private void Update()
    {
        
    }

    //public void SavePost(int postId, AAA info)
    //{
    //    StartCoroutine(CoSaveSavePost(postId, info));
    //}

    //IEnumerator CoSaveSavePost(int postId, AAA info)
    //{
    //    // 저장 경로 User/UserId/내정보
    //    string path = "POST/" + FirebaseAuth.DefaultInstance.CurrentUser.Email + "post_" + postId;

    //    //정보 저장 요청
    //    Task task = store.Document(path).SetAsync(info);
    //    // 통신이 완료될때까지 기다리자.
    //    yield return new WaitUntil(() => { return task.IsCompleted; });

    //    // 만약에 예외가 없다면
    //    if (task.Exception == null)
    //    {
    //        print("게시물 저장 성공");
    //    }
    //    else
    //    {
    //        print("게시물 저장 실패 : " + task.Exception);
    //    }
    //}

    //public void LoadUserInfo(Action<UserInfo> onComplete/*완료되었을 때 호출되는 함수*/)
    //{
    //    StartCoroutine(CoLoadUserInfo(onComplete));
    //}

    //IEnumerator CoLoadUserInfo(Action<UserInfo> onComplete)
    //{
    //    // 저장 경로 User/UserID/내정보
    //    string path = "USER/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

    //    //정보 조회 요청
    //    Task<DocumentSnapshot> task = store.Document(path).GetSnapshotAsync();
    //    // 통신이 완료될때까지 기다리자.
    //    yield return new WaitUntil(() => { return task.IsCompleted; });

    //    // 만약에 예외가 없다면
    //    if (task.Exception == null)
    //    {
    //        print("회원정보 불러오기 성공");

    //        // 불러온 정보를 UserInfo 변수에 저장
    //        UserInfo loadInfo = task.Result.ConvertTo<UserInfo>();

    //        // 내용 출력
    //        if (onComplete != null)
    //        {
    //            onComplete(loadInfo);
    //        }           
    //    }
    //    else
    //    {
    //        print("화원정보 불러오기 실패 : " + task.Exception);
    //    }
    //}
}
