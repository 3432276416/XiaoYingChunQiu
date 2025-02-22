using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameWinPanel : MonoBehaviour
{
    VisualElement rootElement;
    Button ChooseCardBtn;
    Button BackToMapBtn;
    [Header("广播")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO chooseCardEvent;
    private void OnEnable() {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        ChooseCardBtn = rootElement.Q<Button>("ChooseCardBtn");
        BackToMapBtn = rootElement.Q<Button>("BackToMapBtn");

        BackToMapBtn.clicked += OnBackToMapBtnClicked;
        ChooseCardBtn.clicked += OnChooseCardBtnClicked;
    }

    private void OnBackToMapBtnClicked()
    {
        loadMapEvent.RaiseEvent(null,this);
    }

    public void OnChooseCardBtnClicked()
    {
        //Debug.Log(this + "broadcast");
        chooseCardEvent.RaiseEvent(null,this);
    }
}
