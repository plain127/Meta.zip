using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KeywordUI : MonoBehaviour
{
    public RawImage trendmap;          // Ʈ���� �̹����� ǥ���� UI ������Ʈ
    public APIManager apiManager;     // APIManager �ν��Ͻ�

    void Start()
    {
        if (apiManager != null)
        {
            // APIManager�� Trend �޼��� ȣ��
            apiManager.Trend();

            // Trend �̹����� �ε�� ������ ��ٸ� �� ������Ʈ
            StartCoroutine(UpdateTrendImage());
        }
        else
        {
            Debug.LogError("APIManager�� �������� �ʾҽ��ϴ�.");
        }
    }

    private IEnumerator UpdateTrendImage()
    {
        // APIManager���� Ʈ���� �̹����� �ε�� ������ ���
        while (apiManager.trendDownloadTexture == null)
        {
            yield return null; // �� ������ ���
        }

        // Ʈ���� �̹����� RawImage�� �Ҵ�
        trendmap.texture = apiManager.trendDownloadTexture;
        Debug.Log("Ʈ���� �̹����� ���������� ����Ǿ����ϴ�.");
    }
}
