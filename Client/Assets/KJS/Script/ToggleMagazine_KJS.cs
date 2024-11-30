using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMagazine_KJS : MonoBehaviour
{
    public GameObject uiElement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            uiElement.SetActive(!uiElement.activeSelf);
        }
    }
}
