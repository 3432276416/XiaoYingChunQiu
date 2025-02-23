using UnityEngine;

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
