using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelUIController : MonoBehaviour
{
    public GameObject menu;
    bool isMenuOpen = false;

    public void OnClickMenuBar()
    {
        if (isMenuOpen)
        {
            // MenuBar ´ÝÀ½
            menu.transform.DOLocalMove(new Vector3(-80, 240, 0), 0.5f);
        }
        else
        {
            // MenuBar ¿®
            menu.transform.DOLocalMove(new Vector3(-860, 240, 0), 0.5f);
        }

        isMenuOpen = !isMenuOpen;
    }
}
