using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LaserLevelMgr : MonoBehaviour
{
    public ObjectEventSO SuccessDialogEvent;
    public ObjectEventSO TeachDialogEvent;
    public ObjectEventSO LoadDialogEvent;

    private void Start()
    {
        LoadDialogEvent.RaiseEvent(this, this);
    }

   
}
