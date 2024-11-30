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
            Debug.Log("체크 표시 활성화");
        }
        else
        {
            Debug.Log("체크 표시 비활성화");
        }

    }
}
