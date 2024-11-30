using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatKey : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        GoToMagazineScene();
    }

    public void GoToMagazineScene()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            MetaConnectionMgr.instance.TownToMagazine();
        }
    }
}
