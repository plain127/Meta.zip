using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarManager : MonoBehaviour
{
    public static AvatarManager instance;

    public int avatarCode;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 아바타 랜덤 코드 생성
    public void RandomAvatartCode()
    {
        int randomIndex = Random.Range(0, 4);

        avatarCode = randomIndex;
        print(avatarCode);
    }

}
