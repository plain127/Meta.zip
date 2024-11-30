using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllButtonMgr : MonoBehaviour
{
    // Channel -> ScrapBook
    public void ChannelToScrapBook()
    {
        MetaConnectionMgr.instance.JoinOrCreateRoom();
    }

    // Channel -> Folder
    public void ChannelToFolder()
    {
        MetaConnectionMgr.instance.JoinFolder();
    }

    // ScrapBook -> Channel
    public void ScrapBookToChannel()
    {
        MetaConnectionMgr.instance.ScrapBookToChannel();
    }

    // ScrapBook -> Folder
    public void ScrapBookToFolder()
    {
        MetaConnectionMgr.instance.ScrapBookToFolder();
    }

    // Town -> Channel
    public void TownToChannel()
    {
        MetaConnectionMgr.instance.TownToChannel();
    }

    // Town -> ScrapBook
    public void TownToScrapBook()
    {
        MetaConnectionMgr.instance.TownToScrapBook();
    }

    // Town -> Folder
    public void TownToFolder()
    {
        MetaConnectionMgr.instance.TownToFolder();
    }

    // Town -> Magazine
    public void TownToMagazine()
    {
        MetaConnectionMgr.instance.TownToMagazine();
    }

    // 게임 종료
    public void QuitGame()
    {
        Debug.Log("게임 종료 버튼이 눌렸습니다.");

        // Unity 에디터에서 실행 중일 경우
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 게임에서는 게임 종료
        Application.Quit();
#endif
    }
}