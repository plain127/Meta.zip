using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MetaZip_FireStorage : MonoBehaviour
{
    public static MetaZip_FireStorage instance;
    FirebaseStorage storage;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            byte[] data = File.ReadAllBytes("C:\\Users\\Admin\\Documents\\GitHub\\new_unity\\Assets\\KJS\\UserInfo\\Magazine.json");  //  Encoding.UTF8.GetBytes("");
            string path = "POST/" + FirebaseAuth.DefaultInstance.CurrentUser.Email + "/post_" + 10;
           
            Upload(data, path);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            string path = "POST/" + FirebaseAuth.DefaultInstance.CurrentUser.Email + "/post_" + 10;
            Download(path, (byte [] data) => {
                string s = Encoding.UTF8.GetString(data);
                File.WriteAllText(Application.dataPath + "/test.json", s);
                print("111");
            });
        }
    }
    // Update is called once per frame
    public void Upload(byte[] data, string path)
    {
        StartCoroutine(CoUpload(data, path));
    }

    IEnumerator CoUpload(byte[] data, string path)
    {
        // 경로 설정 및 파일 이름
        StorageReference storageReference = storage.GetReference(path);

        // 파일 업로드 요청
        Task<StorageMetadata> task = storageReference.PutBytesAsync(data);
        // 통신이 완료될때까지 기다리자.
        yield return new WaitUntil(() => { return task.IsCompleted; });

        // 만약에 예외가 없다면
        if (task.Exception == null)
        {
            print("파일 업로드 성공");
        }
        else
        {
            print("파일 업로드 실패 : " + task.Exception);
        }
    }

    public void Download(string path, Action<byte[]> onComplete)
    {
        StartCoroutine(CoDownload(path, onComplete));
    }

    IEnumerator CoDownload(string path, Action<byte[]> onComplete)
    {
        // 경로 설정 및 파일 이름
        StorageReference storageReference = storage.GetReference(path);

        // 파일 다운로드 요청
        Task<byte[]> task = storageReference.GetBytesAsync(long.MaxValue);
        // 통신이 완료될때까지 기다리자.
        yield return new WaitUntil(() => { return task.IsCompleted; });

        // 만약에 예외가 없다면
        if (task.Exception == null)
        {
            if(onComplete !=null)
            {
                onComplete(task.Result);
            }
            //File.WriteAllBytes(Application.dataPath + "/testImage.jpg", task.Result);
            print("파일 다운로드 성공");
        }
        else
        {
            print("파일 다운로드 실패 : " + task.Exception);
        }
    }
}
