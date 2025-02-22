using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor {
    BaseEventSO<T> baseEventSO;
    private void OnEnable() {
        if (baseEventSO == null)
        {
            baseEventSO = target as BaseEventSO<T>;
        }
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        EditorGUILayout.LabelField("订阅数量："+ GetListener().Count);
        foreach (var listener in GetListener())
        {
            EditorGUILayout.LabelField(listener.ToString()); //获取监听名字
        }
    }

    List<MonoBehaviour> GetListener()
    {
        List<MonoBehaviour> listeners = new();

        if (baseEventSO == null || baseEventSO.OnEventRaised == null)
            return listeners;

        var subscribers = baseEventSO.OnEventRaised.GetInvocationList();
        foreach (var subscriber in subscribers) {
            var obj = subscriber.Target as MonoBehaviour;
            if (!listeners.Contains(obj))
            {
                listeners.Add(obj);
            }
        }
        return listeners;
    }
}