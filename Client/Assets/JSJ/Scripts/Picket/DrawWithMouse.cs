using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Unity.Mathematics;
using TMPro;

public class DrawWithMouse : MonoBehaviourPun //IPunObservable
{
    public Camera uiCamera;

    public Canvas canvasPicket;   // Picket UI Canvas

    public Line line;

    public GameObject stickerPrefab;

    [Header("SetParent")]
    public GameObject lineParent;      // Line들이 모인 오브젝트
    public GameObject stickerParent;   // Sticker들이 모인 오브젝트

    [Header("Tool 버튼")]
    public Button drawButton;
    public Button stickerButton;

    [Header("커서 이미지")]
    public Texture2D penIcon;
    public Texture2D stickerIcon;

    [Header("Bool")]
    public bool isDrawing = false;
    public bool isAttaching = false;
    public bool isCursorActive = false;

    GameObject nickNamePrefab;
    TMP_Text text_NickName;

    float timestep;

    Vector2 nickNamePos;


    private void Start()
    {
        drawButton.onClick.AddListener(DrawMode);
        stickerButton.onClick.AddListener(StickerMode);
    }

    // 닉네임 생성 동기화 함수
    [PunRPC]
    public void CreateNickName()
    {
        // 닉네임 프리팹 생성
        nickNamePrefab = PhotonNetwork.Instantiate("NickNamePrefab", Vector3.zero, Quaternion.identity);
        // 닉네임 프리팹의 Text 자식 받아오기
        text_NickName = nickNamePrefab.GetComponentInChildren<TMP_Text>();

        text_NickName.text = PhotonNetwork.NickName;

        nickNamePrefab.SetActive(true);
    }

    [PunRPC]
    public void HideNickName()
    {
        if (nickNamePrefab != null)
        {
            nickNamePrefab.SetActive(false);
        }
    }


    // ------------------------------------------------------------------------------------------------------- [ Draw ]
    // 그리기 모드
    public void DrawMode()
    {
        isDrawing = !isDrawing;

        // 만일, 그리기 상태라면
        if (isDrawing)
        {
            // 커서를 펜 아이콘으로 설정
            SetPenIcon();

            // 닉네임 생성 함수 RPC 호출
            //photonView.RPC("CreateNickName", RpcTarget.AllBuffered);
           
            // 붙이기 비활성화
            isAttaching = false;
        }
        else
        {
            // 커서 초기화
            ResetCursor();

            //photonView.RPC("HideNickName", RpcTarget.AllBuffered);
        }
    }


    // ------------------------------------------------------------------------------------------------------- [ Sticker ]
    // 스티커 모드
    public void StickerMode()
    {
        isAttaching = !isAttaching;

        // 만일, 붙이기 상태라면
        if (isAttaching)
        {
            // 커서를 스티커 아이콘으로 설정
            SetStickerIcon();

            // 그리기 비활성화
            isDrawing = false;
        }
        else
        {
            // 커서 초기화
            ResetCursor();
        }
    }
   

    private void Update()
    {
        // 그리기 모드일 때
        if (isDrawing == true)
        {
            //UpdateNickName();
        }

        if (text_NickName != null)
        {
            //text_NickName.transform.position = nickNamePos;
        }
        


        // ------------------------------------------------------------------------------------------------------- [ Start Draw ]
        // 그리기가 true이고, 마우스를 눌렀을 때
        if (Input.GetMouseButtonDown(0) && isDrawing == true)
        {
            CreateNewLine();
        }

        // ------------------------------------------------------------------------------------------------------- [ Drawing ]
        // 그리기가 true이고, 마우스를 누르고 있을 때
        if (Input.GetMouseButton(0) && isDrawing == true)
        {
            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (Time.time - timestep > 0.01f)
                {
                    Vector3 point = hitInfo.point + new Vector3(0, 0, -0.1f);

                    line.AddPoint(point);

                    timestep = Time.time;
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------- [ Start Sticker ]
        // 붙이기가 true이고, 마우스를 눌렀을 때
        if (Input.GetMouseButtonDown(0) && isAttaching == true)
        {
            Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                AttachSticker();
            }
        }
    }


    // ------------------------------------------------------------------------------------------------------[ Line ]
    // 라인 생성 함수
    void CreateNewLine()
    {
        GameObject lineInstance = PhotonNetwork.Instantiate("Line", Vector3.zero, Quaternion.identity);

        // 로컬에서 라인 오브젝트의 부모 설정
        lineInstance.transform.SetParent(lineParent.transform);

        PhotonView linePhotonView = lineInstance.GetComponent<PhotonView>();

        // 상대방에게도 라인 오브젝트의 부모를 동기화
        photonView.RPC("SetLineParent", RpcTarget.OthersBuffered, linePhotonView.ViewID);

        line = lineInstance.GetComponent<Line>();
    }

    // 라인 오브젝트 부모 동기화 함수
    [PunRPC]
    void SetLineParent(int lineViewID)
    {
        GameObject lineObject = PhotonView.Find(lineViewID).gameObject;
        lineObject.transform.SetParent(lineParent.transform);
    }


    // ---------------------------------------------------------------------------------------------------[ Sticker ]
    // 스티커 생성 함수
    void AttachSticker()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasPicket.transform as RectTransform,
            Input.mousePosition,
            uiCamera,
            out Vector2 localPoint
            );

        photonView.RPC("Sticker", RpcTarget.AllBuffered, localPoint);
    }
    
    // 스티커 생성 동기화
    [PunRPC]
    void Sticker(Vector2 localPoint)
    {
        GameObject stickerInstance = Instantiate(stickerPrefab, canvasPicket.transform);
        stickerInstance.transform.SetParent(stickerParent.transform);
        stickerInstance.transform.localPosition = localPoint;
    }


    // ----------------------------------------------------------------------------------------------------[ Cursor ]
    // 커서를 펜 아이콘으로 설정
    public void SetPenIcon()
    {
        Cursor.SetCursor(penIcon, Vector2.zero, CursorMode.Auto);
    }

    // 커서를 스티커 아이콘으로 설정
    public void SetStickerIcon()
    {
        Cursor.SetCursor(stickerIcon, Vector2.zero, CursorMode.Auto);
    }

    // 커서 초기화
    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    // --------------------------------------------------------------------------------------------------[ NIckName ]
    // 닉네임 프리팹 위치 업데이트
    public void UpdateNickName()
    {
        Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit nickNameInfo;

        if (Physics.Raycast(ray, out nickNameInfo))
        {
            Vector3 offset = new Vector3(60, -60, 0);

            // 닉네임 프리팹 위치
            nickNamePos = Input.mousePosition + offset;
            text_NickName.transform.position = nickNamePos;
        }
    }

    // 닉네임 프리팹 위치 동기화
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(nickNamePos);
    //    }
    //    else
    //    {
    //        nickNamePos = (Vector2)stream.ReceiveNext();
    //    }

        
    //}
}
