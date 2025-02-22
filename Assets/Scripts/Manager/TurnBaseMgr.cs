using System.Collections;
using System.Collections.Generic;
using NLog;
using UnityEngine;

public class TurnBaseMgr : MonoBehaviour
{
    public static NLog.Logger logger = LogManager.GetCurrentClassLogger();
    public IntVariable RoundCount;
    public GameObject player;
    public Enm enm;
    public bool isPlayerTurn = false;
    public bool isEnmTurn = false;
    public bool battleEnd = true;
    float timeCounter;
    [SerializeField] float enmTurnDuration;
    public float playerTurnDuration;
    public int round { get => RoundCount.curValue; set => RoundCount.SetValue(value); }  //回合数
    [Header("广播")]
    public ObjectEventSO OnPlayerTurnBeginEvent;
    public ObjectEventSO OnEnmTurnBeginEvent;
    public ObjectEventSO OnEnmTurnEndEvent;
    public ObjectEventSO OnPlayerTurnEndEvent;
    public ObjectEventSO OnChooseCardEvent;
    public IntEventSO OnRoundEnd;
    private void Update()
    {
        if (battleEnd) return;
        if (isEnmTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= enmTurnDuration)
            {
                timeCounter = 0f;
                EnmTurnEnd();
                isPlayerTurn = true;
            }
        }
        if (isPlayerTurn)
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= playerTurnDuration)
            {
                timeCounter = 0f;
                if (round == 0) player.GetComponent<Player>().GameStart();
                round++;
                PlayerTurnBegin();
                isPlayerTurn = false;
            }
        }
    }

    public void EnmTurnBegin()
    {
        isEnmTurn = true;
        OnEnmTurnBeginEvent.RaiseEvent(null, this);
        enmTurnDuration = enm.enms.Count * 1.25f;
    }
    void EnmTurnEnd()
    {
        isEnmTurn = false;
        OnEnmTurnEndEvent.RaiseEvent(null, this);

        logger.Info("test end");
        LogManager.Shutdown();
    }
    void PlayerTurnBegin()
    {

        OnRoundEnd.RaiseEvent(round, this);
        OnPlayerTurnBeginEvent.RaiseEvent(null, this);

    }

    [ContextMenu("GameStart")]
    public void GameStart()
    {
        isPlayerTurn = true;
        isEnmTurn = false;
        battleEnd = false;
        timeCounter = 0f;
        round = 0;
        enm = FindObjectOfType<Enm>().GetComponent<Enm>();
        enm.speed = 0;

        logger.Info("test start");
    }
    /// <summary>
    /// 房间加载后的事件
    /// </summary>
    public void OnLoadRoomEvent(object obj)
    {
        Room room = obj as Room;
        switch (room.data.type)
        {
            case RoomType.MinorEnm:
            case RoomType.EliteEnm:
            case RoomType.Boss:
                player.SetActive(true);
                GameStart();
                break;
            case RoomType.Shop:
            case RoomType.Treasure:
                player.SetActive(false);
                break;
            case RoomType.Restroom:
                player.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 游戏结束的事件函数
    /// </summary>
    public void StopTurnBase()
    {
        battleEnd = true;
        player.SetActive(false);

        enm = null;
    }


    public void NewGame()
    {
        player.GetComponent<Player>().NewGame();
        //OnChooseCardEvent.RaiseEvent(this, this);
    }
}
