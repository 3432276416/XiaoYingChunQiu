using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "Variable/IntVariable")]
public class IntVariable : ScriptableObject {
    public int maxValue;
    public int curValue;
    public IntEventSO ValChangeEvent;
    [TextArea]
    [SerializeField]string description;

    /// <summary>
    /// 通用的方法，用于更新数值
    /// </summary>
    public void SetValue(int val)
    {
        curValue = val;
        ValChangeEvent?.RaiseEvent(val,this);
    }
}