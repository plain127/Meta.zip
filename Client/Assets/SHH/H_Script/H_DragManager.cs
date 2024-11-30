using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class H_DragManager : MonoBehaviour
{
    public static H_DragManager inst;
    public bool isDragging = false;
    public GameObject postit;
    public Transform noticepos;

    public LayerMask layermask;
    RaycastHit hitInfo;
    Vector3 targetPos;

    public bool isColliding = false;

    public GameObject currentPostIt;

    public GameObject Popup_merge; //Äµ¹ö½º
    public Transform viewcontents; //½ºÅ©·Ñºä
    public RawImage view1;
    public RawImage view2;

    //UI´Ý±â ¹öÆ°
    public Button Btn_close_mergeview;

    private void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        Popup_merge.SetActive(false);
    }

    void Update()
    {

        OnMouseButtonDown();

        if (Input.GetMouseButton(0))
        {
            OnMouseDragging();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseButtonUp();
        }

    }
    private void OnMouseButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }
    }
    private void OnMouseDragging()
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layermask))
            {
                currentPostIt = hitInfo.transform.gameObject;
                targetPos = hitInfo.point;
                targetPos.z = hitInfo.transform.position.z;
                hitInfo.transform.position = targetPos;
            }

        }

    }

    private void OnMouseButtonUp()
    {
        isDragging = false;

        // Å¬¸¯µÈ Æ÷½ºÆ®ÀÕ È®ÀÎ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layermask))
        {
            if(hitInfo.transform.name.Contains("MergeNewItem"))
            {
                GameObject clickedPostIt = hitInfo.transform.gameObject;
                H_NewFolder nf = clickedPostIt.GetComponent<H_NewFolder>();

                Popup_merge.SetActive(true);


                view1.texture = nf.texs[0];
                view2.texture = nf.texs[1];

            }
      

        }
    }

    public void OnClickCloseContentview()
    {
        Popup_merge.gameObject.SetActive(false);
    }
}
