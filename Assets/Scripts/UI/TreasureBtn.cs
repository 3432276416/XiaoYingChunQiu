using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreasureBtn : MonoBehaviour,IPointerDownHandler
{
    public ObjectEventSO chooseCardEvent;
    public void OnPointerDown(PointerEventData eventData)
    {
        chooseCardEvent.RaiseEvent(null,this);
    }

}
