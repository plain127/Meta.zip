using UnityEngine;
using Photon.Pun;

public class PrefabManager_KJS : MonoBehaviourPunCallbacks
{
    public GameObject uiTool; // UI ������Ʈ (MagazineView 2�� �ڽ�)
    public float detectionRange = 5f; // ���� ����
    public string postId; // LoadObjectsFromFile�� ������ postId

    private GameObject player;
    private bool isPlayerInRange = false;
    private bool isUIActive = false; // UI�� ���� ���¸� ����

    private LoadMgr_KJS loadMgr; // LoadMgr_KJS �ν��Ͻ� ����

    void Start()
    {
        // "Player" �±װ� ���� ������Ʈ ã��
        player = GameObject.FindGameObjectWithTag("Player");

        // LoadMgr_KJS ��ũ��Ʈ�� ������ ������Ʈ ã��
        loadMgr = FindObjectOfType<LoadMgr_KJS>();

        // "MagazineView 2" ������Ʈ�� ã��, �� �ڽ� �� "Tool 2"�� ã�� �Ҵ�
        GameObject magazineView = GameObject.Find("MagazineView 2");
        if (magazineView != null)
        {
            uiTool = magazineView.transform.Find("Tool 2")?.gameObject;
        }

        // UI�� ���������� �Ҵ�Ǿ����� ��Ȱ��ȭ
        if (uiTool != null)
            uiTool.SetActive(false);
        else
            Debug.LogWarning("uiTool�� ã�� �� �����ϴ�.");
    }

    void Update()
    {
        // ���� �÷��̾ �ƴϸ� Ű �Է��� ����
        if (!photonView.IsMine)
            return;

        if (player != null)
        {
            // �÷��̾���� �Ÿ� ���
            float distance = Vector3.Distance(player.transform.position, transform.position);

            // ���� �ȿ� ������ isPlayerInRange�� true�� ����
            if (distance <= detectionRange)
            {
                isPlayerInRange = true;
            }
            else
            {
                isPlayerInRange = false;

                // �÷��̾ ������ ����� UI�� �ڵ����� ��Ȱ��ȭ
                if (isUIActive)
                {
                    ToggleUI(false);
                }
            }

            // ���� ���� ���� �� V Ű�� ������ UI�� ����ϰ� LoadObjectsFromFile ȣ��
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.O))
            {
                ToggleUI(!isUIActive);  // ���� ���� ����

                // UI�� Ȱ��ȭ�� �� LoadMgr_KJS�� LoadObjectsFromFile ȣ��
                if (isUIActive && loadMgr != null)
                {
                    loadMgr.LoadObjectsFromFile(postId);
                }
            }
        }
    }

    // UI Ȱ��ȭ/��Ȱ��ȭ ó��
    private void ToggleUI(bool isActive)
    {
        if (uiTool != null)
        {
            uiTool.SetActive(isActive);
            isUIActive = isActive;
        }
    }
}