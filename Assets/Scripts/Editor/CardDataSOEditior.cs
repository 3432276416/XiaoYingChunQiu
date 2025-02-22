using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardDataSO))]
public class CardDataSOEditior : Editor {

    CardDataSO cardDataSO;

    private void OnEnable() {
        if (cardDataSO == null)
        {
            cardDataSO = target as CardDataSO;
        }
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
        EditorGUILayout.LabelField("没有添加CSV文件则代表此卡片需要自定义生成效果并手动挂载");

        if (GUILayout.Button("刷新"))
        {
            //cardDataSO.SpawnCard();
        }
    }
}