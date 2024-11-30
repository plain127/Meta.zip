using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using ReqRes;
using Firebase.Firestore;
using UniHumanoid;

public class APIManager : MonoBehaviour
{
    public FireStore firestore;

    AuthURL authURL = new AuthURL();
    ArticleURL articleURL = new ArticleURL();
    AIURL aiUrl = new AIURL();
    public void Start()
    {
        //GameObject firebase = GameObject.Find("Firebase");
        firestore = FireStore.instance;
    }


    //회원가입
    public void Auth()
    {
        string authUrl = authURL.authURL;
        StartCoroutine(AuthHttp(authUrl));
    }


    //기사 호출 관련 함수
    public void SearchArticle(string query, int limit)
    {
        SearchRequest searchRequest = new SearchRequest { query = query, limit = limit };
        string searchUrl = articleURL.searchURL;
        StartCoroutine(PostHttp<SearchRequest, SearchResponse>(searchRequest, searchUrl));
    }

    public void CreateArticle(string postId, int pageId, string content, int type, string imageData, int fontSize, string fontFace, bool isUnderlined, bool isStrikethrough, float x, float y, float z, float scale_x, float scale_y, float scale_z)
    {
        Scale scale = new Scale
        {
            x = scale_x,
            y = scale_y,
            z = scale_z
        };

        Position position = new Position
        {
            x = x,
            y = y,
            z = z
        };

        Elements element = new Elements
        {
            content = content,
            type = type,
            imageData = imageData,
            position = position,
            scale = scale,
            fontSize = fontSize,
            fontFace = fontFace,
            isUnderlined = isUnderlined,
            isStrikethrough = isStrikethrough
        };

        Pages page = new Pages
        {
            pageId = pageId,
            elements = new List<Elements> { element }
        };

        Posts post = new Posts
        {
            postId = postId,
            pages = new List<Pages> { page }
        };

        Article article = new Article
        {
            userid = firestore.GetUserInfo().userId,
            posts = new List<Posts> { post }
        };

        string createUrl = articleURL.createURL;
        StartCoroutine(PostHttp<Article, Article>(article, createUrl));
    }

    public void GetArticle(string articleid)
    {
        GetRequest getRequest = new GetRequest
        {
            userid = firestore.GetUserInfo().userId,
            articleid = articleid
        };

        string getUrl = articleURL.getURL;
        StartCoroutine(PostHttp<GetRequest, GetResponse>(getRequest, getUrl));
    }

    //LLM 호출 메서드
    public void LLM(string text)
    {
        string userId = firestore.GetUserInfo().userId;
        Debug.Log($"{userId}");
        string chatUrl = aiUrl.chatURL;

        ChatRequest chatRequest = new ChatRequest { userId = userId, text = text };

        StartCoroutine(PostHttp<ChatRequest, ChatResponse>(chatRequest, chatUrl));
    }

    public void TestTest(string testText)
    {
        ChatResponse response = JsonUtility.FromJson<ChatResponse>(testText);

        // AIChatMgr_KJS의 인스턴스를 찾아서 응답 텍스트를 업데이트
        AiChatMgr_KJS.Instance.UpdateChatResponse(response.text);
    }

    //Object 생성/호출 메서드
    public void GiveObject()
    {
        string giveUrl = aiUrl.giveObjectURL;
        StartCoroutine(GiveObjectHTTP(giveUrl));
    }

    //Cover 생성/호출 메서드
    public void LoadCover(string imgPath, string fileSavePath)
    {
        Files files = new Files
        {
            imgPath = imgPath
        };
        LoadCoverRequest loadCoverRequest = new LoadCoverRequest { filePath = new List<Files> { files } };
        string loadCoverUrl = aiUrl.loadCoverURL;
        StartCoroutine(PostHttpFile<LoadCoverRequest>(loadCoverRequest, loadCoverUrl, fileSavePath));
    }

    //Object 생성/호출 메서드
    public void Loadobject(string objPath, string pngPath, string fileSavePath)
    {
        Files files = new Files
        {
            imgPath = pngPath,
            objPath = objPath
        };
        LoadObjectRequest loadObjectRequest = new LoadObjectRequest { filePath = new List<Files> { files } };
        string loadObjectUrl = aiUrl.loadObjectURL;
        StartCoroutine(PostHttpFile<LoadObjectRequest>(loadObjectRequest, loadObjectUrl, fileSavePath));
    }

    //Trend 생성/호출 메서드 
    public void Trend()
    {
        string trendUrl = aiUrl.trendURL;
        StartCoroutine(PostHttpTrend(trendUrl));
    }

    //Ocr 생성/호출 메서드
    public void Ocr(string screenShot)
    {
        string ocrUrl = aiUrl.ocrURL;
        StartCoroutine(PostHttpFile2(ocrUrl, screenShot));
    }

    //Http Json Post
    public IEnumerator PostHttp<TRequest, TResponse>(TRequest requestObject, string url)
    {
        string json = JsonUtility.ToJson(requestObject);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                TResponse responseObject = JsonUtility.FromJson<TResponse>(www.downloadHandler.text);
                Debug.Log("success");
                Debug.Log("Response: " + JsonUtility.ToJson(responseObject));

                if( requestObject is ChatRequest)
                {
                    //typeof(ChatResponse)

                    TestTest(JsonUtility.ToJson(responseObject));
                }
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}, Response: {www.downloadHandler.text}");
            }

        }
    }

    public IEnumerator AuthHttp(string url)
    {
        AuthRequest auth = new AuthRequest
        {
            userid = firestore.GetUserInfo().userId,
            username = firestore.GetUserInfo().nickName
        };
        string json = JsonUtility.ToJson(auth);
        Debug.Log("JSON Payload: " + json);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            Debug.Log($"Response Code: {www.responseCode}, Response Text: {www.downloadHandler.text}");

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Request Success");
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}, Response: {www.downloadHandler.text}");
            }
        }
    }

    //Http File Post
    public IEnumerator PostHttpFile<TRequest>(TRequest requestObject, string url, string fileSavePath)
    {
        string json = JsonUtility.ToJson(requestObject);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerFile(fileSavePath);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"File downloaded successfully and saved at: {fileSavePath}");
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}");
            }
        }
    }

    public IEnumerator PostHttpFile2(string url, string screenShot)
    {
        OcrRequest ocrRequest = new OcrRequest { screenShot = screenShot };
        string json = JsonUtility.ToJson(ocrRequest);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {

            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}");
            }
        }
    }

    public IEnumerator GiveObjectHTTP(string url)
    {
        LoadObjectRequest loadObjectRequest = new LoadObjectRequest();

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            string jsonData = JsonUtility.ToJson(loadObjectRequest);
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            www.uploadHandler.contentType = "application/json";

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    LoadObjectRequest responseObject = JsonUtility.FromJson<LoadObjectRequest>(www.downloadHandler.text);
                    Debug.Log("Success");
                    Debug.Log("Response: " + JsonUtility.ToJson(responseObject));
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to parse JSON: " + e.Message);
                }
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}");
            }
        }
    }

    public IEnumerator PostHttpTrend(string url)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = new DownloadHandlerTexture();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success");
            trendDownloadTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

        }
        else
        {
            Debug.LogError("Failed to download image: " + request.error);
        }
    }

    public Texture2D coverDownloadTexture;
    public Texture2D trendDownloadTexture;


    AlphaURL alphaURL = new AlphaURL();
    //알파 시연 메서드
    //첫 번째 썸네일 이미지 호출 코드
    //두 번째 트렌드 이미지 호출 코드
    public void Cover()
    {
        string coverURL = alphaURL.coverURL;
        StartCoroutine(CoverDownloadImage(coverURL));
    }
    IEnumerator CoverDownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            coverDownloadTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        else
        {
            Debug.LogError("Failed to download image: " + request.error);
        }
    }
    public Texture2D CoverGetDownloadedImage()
    {
        return coverDownloadTexture;
    }

    public Texture2D TrendGetDownloadedImage()
    {
        return trendDownloadTexture;
    }

}