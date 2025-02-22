using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroDataSO", menuName = "Card/HeroDataSO")]
public class HeroDataSO : ScriptableObject
{
    public Sprite heroSprite; //英雄图片
    public string Name; //名字
    public string Title;  //称号
    public string passiveSkillName;
    public string unlockSkillName;
    public string costSkillName;
    public Elem elem; //元素
    public int Point; //点数
    public int HP;
    public int Attack; //攻击
    public List<HeatEffect> passiveEffects; //被动
    public List<HeatEffect> unlockEffects; //热度解锁技能
    public List<HeatEffect> costEffects; //热度消耗技能

}
