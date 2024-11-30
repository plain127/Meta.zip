using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenubarMgr : MonoBehaviour
{
    public GameObject roombar;
    public Button menuButton;

    // �޴��ٰ� �����ִ���
    bool isMenuOpen = false;

    void Start()
    {
        menuButton.onClick.AddListener(OnClickMenuBar);
    }
     
    public void OnClickMenuBar()
    {
        if (isMenuOpen)
        {
            // MenuBar ����
            roombar.transform.DOLocalMove(new Vector3(-1040, 240, 0), 0.5f);
        }
        else
        {
            // MenuBar ��
            roombar.transform.DOLocalMove(new Vector3(-860, 240, 0), 0.5f);
        }

        isMenuOpen = !isMenuOpen;
    }
}
