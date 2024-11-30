using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string Meta_Town_Scene;
    public string Meta_Studio_Scene;

    public void OnClicktown()
    {
        TownSceneChange();
    }
    public void OnClickStudio()
    {
        StudioSceneChange();
    }
    public void TownSceneChange()
    {
        SceneManager.LoadScene("Meta_Town_Scene");
        Debug.Log("����ȯ����");
    }
    public void StudioSceneChange()
    {
        SceneManager.LoadScene("Meta_Studio_Scene");
        Debug.Log("����ȯ����");
    }

}
