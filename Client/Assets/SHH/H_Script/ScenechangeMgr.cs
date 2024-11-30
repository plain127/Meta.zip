using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScenechangeMgr : MonoBehaviour
{
    public string townscene;

    void Start()
    {
        if (!string.IsNullOrEmpty(townscene))
        {
            GameObject ChangeScene = GameObject.Find(townscene);

            if (ChangeScene != null)
            {
                ChangeScene.SetActive(false);
            }
            else
            {
                Debug.LogWarning("¸øÃ£°Ú´Ù ²Ò²¿¸®!");
            }
        }
        
    }
}
