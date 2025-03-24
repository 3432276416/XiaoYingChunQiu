using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuPanel : MonoBehaviour
{
    VisualElement rootEle;
    Button newGameBtn,quitBtn,bestiaryBtn,optionBtn;

    public GameObject BestiaryPanel;
    //public GameObject OptionPanel;

    private void OnEnable() {
        rootEle = GetComponent<UIDocument>().rootVisualElement;
        //OptionPanel = FindObjectOfType<SettingPanel>(true).gameObject;
        newGameBtn = rootEle.Q<Button>("NewGameBtn");
        quitBtn = rootEle.Q<Button>("QuitBtn");
        bestiaryBtn = rootEle.Q<Button>("BestiaryBtn");
        //optionBtn = rootEle.Q<Button>("OptionBtn");

        newGameBtn.clicked += OnNewGameBtnClicked;
        quitBtn.clicked += OnQuitBtnClicked;
        //optionBtn.clicked += () => OptionPanel.SetActive(!OptionPanel.activeInHierarchy);
    }

    private void OnBestiaryBtnClicked()
    {
        BestiaryPanel.SetActive(true);
    }

    private void OnQuitBtnClicked() => Application.Quit();


    private void OnNewGameBtnClicked()
    {
        newGameBtn.SetEnabled(false);
        EventManager.Instance.RaiseEvent(EventName.LoadLaserLevel1,this);
    }

}
