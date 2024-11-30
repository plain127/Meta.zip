using System;
using System.Collections.Generic;
using System.IO;
using TMPro; // TextMeshPro 네임스페이스
using UnityEngine;

public class PostDropDownLoader_KJS : MonoBehaviour
{
    public TMP_Dropdown postDropdown; // TextMeshPro Dropdown
    public SaveMgr_KJS saveManager; // SaveMgr_KJS 스크립트를 참조

    private string rootDirectory; // postId 폴더들이 위치한 루트 디렉토리

    private void Start()
    {
        // postId 폴더들이 저장된 디렉토리 설정
        rootDirectory = Application.dataPath + "/KJS/UserInfo";

        Debug.Log($"Looking for postId folders in directory: {rootDirectory}");
        if (Directory.Exists(rootDirectory))
        {
            LoadPostIdsFromDirectories();
        }
        else
        {
            Debug.LogError($"디렉토리가 존재하지 않습니다: {rootDirectory}");
        }

        // Dropdown의 onValueChanged 이벤트에 이벤트 리스너 등록
        if (postDropdown != null)
        {
            postDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
    }

    // postId 폴더를 탐색하고 Magazine.json 파일에서 postId 불러오기
    private void LoadPostIdsFromDirectories()
    {
        List<string> postIdList = new List<string>
        {
            "문서 목록" // 기본값으로 추가
        };

        try
        {
            // 루트 디렉토리의 하위 폴더들을 검색
            string[] directories = Directory.GetDirectories(rootDirectory);

            Debug.Log($"총 {directories.Length}개의 폴더를 찾았습니다.");

            foreach (string directory in directories)
            {
                string jsonFilePath = Path.Combine(directory, "Magazine.json"); // Magazine.json 경로

                if (File.Exists(jsonFilePath))
                {
                    Debug.Log($"JSON 파일 발견: {jsonFilePath}");

                    // JSON 파일 읽기
                    string json = File.ReadAllText(jsonFilePath);

                    // JSON 데이터 파싱
                    RootObject rootData = JsonUtility.FromJson<RootObject>(json);

                    // postId 추출 및 리스트에 추가
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
                        Debug.LogWarning($"JSON 파일이 비어있거나 posts 데이터가 없습니다: {jsonFilePath}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Magazine.json 파일이 존재하지 않습니다: {jsonFilePath}");
                }
            }

            // Dropdown에 postId 추가
            PopulateDropdownWithPostId(postIdList);
        }
        catch (Exception e)
        {
            Debug.LogError($"폴더 및 파일을 탐색하는 중 오류 발생: {e.Message}");
        }
    }

    // Dropdown에 postId를 추가
    private void PopulateDropdownWithPostId(List<string> postIdList)
    {
        if (postDropdown == null)
        {
            Debug.LogError("postDropdown이 연결되지 않았습니다!");
            return;
        }

        postDropdown.ClearOptions(); // 기존 옵션 초기화
        postDropdown.AddOptions(postIdList); // postId 추가
        Debug.Log($"Dropdown에 추가된 postId: {string.Join(", ", postIdList)}");

        // 기본 선택값으로 "문서 목록"을 선택
        postDropdown.value = 0; // 인덱스 0 선택
        OnDropdownValueChanged(0); // 기본값 로드
    }

    // Dropdown에서 선택된 값이 변경되었을 때 호출되는 이벤트
    private void OnDropdownValueChanged(int index)
    {
        if (postDropdown == null) return;

        // 선택된 postId 가져오기
        string selectedPostId = postDropdown.options[index].text;

        Debug.Log($"Dropdown에서 선택된 postId: {selectedPostId}");

        // "문서 목록"은 동작하지 않도록 설정
        if (selectedPostId == "문서 목록")
        {
            Debug.Log("기본 항목 '문서 목록'이 선택되었습니다. 아무 작업도 수행하지 않습니다.");
            return;
        }

        // SaveMgr_KJS의 LoadSpecificPostById 호출
        if (saveManager != null)
        {
            saveManager.LoadSpecificPostById(selectedPostId);
        }
        else
        {
            Debug.LogError("SaveMgr_KJS가 연결되지 않았습니다.");
        }
    }
}