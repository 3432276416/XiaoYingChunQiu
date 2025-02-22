using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{

    public CardMgr cardMgr;
    public FadePanel fadePanel;
    AssetReference curScene;
    public AssetReference map;
    public AssetReference menu;
    public AssetReference intro;
    public AssetReference cardMenu; //选择卡牌的界面
    public AssetReference heroMenu; //选择好汉的界面
    Room curRoom;
    public Vector2Int curRoomVector;
    [Header("广播")]
    public ObjectEventSO AfterLoadRoomEvent;
    public ObjectEventSO updateRoomEvent;

    public void OnLoadRoomEvent(object data)
    {
        if (data is Room)
        {
            curRoom = data as Room;
            var curData = curRoom.data;

            curRoomVector = new(curRoom.col, curRoom.row);
            curScene = curData.sceneToLoad;
        }

        StartCoroutine(UnloadSceneEvent());
        //加载房间
        StartCoroutine(LoadSceneEventWithEventExecute(AfterLoadRoomEvent,curRoom,this));
        
    } 

    private void Awake() {
        curRoomVector = Vector2Int.one * -1;

        //LoadCutScene();
        LoadMenu();
    }
    IEnumerator LoadSceneEvent()
    {
        yield return new WaitForSeconds(0.45f);
        var s = curScene.LoadSceneAsync(LoadSceneMode.Additive);
        yield return s;

        fadePanel.FadeOut(0.2f);
        yield return new WaitForSeconds(0.25f);
        SceneManager.SetActiveScene(s.Result.Scene);
    }

    IEnumerator LoadSceneEventWithEventExecute(ObjectEventSO eventSO,object val,object sender)
    {
        yield return StartCoroutine(LoadSceneEvent());
        eventSO.RaiseEvent(val,sender);
    }

    public IEnumerator UnloadSceneEvent()
    {
        fadePanel.FadeIn(0.4f);
        yield return new WaitForSeconds(0.45f);
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    /// <summary>
    /// 加载地图，监听返回地图的事件函数
    /// </summary>
    /// <returns></returns>
    public void LoadMap()
    {
        StartCoroutine(UnloadSceneEvent());
        if (curRoomVector != Vector2.one*-1)
        {
            updateRoomEvent.RaiseEvent(curRoomVector,this);
        }
        curScene = map;
        StartCoroutine(LoadSceneEvent());
    }

    public void LoadCardMenu()
    {
        if (curScene != null)
            StartCoroutine(UnloadSceneEvent());
        curScene = cardMenu; 
        StartCoroutine(LoadSceneEvent());
    }

    public void LoadHeroMenu()
    {
        if (curScene != null)
            StartCoroutine(UnloadSceneEvent());
        curScene = heroMenu;
        StartCoroutine(LoadSceneEvent());
    }

    public void LoadMenu()
    {
        if (curScene!=null)
            StartCoroutine(UnloadSceneEvent());

        curScene = menu;
        StartCoroutine(LoadSceneEvent());
    }

/// <summary>
/// 加载一开始的场景
/// </summary>
    public void LoadCutScene()
    {
        if (curScene!=null)
            StartCoroutine(UnloadSceneEvent());

        curScene = intro;
        StartCoroutine(LoadSceneEvent());
    }
}
