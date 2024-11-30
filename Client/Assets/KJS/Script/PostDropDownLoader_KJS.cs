using System;
using System.Collections.Generic;
using System.IO;
using TMPro; // TextMeshPro ���ӽ����̽�
using UnityEngine;

public class PostDropDownLoader_KJS : MonoBehaviour
{
    public TMP_Dropdown postDropdown; // TextMeshPro Dropdown
    public SaveMgr_KJS saveManager; // SaveMgr_KJS ��ũ��Ʈ�� ����

    private string rootDirectory; // postId �������� ��ġ�� ��Ʈ ���丮

    private void Start()
    {
        // postId �������� ����� ���丮 ����
        rootDirectory = Application.dataPath + "/KJS/UserInfo";

        Debug.Log($"Looking for postId folders in directory: {rootDirectory}");
        if (Directory.Exists(rootDirectory))
        {
            LoadPostIdsFromDirectories();
        }
        else
        {
            Debug.LogError($"���丮�� �������� �ʽ��ϴ�: {rootDirectory}");
        }

        // Dropdown�� onValueChanged �̺�Ʈ�� �̺�Ʈ ������ ���
        if (postDropdown != null)
        {
            postDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    // postId ������ Ž���ϰ� Magazine.json ���Ͽ��� postId �ҷ�����
    private void LoadPostIdsFromDirectories()
    {
        List<string> postIdList = new List<string>
        {
            "���� ���" // �⺻������ �߰�
        };

        try
        {
            // ��Ʈ ���丮�� ���� �������� �˻�
            string[] directories = Directory.GetDirectories(rootDirectory);

            Debug.Log($"�� {directories.Length}���� ������ ã�ҽ��ϴ�.");

            foreach (string directory in directories)
            {
                string jsonFilePath = Path.Combine(directory, "Magazine.json"); // Magazine.json ���

                if (File.Exists(jsonFilePath))
                {
                    Debug.Log($"JSON ���� �߰�: {jsonFilePath}");

                    // JSON ���� �б�
                    string json = File.ReadAllText(jsonFilePath);

                    // JSON ������ �Ľ�
                    RootObject rootData = JsonUtility.FromJson<RootObject>(json);

                    // postId ���� �� ����Ʈ�� �߰�
                    if (rootData != null && rootData.posts != null && rootData.posts.Count > 0)
                    {
                        foreach (var post in rootData.posts)
                        {
                            Debug.Log($"postId: {post.postId}");
                            postIdList.Add(post.postId);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"JSON ������ ����ְų� posts �����Ͱ� �����ϴ�: {jsonFilePath}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Magazine.json ������ �������� �ʽ��ϴ�: {jsonFilePath}");
                }
            }

            // Dropdown�� postId �߰�
            PopulateDropdownWithPostId(postIdList);
        }
        catch (Exception e)
        {
            Debug.LogError($"���� �� ������ Ž���ϴ� �� ���� �߻�: {e.Message}");
        }
    }

    // Dropdown�� postId�� �߰�
    private void PopulateDropdownWithPostId(List<string> postIdList)
    {
        if (postDropdown == null)
        {
            Debug.LogError("postDropdown�� ������� �ʾҽ��ϴ�!");
            return;
        }

        postDropdown.ClearOptions(); // ���� �ɼ� �ʱ�ȭ
        postDropdown.AddOptions(postIdList); // postId �߰�
        Debug.Log($"Dropdown�� �߰��� postId: {string.Join(", ", postIdList)}");

        // �⺻ ���ð����� "���� ���"�� ����
        postDropdown.value = 0; // �ε��� 0 ����
        OnDropdownValueChanged(0); // �⺻�� �ε�
    }

    // Dropdown���� ���õ� ���� ����Ǿ��� �� ȣ��Ǵ� �̺�Ʈ
    private void OnDropdownValueChanged(int index)
    {
        if (postDropdown == null) return;

        // ���õ� postId ��������
        string selectedPostId = postDropdown.options[index].text;

        Debug.Log($"Dropdown���� ���õ� postId: {selectedPostId}");

        // "���� ���"�� �������� �ʵ��� ����
        if (selectedPostId == "���� ���")
        {
            Debug.Log("�⺻ �׸� '���� ���'�� ���õǾ����ϴ�. �ƹ� �۾��� �������� �ʽ��ϴ�.");
            return;
        }

        // SaveMgr_KJS�� LoadSpecificPostById ȣ��
        if (saveManager != null)
        {
            saveManager.LoadSpecificPostById(selectedPostId);
        }
        else
        {
            Debug.LogError("SaveMgr_KJS�� ������� �ʾҽ��ϴ�.");
        }
    }
}