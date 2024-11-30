using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMgr : MonoBehaviour
{
    //삐약이 사용법
    public GameObject npctutorial;

    private void Awake()
    {
        
    }

    void Start()
    {
        npctutorial = transform.GetChild(0).gameObject;
        npctutorial.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            npctutorial.SetActive(false);
        }
    }

}
