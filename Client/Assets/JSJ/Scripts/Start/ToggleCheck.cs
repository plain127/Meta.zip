using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleCheck : MonoBehaviour
{
    public Toggle t_Fashion;
    public Toggle t_Sports;
    public Toggle t_Cook;

    void Start()
    {
        t_Fashion.onValueChanged.AddListener(OnToggleChanged);
        t_Sports.onValueChanged.AddListener(OnToggleChanged);
        t_Cook.onValueChanged.AddListener(OnToggleChanged);
    }

    void Update()
    {
        
    }

    public void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            Debug.Log("üũ ǥ�� Ȱ��ȭ");
        }
        else
        {
            Debug.Log("üũ ǥ�� ��Ȱ��ȭ");
        }

    }
}
