using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public Player player;
    public GameObject gamePlayPanel;
    public GameObject gameWinPanel;
    public GameObject gameLosePanel;
    public GameObject ChooseCardPanel;
  
/// <summary>
/// 加载房间时调用，监听端
/// </summary>
/// <param name="obj"></param>
    public void OnLoadRoomEvent(object obj)
    {
        Room curRoom = (Room)obj;

        switch (curRoom.data.type)
        {
            case RoomType.MinorEnm:
            case RoomType.EliteEnm:
            case RoomType.Boss:
                gamePlayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.Restroom:
                break;
            default:
                break;
        }
    }
/// <summary>
/// 加载地图/菜单时调用
/// </summary>
    public void HideAllPanels()
    {
        gameLosePanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        ChooseCardPanel.SetActive(false);
    }
/// <summary>
/// 游戏胜利时调用
/// </summary>
    public void OnGameWinEvent()
    {
        gameWinPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }
/// <summary>
/// 游戏失败时调用
/// </summary>
    public void OnGameLoseEvent()
    {
        gamePlayPanel.SetActive(false);
        gameLosePanel.SetActive(true);
    }

    ///// <summary>
    ///// 抽卡面板唤起
    ///// </summary>
    ////    public void OnChooseCardEvent(object cardEff)
    ////    {
    ////        //if (cardEff is Effect)
    ////        //{
    ////        //    //Debug.Log(this + "listen");
    ////        //    //gameWinPanel.SetActive(false);
    ////        //    ChooseCardPanel.SetActive(true);
    ////        //    ChooseCardPanel.GetComponent<ChooseCardPanel>().curCardEffect = cardEff as Effect;
    ////        //}

    ////    }
    ///

    /// <summary>
    /// 选择菜单
    /// </summary>
    /// <param name="obj"></param>
    public void OnChooseCard(object obj)
    {
        Effect curRoom = (Effect) obj ;

        ChooseCardPanel.SetActive(true);

    }
}
