using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUIController : MonoBehaviour
{
    //    public GameObject[] icons;   // ������ �迭

    //    public float duration = 0.5f;

    //    Vector3[] originPos;

    //    bool isMenuOpen = false;

    //    void Start()
    //    {
    //        originPos = new Vector3[icons.Length];

    //        for (int i = 0; i < icons.Length; i++)
    //        {
    //            // �ʱ� ��ġ ����
    //            originPos[i] = icons[i].transform.localPosition;

    //            // ������ ��Ȱ��ȭ
    //            icons[i].SetActive(false);
    //        }
    //    }

    //    // ----------------------------------------------------------------------------------------------- [ Menu Bar ]
    //    // Menu Toggle �Լ�
    //    public void MenuToggle()
    //    {
    //        if (isMenuOpen)
    //        {
    //            MenuClose();
    //        }
    //        else
    //        {
    //            MenuOpen();
    //        }

    //        isMenuOpen = !isMenuOpen;
    //    }

    //    // Menu ���� �Լ�
    //    public void MenuOpen()
    //    {
    //        for (int i = 0; i < icons.Length; i++)
    //        {
    //            icons[i].transform.DOLocalMove(new Vector3(250 - 960 + (i * 150), 450, 0), duration);

    //            // ������ Ȱ��ȭ
    //            icons[i].SetActive(true);
    //        }
    //    }

    //    // Menu �ݴ� �Լ�
    //    public void MenuClose()
    //    {
    //        for (int i = 0; i < icons.Length; i++)
    //        {
    //            // �ִϸ��̼��� ������,
    //            icons[i].transform.DOLocalMove(originPos[i], duration).OnComplete(Gameoff);
    //        }
    //    }

    //    public void Gameoff()
    //    {
    //        for (int i = 0; i < icons.Length; i++)
    //        {
    //            // ������ ��Ȱ��ȭ
    //            icons[i].SetActive(false);
    //        }
    //    }


    //    // ----------------------------------------------------------------------------------------------- [ Game Quit ]
    //    public void QuitGame()
    //    {
    //        Debug.Log("Game is exiting...");
    //        Application.Quit();

    //        // Note: Application.Quit() will not work in the Unity Editor.
    //        // To test it in the Editor, you can stop play mode:
    //#if UNITY_EDITOR
    //        UnityEditor.EditorApplication.isPlaying = false;
    //#endif
    //    }

    public GameObject menu;
    bool isMenuOpen = false;
    
    public void OnClickMenuBar()
    {
        if (isMenuOpen)
        {
            // MenuBar ����
            menu.transform.DOLocalMove(new Vector3(-1040, 240, 0), 0.5f);
        }
        else
        {
            // MenuBar ��
            menu.transform.DOLocalMove(new Vector3(-860, 240, 0), 0.5f);
        }

        isMenuOpen = !isMenuOpen;
    }
}
