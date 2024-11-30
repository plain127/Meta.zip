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

    //USER->Email-> �̸� �г��� ������� ����ī�װ�

    IEnumerator CoSaveUserInfo(H_userinfo info)
    {
        //���� ��� USER/UserID/������
        string path = "USER/" + FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        //���� ���� ��û
        Task task = store.Document(path).SetAsync(info);
        
        //����� �Ϸ�ɶ����� ��ٷ�!
        yield return new WaitUntil(() => { return task.IsCompleted; });

        if (task.Exception == null)
        {
            print("���� ���� ���� ����!");
        }
        else //json����
        {
            print("���� ����! :" + task.Exception);
        }
    }
    void Update()
    {
        
    }
}
