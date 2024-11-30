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
    //    // ���� ��� User/UserId/������
    //    string path = "POST/" + FirebaseAuth.DefaultInstance.CurrentUser.Email + "post_" + postId;

    //    //���� ���� ��û
    //    Task task = store.Document(path).SetAsync(info);
    //    // ����� �Ϸ�ɶ����� ��ٸ���.
    //    yield return new WaitUntil(() => { return task.IsCompleted; });

    //    // ���࿡ ���ܰ� ���ٸ�
    //    if (task.Exception == null)
    //    {
    //        print("�Խù� ���� ����");
    //    }
    //    else
    //    {
    //        print("�Խù� ���� ���� : " + task.Exception);
    //    }
    //}

    //public void LoadUserInfo(Action<UserInfo> onComplete/*�Ϸ�Ǿ��� �� ȣ��Ǵ� �Լ�*/)
    //{
    //    StartCoroutine(CoLoadUserInfo(onComplete));
    //}

    //IEnumerator CoLoadUserInfo(Action<UserInfo> onComplete)
    //{
    //    // ���� ��� User/UserID/������
    //    string path = "USER/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

    //    //���� ��ȸ ��û
    //    Task<DocumentSnapshot> task = store.Document(path).GetSnapshotAsync();
    //    // ����� �Ϸ�ɶ����� ��ٸ���.
    //    yield return new WaitUntil(() => { return task.IsCompleted; });

    //    // ���࿡ ���ܰ� ���ٸ�
    //    if (task.Exception == null)
    //    {
    //        print("ȸ������ �ҷ����� ����");

    //        // �ҷ��� ������ UserInfo ������ ����
    //        UserInfo loadInfo = task.Result.ConvertTo<UserInfo>();

    //        // ���� ���
    //        if (onComplete != null)
    //        {
    //            onComplete(loadInfo);
    //        }           
    //    }
    //    else
    //    {
    //        print("ȭ������ �ҷ����� ���� : " + task.Exception);
    //    }
    //}
}
