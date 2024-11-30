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
    public GameObject lineParent;      // Line���� ���� ������Ʈ
    public GameObject stickerParent;   // Sticker���� ���� ������Ʈ

    [Header("Tool ��ư")]
    public Button drawButton;
    public Button stickerButton;

    [Header("Ŀ�� �̹���")]
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

    // �г��� ���� ����ȭ �Լ�
    [PunRPC]
    public void CreateNickName()
    {
        // �г��� ������ ����
        nickNamePrefab = PhotonNetwork.Instantiate("NickNamePrefab", Vector3.zero, Quaternion.identity);
        // �г��� �������� Text �ڽ� �޾ƿ���
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
    // �׸��� ���
    public void DrawMode()
    {
        isDrawing = !isDrawing;

        // ����, �׸��� ���¶��
        if (isDrawing)
        {
            // Ŀ���� �� ���������� ����
            SetPenIcon();

            // �г��� ���� �Լ� RPC ȣ��
            //photonView.RPC("CreateNickName", RpcTarget.AllBuffered);
           
            // ���̱� ��Ȱ��ȭ
            isAttaching = false;
        }
        else
        {
            // Ŀ�� �ʱ�ȭ
            ResetCursor();

            //photonView.RPC("HideNickName", RpcTarget.AllBuffered);
        }
    }


    // ------------------------------------------------------------------------------------------------------- [ Sticker ]
    // ��ƼĿ ���
    public void StickerMode()
    {
        isAttaching = !isAttaching;

        // ����, ���̱� ���¶��
        if (isAttaching)
        {
            // Ŀ���� ��ƼĿ ���������� ����
            SetStickerIcon();

            // �׸��� ��Ȱ��ȭ
            isDrawing = false;
        }
        else
        {
            // Ŀ�� �ʱ�ȭ
            ResetCursor();
        }
    }
   

    private void Update()
    {
        // �׸��� ����� ��
        if (isDrawing == true)
        {
            //UpdateNickName();
        }

        if (text_NickName != null)
        {
            //text_NickName.transform.position = nickNamePos;
        }
        


        // ------------------------------------------------------------------------------------------------------- [ Start Draw ]
        // �׸��Ⱑ true�̰�, ���콺�� ������ ��
        if (Input.GetMouseButtonDown(0) && isDrawing == true)
        {
            CreateNewLine();
        }

        // ------------------------------------------------------------------------------------------------------- [ Drawing ]
        // �׸��Ⱑ true�̰�, ���콺�� ������ ���� ��
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
        // ���̱Ⱑ true�̰�, ���콺�� ������ ��
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
    // ���� ���� �Լ�
    void CreateNewLine()
    {
        GameObject lineInstance = PhotonNetwork.Instantiate("Line", Vector3.zero, Quaternion.identity);

        // ���ÿ��� ���� ������Ʈ�� �θ� ����
        lineInstance.transform.SetParent(lineParent.transform);

        PhotonView linePhotonView = lineInstance.GetComponent<PhotonView>();

        // ���濡�Ե� ���� ������Ʈ�� �θ� ����ȭ
        photonView.RPC("SetLineParent", RpcTarget.OthersBuffered, linePhotonView.ViewID);

        line = lineInstance.GetComponent<Line>();
    }

    // ���� ������Ʈ �θ� ����ȭ �Լ�
    [PunRPC]
    void SetLineParent(int lineViewID)
    {
        GameObject lineObject = PhotonView.Find(lineViewID).gameObject;
        lineObject.transform.SetParent(lineParent.transform);
    }


    // ---------------------------------------------------------------------------------------------------[ Sticker ]
    // ��ƼĿ ���� �Լ�
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
    
    // ��ƼĿ ���� ����ȭ
    [PunRPC]
    void Sticker(Vector2 localPoint)
    {
        GameObject stickerInstance = Instantiate(stickerPrefab, canvasPicket.transform);
        stickerInstance.transform.SetParent(stickerParent.transform);
        stickerInstance.transform.localPosition = localPoint;
    }


    // ----------------------------------------------------------------------------------------------------[ Cursor ]
    // Ŀ���� �� ���������� ����
    public void SetPenIcon()
    {
        Cursor.SetCursor(penIcon, Vector2.zero, CursorMode.Auto);
    }

    // Ŀ���� ��ƼĿ ���������� ����
    public void SetStickerIcon()
    {
        Cursor.SetCursor(stickerIcon, Vector2.zero, CursorMode.Auto);
    }

    // Ŀ�� �ʱ�ȭ
    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    // --------------------------------------------------------------------------------------------------[ NIckName ]
    // �г��� ������ ��ġ ������Ʈ
    public void UpdateNickName()
    {
        Ray ray = uiCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit nickNameInfo;

        if (Physics.Raycast(ray, out nickNameInfo))
        {
            Vector3 offset = new Vector3(60, -60, 0);

            // �г��� ������ ��ġ
            nickNamePos = Input.mousePosition + offset;
            text_NickName.transform.position = nickNamePos;
        }
    }

    // �г��� ������ ��ġ ����ȭ
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
