using System;
using System.IO;
using UnityEngine;
using TMPro; // TextMeshPro ���
using Photon.Pun; // PhotonNetwork ���
using Photon.Realtime; // PhotonRoom ���

public class Billboard : MonoBehaviourPunCallbacks, IPunObservable
{
    private Transform camTransform; // ī�޶� Transform

    [Header("TMP �ؽ�Ʈ ���")]
    [SerializeField]
    private TextMeshProUGUI textMeshPro; // TMP �ؽ�Ʈ�� ����� ������Ʈ

    [Header("PicketId_KJS ����")]
    [SerializeField]
    private PicketId_KJS picketIdComponent; // PicketId_KJS ������Ʈ ����

    [Header("��ũ���� ��� Ȯ�ο� (Debug)")]
    public string screenshotPathDebug; // PicketId_KJS�� ��θ� Inspector�� ����

    private string lastScreenshotPath; // ���� ��θ� ������ ���� ����
    private string formattedText; // ���� �ؽ�Ʈ ����
    private string photonNickname; // PhotonNetwork �г���

    void Start()
    {
        // PicketId_KJS ������Ʈ �ڵ����� ã��
        if (picketIdComponent == null)
        {
            picketIdComponent = FindObjectOfType<PicketId_KJS>();
        }

        if (picketIdComponent == null)
        {
            Debug.LogError("PicketId_KJS ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // �ʱ� �г��� �� ��� ����
        photonNickname = GetPhotonNickname();
        lastScreenshotPath = picketIdComponent.GetScreenshotPath();
        screenshotPathDebug = lastScreenshotPath;

        UpdateBillboardText();
    }

    void LateUpdate()
    {
        // ������ ���: ������Ʈ�� �׻� ī�޶� �ٶ󺸵��� ȸ��
        if (camTransform == null)
        {
            if (Camera.main != null)
            {
                camTransform = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("Main Camera�� ���� �����ϴ�. ī�޶� �������� �Ҵ��ؾ� �մϴ�.");
                return;
            }
        }

        // ������Ʈ�� forward ������ ī�޶��� �������� ����
        transform.forward = camTransform.forward;
    }

    void Update()
    {
        // PicketId_KJS�� ��� ���� ����
        string currentPath = picketIdComponent.GetScreenshotPath();
        if (currentPath != lastScreenshotPath)
        {
            lastScreenshotPath = currentPath;
            screenshotPathDebug = currentPath;

            // ��� ������Ʈ �� ����ȭ
            UpdateBillboardText();
            photonView.RPC("SyncBillboardData", RpcTarget.Others, photonNickname, screenshotPathDebug);
        }
    }

    /// <summary>
    /// �ܺο��� ��θ� ���� ������Ʈ�� �� �ֵ��� �����Ǵ� �޼���
    /// </summary>
    /// <param name="newPath">���ο� ��ũ���� ���</param>
    public void UpdateScreenshotPath(string newPath)
    {
        if (newPath != lastScreenshotPath)
        {
            lastScreenshotPath = newPath;
            screenshotPathDebug = newPath;

            // �ؽ�Ʈ ������Ʈ �� ����ȭ
            UpdateBillboardText();
            photonView.RPC("SyncBillboardData", RpcTarget.Others, photonNickname, screenshotPathDebug);
        }
    }

    /// <summary>
    /// TMP �ؽ�Ʈ�� ������Ʈ
    /// </summary>
    private void UpdateBillboardText()
    {
        if (!string.IsNullOrEmpty(screenshotPathDebug) && File.Exists(screenshotPathDebug))
        {
            try
            {
                // ��ũ���� ������ ���� ��¥ ��������
                DateTime fileDate = File.GetCreationTime(screenshotPathDebug);
                string formattedDate = $"{fileDate.Month}�� {fileDate.Day}��"; // ��¥ ����

                // �ؽ�Ʈ ����
                formattedText = $"{photonNickname}�� ����";

                // TMP �ؽ�Ʈ ������Ʈ
                if (textMeshPro != null)
                {
                    textMeshPro.text = formattedText;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("��ũ���� ��¥�� �д� �� ���� �߻�: " + ex.Message);
            }
        }
        else
        {
            formattedText = $"{photonNickname}�� ��ũ�� ��¥�� ã�� �� ����";

            // TMP �ؽ�Ʈ ������Ʈ
            if (textMeshPro != null)
            {
                textMeshPro.text = formattedText;
            }
        }
    }

    /// <summary>
    /// PhotonNetwork���� �г��� ��������
    /// </summary>
    private string GetPhotonNickname()
    {
        // PhotonNetwork�� ����� ���¿��� �г��� ��ȯ
        if (PhotonNetwork.IsConnected)
        {
            return PhotonNetwork.NickName;
        }
        else
        {
            return "Guest"; // ������� �ʾ��� �� �⺻ �г���
        }
    }

    /// <summary>
    /// RPC�� ���� ������ �����͸� ����ȭ
    /// </summary>
    /// <param name="nickname">Photon �г���</param>
    /// <param name="screenshotPath">��ũ���� ���</param>
    [PunRPC]
    private void SyncBillboardData(string nickname, string screenshotPath)
    {
        photonNickname = nickname;
        screenshotPathDebug = screenshotPath;

        // ������ �ؽ�Ʈ ������Ʈ
        UpdateBillboardText();
    }

    /// <summary>
    /// Photon�� ������ ����ȭ (IPunObservable ����)
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���� ������ ����
            stream.SendNext(photonNickname);
            stream.SendNext(screenshotPathDebug);
        }
        else
        {
            // ���� ������ ����
            photonNickname = (string)stream.ReceiveNext();
            screenshotPathDebug = (string)stream.ReceiveNext();

            // �ؽ�Ʈ ������Ʈ
            UpdateBillboardText();
        }
    }
}