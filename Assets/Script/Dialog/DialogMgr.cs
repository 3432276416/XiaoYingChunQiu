
using System;
using UnityEngine;

public class DialogMgr : MonoBehaviour
{

    public void Awake()
    {
        EventManager.Instance.AddListener(EventName.TeachLaserLevel, TeachLaserLevel);
    }

    private void TeachLaserLevel(object oj,EventArgs args)
    {

        //EventManager.Instance.RaiseEvent(EventName.LoadDialog, this);

        //EventManager.Instance.RaiseEvent(EventName.TeachLaserLevel, this);
    }

}
