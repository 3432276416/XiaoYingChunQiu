using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public Player player;
    public MapLayoutSO mapLayout;
    public List<CharacterBase> aliveEnmList;
    public List<Hero> aliveHeroList;
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameLoseEvent;

    public UIMgr uiMgr;
    
    private void OnEnable() {
        var heros = player.GetComponentsInChildren<Hero>();
        foreach (var item in heros)
        {
            aliveHeroList.Add(item);
        }
    }
    /// <summary>
    /// 更新游戏地图状态
    /// </summary>
    /// <param name="value"></param>
    public void UpdateMapLayoutData(object value)
    {
        Vector2Int roomVector = (Vector2Int)value;
        if (mapLayout.roomMapDataList.Count == 0)
        {
            return;
        }

        var curRoom = mapLayout.roomMapDataList.Find(x => x.col == roomVector.x && x.row == roomVector.y);
        curRoom.state = RoomState.Visited;

        var sameColRoom = mapLayout.roomMapDataList.FindAll(x => x.col == curRoom.col);
        foreach (var item in sameColRoom)
        {
            if (item.row != roomVector.y)
            {
                item.state = RoomState.Locked;
            }
        }

        foreach (var item in curRoom.LinkTo)
        {
            var linkRoom = mapLayout.roomMapDataList.Find(r => r.col == item.x && r.row == item.y);
            linkRoom.state = RoomState.Attainable;
        }

        aliveEnmList.Clear();
    }

    public void OnCharacterDeadEvent(object obj)
    {
        if (obj is Hero)
        {
            aliveHeroList.Remove(obj as Hero);
            if (aliveHeroList.Count == 0)
            {
                StartCoroutine(EventDelayAction(gameLoseEvent));
            }
        }
        else if (obj is Boss)
        {
            StartCoroutine(EventDelayAction(gameLoseEvent));
        }
        else if (obj is EnmSoldier)
        {
            aliveEnmList.Remove(obj as EnmSoldier);

            if (aliveEnmList.Count == 0)
            {
                Debug.Log("没人活了");
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }
        else if (obj is Player)
        {
            StartCoroutine(EventDelayAction(gameLoseEvent));
        }
    }
    /// <summary>
    /// 在房间加载后获取敌人，添加到`aliveEnmList`中
    /// </summary>
    /// <param name="obj"></param>
    [ContextMenu("test")]
    public void OnLoadRoomEvent()
    {
        StartCoroutine(OnLoadRoomEventIEnumerator());
    }

    IEnumerator EventDelayAction(ObjectEventSO so)
    {
        yield return new WaitForSeconds(1.5f);

        so.RaiseEvent(null, this);
    }

    IEnumerator OnLoadRoomEventIEnumerator()
    {
        yield return new WaitForSeconds(0.5f);
        var enms = FindObjectsByType<EnmSoldier>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var item in enms)
        {
            aliveEnmList.Add(item);
        }
    }

    public void OnNewGameEvent()
    {
        mapLayout.roomMapDataList.Clear();
        mapLayout.LinePosList.Clear();
    }
}
