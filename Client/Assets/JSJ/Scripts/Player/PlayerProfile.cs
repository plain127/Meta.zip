using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    // Layer 'player'를 가진 Player를 클릭하면 프로필 UI가 뜨게 하자.
    public float playerRayDistance = 5f;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            ClickOnPlayer();
        }
    }

    public void ClickOnPlayer()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, playerRayDistance, 1 << 16))
        {
            print(hitInfo.collider.gameObject.name);
                   
        }
    }

    
}