using System;
using System.Collections.Generic;

/// <summary>
/// ���ڴ����¼�����չ��
/// </summary>
public static class EventTriggerExt
{
    /// <summary>
    /// �����¼����޲�����
    /// </summary>
    /// <param name="sender">����Դ</param>
    /// <param name="eventName">�¼���</param>
    public static void TriggerEvent(this object sender, string eventName)
    {
        EventManager.Instance.RaiseEvent(eventName, sender);
    }
    /// <summary>
    /// �����¼����в�����
    /// </summary>
    /// <param name="sender">����Դ</param>
    /// <param name="eventName">�¼���</param>
    /// <param name="args">�¼�����</param>
    public static void RaiseEvent(this object sender, string eventName, EventArgs args)
    {
        EventManager.Instance.RaiseEvent(eventName, sender, args);
    }

}

/// <summary>
/// �¼�������
/// </summary>
public class EventManager : SingletonBase<EventManager>
{
    private Dictionary<string, EventHandler> handlerDic = new Dictionary<string, EventHandler>();
    /// <summary>
    /// ����һ���¼��ļ�����
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="handler">�¼���������</param>
    public void AddListener(string eventName, EventHandler handler)
    {
        if (handlerDic.ContainsKey(eventName))
            handlerDic[eventName] += handler;
        else
            handlerDic.Add(eventName, handler);
    }
    /// <summary>
    /// �Ƴ�һ���¼��ļ�����
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="handler">�¼���������</param>
    public void RemoveListener(string eventName, EventHandler handler)
    {
        if (handlerDic.ContainsKey(eventName))
            handlerDic[eventName] -= handler;
    }
    /// <summary>
    /// �����¼����޲�����
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="sender">����Դ</param>
    public void RaiseEvent(string eventName, object sender)
    {
        if (handlerDic.ContainsKey(eventName))
            handlerDic[eventName]?.Invoke(sender, EventArgs.Empty);
    }
    /// <summary>
    /// �����¼����в�����
    /// </summary>
    /// <param name="eventName">�¼���</param>
    /// <param name="sender">����Դ</param>
    /// <param name="args">�¼�����</param>
    public void RaiseEvent(string eventName, object sender, EventArgs args)
    {
        if (handlerDic.ContainsKey(eventName))
            handlerDic[eventName]?.Invoke(sender, args);
    }
    /// <summary>
    /// ��������¼�
    /// </summary>
    public void Clear()
    {
        handlerDic.Clear();
    }
}

