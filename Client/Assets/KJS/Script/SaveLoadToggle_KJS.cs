using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadToggle_KJS : MonoBehaviour
{
    public GameObject uiPanel1;
    public GameObject uiPanel2;

    private void Start()
    {
        // ó�� ������ �� �гε��� ��Ȱ��ȭ
        uiPanel1.SetActive(false);
        uiPanel2.SetActive(false);
    }

    public void ToggleUIPanel1()
    {
        uiPanel1.SetActive(!uiPanel1.activeSelf);
    }

    public void ToggleUIPanel2()
    {
        uiPanel2.SetActive(!uiPanel2.activeSelf);
    }
}