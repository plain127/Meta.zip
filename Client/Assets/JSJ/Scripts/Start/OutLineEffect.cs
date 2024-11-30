using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineEffect : MonoBehaviour
{
    public Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();

        // 아웃라인 비활성화
        outline.enabled = false;
    }

    // 커서를 오브젝트에 올렸을 때
    public void OnMouseEnter()
    {
        // 아웃라인 활성화
        outline.enabled = true;
    }

    // 커서를 오브젝트에서 내렸을 때
    public void OnMouseExit()
    {
        // 아웃라인 비활성화
        outline.enabled = false;
    }
}
