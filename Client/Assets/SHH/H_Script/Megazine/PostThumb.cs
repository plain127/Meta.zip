using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PostThumb : MonoBehaviour
{
    public H_PostInfo postinfo;
    public Image image;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetInfo(H_PostInfo info)
    {
        postinfo = info;

        HttpInfo htpinfo = new HttpInfo();
        htpinfo.url = info.thumburl;
        htpinfo.onComplete = OncompleteDownload;

        StartCoroutine(HttpManager.GetInstance().DownloadSprite(htpinfo));
    }
    public void OncompleteDownload(DownloadHandler downloadhandler)
    {
        DownloadHandlerTexture handlerTexture = downloadhandler as DownloadHandlerTexture;
        Sprite sprite = Sprite.Create(handlerTexture.texture,new Rect(0,0, handlerTexture.texture.width, handlerTexture.texture.height),Vector2.zero);

        
        image.sprite = sprite;
    }
}
