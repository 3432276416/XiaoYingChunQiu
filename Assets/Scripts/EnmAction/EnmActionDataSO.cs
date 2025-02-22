using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnmActionDataSO", menuName = "Enm/EnmActionDataSO")]
public class EnmActionDataSO : ScriptableObject {
    public List<EnmAction> Actions;
}
[System.Serializable]
public struct EnmAction
{
    public Effect effect;
    public Sprite intent;
}