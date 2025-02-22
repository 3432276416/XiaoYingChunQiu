using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hero : CharacterBase
{
    public HeroDataSO heroData;
    public HeroInfoDisplayController displayController;
    public List<Solider> ownSoliders; //С??
    public bool OnStage; //????????
    public Player player;
    public void Init(HeroDataSO heroData)
    {
        this.heroData = heroData;
        curHp = maxHp = heroData.HP;
        Attack = heroData.Attack;
        displayController.Init(heroData);
    }
    /// <summary>
    /// С????buff
    /// </summary>
    /// <param name="sd"></param>
    public void AddSolider(Solider sd)
    {
        this.ownSoliders.Add(sd);
        this.curHp += sd.curHp;
        this.Attack += sd.Attack;
        this.maxHp += sd.maxHp;
    }
    /// </summary>
    /// ????ú??????
    /// </summary>
    public void HealHero(int val)
    {
        int res = Mathf.Min(curHp + val, maxHp);
        curHp = res;
    }
    /// <summary>
    /// ???????кú???????
    /// </summary>
    public void StrengthHero(int val)
    {
        Attack += val;
    }

    public void NewGame()
    {
        curHp = maxHp;
        isDead = false;
        buffs.Clear();
    }

    #region 热度技能触发
    /// <summary>
    /// 被动触发
    /// </summary>
    public void PassiveExecute(HeatTriggerType type)
    {
        foreach (var item in heroData.passiveEffects)
        {
            Debug.Log(item.triggerType + "==" + type + "?" + type.Equals(item.triggerType));
            if (type == item.triggerType)
            {
                item.Execute(this, player);
            }
        }
    }
    #endregion
}
