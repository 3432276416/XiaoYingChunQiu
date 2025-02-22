using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite sprite;
    public int cost;
    [TextArea]
    public string description;
    public CardType type;
    public Elem elem;
    //执行卡牌效果
    public List<Effect> cardEffs;

    [Header("易于使用的卡牌编辑相关")]
    public TextAsset cardCSVFile;
}

    //public void SpawnCard()
    //{
    //    if (!cardCSVFile) return;

    //    //清空CardDataSO的效果,图片自己导入赋值，名字什么的都可以自己写

    //    cardEffs.Clear();

    //    //获取数据，分割成数组
    //    string[] textInLines = cardCSVFile.text.Split('\n');

    //    for (int i = 1; i < textInLines.Length-1; i++)
    //    {  
    //        var value = textInLines[i].Split(',');

    //        //获取数据
    //        Effect eff = value[0] switch 
    //        {
    //            "damage" => CreateInstance<DamageEffect>(),
    //            "heal" => CreateInstance<HealthEffect>(),
    //            "defence" => CreateInstance<DefenceEffect>(),
    //            "strength" => CreateInstance<StrengthEffect>(),
    //            "weak" => CreateInstance<StrengthEffect>(),
    //            _ => throw new NotImplementedException($"生成第{i}个卡牌效果时类型({value[0]})获取失败,可选类型为damage,heal,defence,strength,weak"),
    //        };

    //        eff.value = int.Parse(value[1]);
            
    //        //最后一个数据有一个占位符号要去掉否则无法识别
    //        eff.tarType = value[2].Remove(value[2].Length-1) switch
    //        {
    //            "self" => EffectTargetType.self,
    //            "target" => EffectTargetType.target,
    //            "all" => EffectTargetType.ALL,
    //            _ => throw new NotImplementedException($@"生成第{i}个卡牌效果时卡牌目标类型({value[2]})获取失败,可选类型为self,target,all"),
    //        };


    //        //文件夹不存在就开新文件夹
    //        var newFolderName = $"{cardName}_Effect";
    //        if (!System.IO.Directory.Exists(@"Assets/CardEffect/"+newFolderName))
    //            AssetDatabase.CreateFolder(@"Assets/CardEffect", newFolderName);
            
    //        AssetDatabase.CreateAsset(eff, @"Assets/CardEffect/"+newFolderName+"/"+$"{cardName}_Effect_{i}"+".asset");
    //        AssetDatabase.SaveAssets(); //存储资源
    //        AssetDatabase.Refresh(); //刷新

    //        cardEffs.Add(eff);
    //    }
    //}
//}