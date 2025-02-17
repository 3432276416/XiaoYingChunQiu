using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{

   
    AssetReference curScene;
    public AssetReference Persist;
    public AssetReference GameBoard;
    public AssetReference GameMenu;
    public AssetReference Dialog; //选择卡牌的界面
    public AssetReference Map; //选择好汉的界面
    private FadePanel fadePanel;

    public Vector2Int curRoomVector;



    public void OnLoadRoomEvent(object data)
    {

        StartCoroutine(UnloadSceneEvent());   //加载房间

        //StartCoroutine(LoadSceneEventWithEventExecute(AfterLoadRoomEvent, curRoom, this));

    }

    private void Awake()
    {
        
        LoadGameBoard();
    }

    public void LoadGameBoard()
    {
        if (curScene != null)
            StartCoroutine(UnloadSceneEvent());

        curScene = GameBoard;
        StartCoroutine(LoadSceneEvent());
    }

    public void LoadMap()
    {
        if (curScene != null)
            StartCoroutine(UnloadSceneEvent());

        curScene = Map;
        StartCoroutine(LoadSceneEvent());
    }

    public void LoadDialog(object story)
    {
        if (curScene != null)
            StartCoroutine(UnloadSceneEvent());

        curScene = Map;
        StartCoroutine(LoadSceneEvent());
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

    IEnumerator LoadSceneEventWithEventExecute(ObjectEventSO eventSO, object val, object sender)
    {
        yield return StartCoroutine(LoadSceneEvent());
        eventSO.RaiseEvent(val, sender);
    }

    public IEnumerator UnloadSceneEvent()  //退出场景
    {
        fadePanel.FadeIn(0.4f);
        yield return new WaitForSeconds(0.45f);
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

   

}
