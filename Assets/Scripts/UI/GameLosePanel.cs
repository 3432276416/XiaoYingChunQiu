using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameLosePanel : MonoBehaviour
{
    Button backToStartMenuBtn;
    public ObjectEventSO loadMenuEvent;
    private void OnEnable() {
        backToStartMenuBtn = GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartBtn");
        backToStartMenuBtn.clicked += OnBackToStartMenuBtnClicked;
    }

    private void OnBackToStartMenuBtnClicked()
    {
        loadMenuEvent.RaiseEvent(null,this);
    }
}
