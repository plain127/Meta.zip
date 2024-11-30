using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�

public class ToolbtnMgr : MonoBehaviour
{
    public static ToolbtnMgr instance;

    // TextMeshPro Button�� ����
    public GameObject BUTTONtmp; // TextMeshPro Button�� �Ҵ��ؾ� �� (GameObject ����)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {

    }

    void Update()
    {
        // V Ű�� ������ �� BUTTONtmp ��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (BUTTONtmp != null)
            {
                BUTTONtmp.SetActive(false); // TMP ��ư ��Ȱ��ȭ
            }
            else
            {
                Debug.LogError("BUTTONtmp�� �Ҵ���� �ʾҽ��ϴ�. TextMeshPro Button�� Inspector�� �����ϼ���.");
            }
        }
    }

    public void Exitpanel()
    {
        // ���� ��ũ��Ʈ�� �Ҵ�� UI�� ��Ȱ��ȭ
        this.gameObject.SetActive(false);

        // Canvas_Inventory�� ù ��° �ڽ��� Ȱ��ȭ
        GameObject inventoryUI = GameObject.Find("Canvas_Inventory").transform.GetChild(0).gameObject;
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(true);
        }
        else
        {
            Debug.LogError("Canvas_Inventory �Ǵ� ù ��° �ڽ��� ã�� �� �����ϴ�.");
        }
    }
}