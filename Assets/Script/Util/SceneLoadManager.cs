using System.Collections;
using UnityEditor.Rendering.LookDev;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using System;

public class SceneLoadManager : MonoBehaviour
{

   
    AssetReference curScene;
    public AssetReference Persist;
    public AssetReference GameBoard;
    public AssetReference GameMenu;
    public AssetReference Dialog; //对话界面
    public AssetReference Map; //地图界面
    public AssetReference LaberLevel;

    public FadePanel fadePanel;

    public Vector2Int curRoomVector;



    public void OnLoadRoomEvent(object data)
    {

        StartCoroutine(UnloadSceneEvent());   //加载房间

        //StartCoroutine(LoadSceneEventWithEventExecute(AfterLoadRoomEvent, curRoom, this));

    }

    private void Awake()
    {
        EventManager.Instance.AddListener(EventName.LoadDialog, LoadDialog);  
    }

    public void Start()
    {
        //EventManager.Instance.RaiseEvent(EventName.LoadDialog,this);
    }

    public void LoadGameBoard(object sender, EventArgs e)
    {
        if (curScene != null)
            StartCoroutine(UnloadSceneEvent());

        curScene = GameBoard;
        StartCoroutine(LoadSceneEvent());
    }

    public void LoadMap(object sender, EventArgs e)
    {
        if (curScene != null)
            StartCoroutine(UnloadSceneEvent());

         curScene = Map;
         StartCoroutine(LoadSceneEvent());

    }

    public void LoadDialog(object sender, EventArgs e)
    {
        if (curScene != null)
            StartCoroutine(UnloadSceneEvent());

        curScene = Dialog;
         StartCoroutine(LoadSceneEvent());
        if(e is StoryArgs)
        {
            StoryArgs args = (StoryArgs)e;
           LoadDialogStory(args.StoryEventName);
        }
       

    }

    private IEnumerable LoadDialogStory(string StoryEventName)
    {
    
        yield return new WaitUntil(() => SceneManager.GetSceneByName(SceneName.Dialog)!=null);  //等待对话场景加载完毕
        Debug.Log("加载对话场景完毕，加载故事内容");
        EventManager.Instance.RaiseEvent(StoryEventName, null);

    }

    public void CloseDialog()
    {
        SceneManager.UnloadSceneAsync("Dialog");
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
