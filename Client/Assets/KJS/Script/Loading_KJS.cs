using UnityEngine;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�
using System.Collections;
using UnityEngine.UI;
using Photon.Pun; // Photon ���ӽ����̽� �߰�

public class Loading_KJS : MonoBehaviour
{
    public RawImage[] rawImages; // �������� ������ �̹��� �迭 (4��)
    public float radius = 100f;  // ���� ������
    public float speed = 1f;     // �̵� �ӵ� (�ʴ� ȸ�� �ӵ�)
    public float angleOffset = 90f; // �� �̹��� ���� ���� ���� (4���� ��� �⺻������ 360 / 4 = 90��)

    public TextMeshProUGUI displayText; // TextMeshProUGUI ������Ʈ
    public string message = "��� ��ٷ��� �߾�!"; // �⺻ �޽���
    private string ellipsis = "..."; // "..." ��� �κ�
    public float typingSpeed = 0.1f; // �� ���ھ� ����ϴ� �ӵ�
    public float ellipsisSpeed = 0.5f; // "..."�� �� �ܰ辿 ��µǴ� �ӵ�

    private RectTransform[] rectTransforms;
    private float[] angles; // �� �̹����� ���� ���� (���� ������ ����)
    private bool isDisplaying = false; // �ؽ�Ʈ ��� ������ Ȯ��

    [Header("Photon Settings")]
    public string prefabName = "YourPrefabName"; // ������ �������� �̸�
    public Vector3 prefabScale = new Vector3(20f, 20f, 20f); // �������� ������
    public float prefabRotationX = -90f; // �������� x�� ȸ����
    public float forwardOffset = 0.5f; // �÷��̾� ������ ���� �Ÿ�

    private void OnEnable()
    {
        // Photon ������ ���� (�ּ� ó��)
        /*
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            // ���� �÷��̾ ������ "Player" �±��� ������Ʈ �˻�
            GameObject playerObject = FindLocalPlayerObject();

            if (playerObject != null)
            {
                Transform playerTransform = playerObject.transform;

                // ���� ��ġ ���
                Vector3 spawnPosition = playerTransform.position + playerTransform.forward * forwardOffset;

                // ���� ȸ���� ���
                Quaternion spawnRotation = Quaternion.Euler(prefabRotationX, playerTransform.rotation.eulerAngles.y, 0);

                // Photon ������ ����
                GameObject spawnedObject = PhotonNetwork.Instantiate(prefabName, spawnPosition, spawnRotation);

                // ������ �������� ������ ����
                spawnedObject.transform.localScale = prefabScale;
            }
            else
            {
                Debug.LogError("���� �÷��̾� ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Photon�� ����Ǿ� ���� �ʰų� �뿡 �������� �ʾҽ��ϴ�.");
        }
        */
    }

    //private GameObject FindLocalPlayerObject()
    //{
    //    // "Player" �±׸� ���� ��� ������Ʈ �˻�
    //    GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

    //    foreach (GameObject obj in playerObjects)
    //    {
    //        PhotonView photonView = obj.GetComponent<PhotonView>();

    //        // isMine�� true�� ������Ʈ�� ��ȯ
    //        if (photonView != null && photonView.IsMine)
    //        {
    //            return obj;
    //        }
    //    }

    //    return null; // ���� �÷��̾� ������Ʈ�� ã�� ���� ���
    //}

    void Start()
    {
        // �迭 �ʱ�ȭ
        rectTransforms = new RectTransform[rawImages.Length];
        angles = new float[rawImages.Length];

        // �� RawImage�� RectTransform ������Ʈ�� �������� �ʱ� ���� ����
        for (int i = 0; i < rawImages.Length; i++)
        {
            if (rawImages[i] != null)
            {
                rectTransforms[i] = rawImages[i].GetComponent<RectTransform>();
                angles[i] = Mathf.Deg2Rad * (i * angleOffset); // ���� ���̸� �������� ��ȯ�Ͽ� �ʱ�ȭ
            }
        }

        // �ؽ�Ʈ ��� ����
        if (displayText != null)
        {
            StartCoroutine(DisplayMessage());
        }
    }

    void Update()
    {
        for (int i = 0; i < rectTransforms.Length; i++)
        {
            if (rectTransforms[i] != null)
            {
                // �ð��� ���� ������ �����Ͽ� �ð� �������� �̵�
                angles[i] -= speed * Time.deltaTime;

                // ���� ��� ���
                float x = Mathf.Cos(angles[i]) * radius;
                float y = Mathf.Sin(angles[i]) * radius;

                // �̹��� ��ġ ����
                rectTransforms[i].anchoredPosition = new Vector2(x, y);
            }
        }
    }

    IEnumerator DisplayMessage()
    {
        while (true)
        {
            // �� ���ھ� �޽��� ���
            displayText.text = "";
            foreach (char c in message)
            {
                displayText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            // "..." ��� �ݺ�
            for (int i = 0; i < 3; i++) // 3�� �ݺ�
            {
                displayText.text = message + ellipsis.Substring(0, (i % 3) + 1); // "��� ��ٷ��� �߾�!.", "..", "..."
                yield return new WaitForSeconds(ellipsisSpeed);
            }

            // �ʱ�ȭ �� �ٽ� �ݺ�
            yield return new WaitForSeconds(ellipsisSpeed);
        }
    }
}