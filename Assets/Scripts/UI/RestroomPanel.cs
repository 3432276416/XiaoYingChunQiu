using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RestroomPanel : MonoBehaviour
{
    VisualElement rootEle;
    Button btn;
    public Effect restEff;
    Player player;
    public ObjectEventSO loadMapEvent;
    private void OnEnable() {
        rootEle = GetComponent<UIDocument>().rootVisualElement;
        btn = rootEle.Q<Button>("RestBtn");
        //TODO:休息室逻辑重写
        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);

        btn.clicked += OnBtnClick;
    }

    private void OnBtnClick()
    {
        //restEff.Execute(null,player);
        loadMapEvent.RaiseEvent(null,this);
    }
}
