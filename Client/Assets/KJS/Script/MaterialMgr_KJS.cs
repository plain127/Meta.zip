using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Photon.Pun; // Photon 네임스페이스 추가

public class MaterialMgr_KJS : MonoBehaviourPunCallbacks // Photon의 MonoBehaviourPunCallbacks 상속
{
    [Header("PicketId_KJS 참조")]
    [SerializeField]
    private PicketId_KJS picketIdComponent; // PicketId_KJS 컴포넌트를 참조

    [Header("스크린샷 경로 (Debug)")]
    public string screenshotPathDebug; // PicketId_KJS의 경로를 Inspector에 표시

    public Texture autoAssignTexture; // 동적으로 로드된 텍스처

    private string lastScreenshotPath; // 이전 경로를 저장해 변경 감지

    void Start()
    {
        // PicketId_KJS 컴포넌트를 자동으로 찾거나 Inspector에서 설정
        if (picketIdComponent == null)
        {
            picketIdComponent = GetComponent<PicketId_KJS>();
        }

        if (picketIdComponent == null)
        {
            Debug.LogError("PicketId_KJS 컴포넌트를 찾을 수 없습니다. Inspector에서 직접 할당해야 합니다.");
            return;
        }

        // 초기 경로 가져오기
        lastScreenshotPath = picketIdComponent.GetScreenshotPath();
        screenshotPathDebug = lastScreenshotPath;

        // 경로가 유효한 경우 텍스처 로드
        if (!string.IsNullOrEmpty(lastScreenshotPath))
        {
            StartCoroutine(LoadTextureFromPath(lastScreenshotPath));
        }
        else
        {
            Debug.LogWarning("초기 스크린샷 경로가 비어 있습니다.");
        }
    }

    void Update()
    {
        // PicketId_KJS의 경로 변경 감지
        string currentPath = picketIdComponent.GetScreenshotPath();
        if (currentPath != lastScreenshotPath)
        {
            lastScreenshotPath = currentPath;
            screenshotPathDebug = currentPath;

            Debug.Log("스크린샷 경로가 변경되었습니다: " + currentPath);

            // 경로가 변경되었으므로 새 텍스처를 로드
            StartCoroutine(LoadTextureFromPath(currentPath));
        }
    }

    IEnumerator LoadTextureFromPath(string path)
    {
        // 로컬 파일에서 텍스처 로드
        string filePath = "file://" + path;

        using (WWW www = new WWW(filePath))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                autoAssignTexture = www.texture; // 텍스처 로드 성공
                photonView.RPC("AssignTextureToMaterialRPC", RpcTarget.AllBuffered); // 포톤 RPC 호출
            }
            else
            {
                Debug.LogError("Failed to load texture from path: " + path + "\nError: " + www.error);
            }
        }
    }

    [PunRPC]
    private void AssignTextureToMaterialRPC()
    {
        // 첫 번째 자식을 가져옴
        Transform firstChild = transform.GetChild(0);

        // 자식 오브젝트가 있는지 확인
        if (firstChild != null && firstChild.name == "PicketPicture")
        {
            Renderer renderer = firstChild.GetComponent<Renderer>();

            if (renderer != null && renderer.material != null && autoAssignTexture != null)
            {
                renderer.material.SetTexture("_BaseMap", autoAssignTexture); // Base Map에 텍스처 설정
                Debug.Log("Texture assigned to PicketPicture's material.");
            }
            else
            {
                Debug.LogWarning("Renderer, Material, or Texture is missing.");
            }
        }
        else
        {
            Debug.LogWarning("First child object is missing or not named 'PicketPicture'.");
        }
    }
}