using System;

[Flags]
public enum RoomType
{
    MinorEnm = 1,
    EliteEnm = 2,
    Shop = 4,
    Treasure = 8,
    Restroom = 16,
    Boss = 32
}
#region buff

public enum BuffType
{
    Strength = 0,
    Weak
}
public class Buff
{
    public BuffType type;
    public int buffRound;
}
#endregion

public enum RoomState
{
    Locked,
    Visited,
    Attainable
}

public enum CardType
{
    Hero, //好汉
    Soldier,//兵牌
    Item //道具牌
}


/// <summary>
/// 元素
/// </summary>
public enum Elem
{
    Wind, //风
    Forest, //林
    Fire,    //火
    Mountain, //山
    Dark,     //暗
    Thunder,   //雷
    None //无
}
public enum EffectTargetType
{

    SingleHero, //对单个英雄
    MultiHero, //对多个英雄
    Player, //对玩家
    Enm, //对敌人
    All //对玩家和敌人

}
public enum EnmSoldierType
{
    man,//官兵
    ghost//鬼
}

/// <summary>
/// OnStart开局,OnDrawCard抽卡,OnUseCard使用卡,OnHeroDie好汉死亡
/// </summary>
public enum HeatTriggerType
{
    OnStart,OnDrawCard,OnUseCard,OnHeroDie,OnRoundStart,OnRoundEnd
}

public enum SoldierType
{
    Vanguard,//冲锋
    Backbone,//中坚
    Logistics//后勤
}