using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IntEventSO))]
public class IntEventSOEditor : BaseEventSOEditor<int>{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
