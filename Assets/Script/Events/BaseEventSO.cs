using UnityEngine;
using UnityEngine.Events;

public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;
    [SerializeField]public UnityAction<T> OnEventRaised;
    public string lastSender;

    /// <summary>
    /// 广播事件
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="sender">呼叫的事件</param>
    public void RaiseEvent(T value, object sender)
    {
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }
}