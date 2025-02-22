using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    VisualElement rootEle;
    Button newGameBtn,quitBtn;


    public ObjectEventSO LoadHeroMenuEvent;
    public ObjectEventSO LoadCardMenuEvent;
    public ObjectEventSO newGameEvent;

    private void OnEnable() {
        rootEle = GetComponent<UIDocument>().rootVisualElement;
        newGameBtn = rootEle.Q<Button>("NewGameBtn");
        quitBtn = rootEle.Q<Button>("QuitBtn");

        newGameBtn.clicked += OnNewGameBtnClicked;
        quitBtn.clicked += OnQuitBtnClicked;
    }

    private void OnQuitBtnClicked() => Application.Quit();


    private void OnNewGameBtnClicked()
    {
        //newGameEvent.RaiseEvent(null,this);
        LoadHeroMenuEvent.RaiseEvent(this, this);
    }
}
