using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Player是控制我方全局（热度相关）的脚本，PlayerCard才是控制牌那些的，
/// 之后可以通过继承PlayerCard来修改各个不同属性的卡
/// </summary>
public class Player : MonoBehaviour
{
    public IntVariable PlayerHeat;//玩家热度
    public IntVariable ActPoints; //玩家行动点数
    public IntVariable PlayerSpeed;//玩家速度
    public int Yue;  //乐
    public List<Hero> tolHeroList = new List<Hero>();
    public List<Hero> stageHeroList = new(); //上场好汉列表
    public List<Hero> supportHeroList = new();//辅助好汉列表

    public HeroFormationConfigSO heroFormationConfig;//好汉组队数据
    public TurnBaseMgr turnBaseMgr; //回合管理器
    public List<Buff> buffs; //持续buff
    public int maxPoints; //最大行动点
    public int curPoints { get => ActPoints.curValue; set => ActPoints.SetValue(value); }
    public int maxHeat;
    public int heat { get => PlayerHeat.curValue; set => PlayerHeat.SetValue(value); }
    public int maxSpeed;
    public int speed { get => PlayerSpeed.curValue; set => PlayerSpeed.SetValue(value); }
    public bool isDead;
    public IntEventSO playerHurtEvent;
    public ObjectEventSO playerDeadEvent;

    private void OnEnable()
    {
        ActPoints.maxValue = maxPoints;
        curPoints = ActPoints.maxValue;
        Init();
    }
    /// <summary>
    /// 判断是否还没选择辅助和上场英雄
    /// </summary>
    /// <returns></returns>
    public bool isHeroEmpty()
    {
        foreach (Hero hr in stageHeroList)
        {
            if (hr.heroData == null) return true;
        }
        foreach (Hero hr in supportHeroList)
        {
            if (hr.heroData == null) return true;
        }
        return false;
    }
    public void pushStage(HeroDataSO data) //上阵战斗区
    {
        foreach (Hero hr in stageHeroList)
        {
            if (hr.heroData == null)
            {
                hr.Init(data);
                return;
            }
        }
    }
    public void pushSuport(HeroDataSO data) //上阵辅助区
    {
        foreach (Hero hr in supportHeroList)
        {
            if (hr.heroData == null)
            {
                hr.Init(data);
                return;
            }
        }
    }
    

    public void Init()
    {
        transform.GetComponentsInChildren<Transform>(true);
        var heroes = GetComponentsInChildren<Hero>();

        //上场好汉初始化
        for (int i = 0; i < 2; i++)
        {
            stageHeroList.Add(heroes[i]);
            heroes[i].Init(heroFormationConfig.heroDataList[i]);
            heroes[i].player = this;
        }

        //支持好汉初始化
        for (int i = 2; i < 6; i++)
        {
            supportHeroList.Add(heroes[i]);
            heroes[i].Init(heroFormationConfig.heroDataList[i]);
            heroes[i].player = this;
        }

        tolHeroList = heroes.ToList();
    }

    /// <summary>
    /// 事件监听函数
    /// </summary>
    public void NewTurn()
    {
        curPoints = Math.Min(maxPoints, curPoints + 3);

        foreach (var item in tolHeroList)
        {
            item.PassiveExecute(HeatTriggerType.OnRoundStart);
        }

    }

    public void UpdateMana(int cost)
    {
        curPoints = Math.Max(0, curPoints - cost);
    }

    /// <summary>
    /// 整一盘游戏开始调用
    /// </summary>
    public void NewGame()
    {
        heat = maxHeat;
        isDead = false;
        speed = 0;
        //ClearHeroData();

        NewTurn();
    }

    /// <summary>
    /// 人物热度受损时引用
    /// </summary>
    public void TakeDamage(int dmg)
    {
        if (heat - dmg > -4)
        {
            //animator.SetTrigger("hit");
            heat -= dmg;
            playerHurtEvent.RaiseEvent(dmg, this);
            Debug.Log($"{this}受到了{dmg}伤害");
        }
        else
        {
            heat = -4;
            isDead = true;
            Debug.Log(this + "噶了");
            playerDeadEvent.RaiseEvent(this, this);
        }
    }

    /// <summary>
    /// 事件函数，用于设置速度 
    /// </summary>
    /// <param name="val"></param>
    public void SetSpeed(int val)
    {
        if (val > 0)
        {
            speed = Mathf.Min(speed + val, maxSpeed);
        }
        else
        {
            speed = Mathf.Max(speed + val, 0);
        }
    }

    public Hero GetHeroFromData(HeroDataSO data)
    {
        foreach (var hr in supportHeroList)
        {
            if (hr.heroData == data) return hr;
        }
        foreach (var hr in stageHeroList)
        {
            if (hr.heroData == data) return hr;
        }
        return null;
    }

    #region 热度启动事件
    /// <summary>
    /// 单局开始时调用
    /// </summary>
    public void GameStart()
    {
        foreach (var item in tolHeroList)
        {
            item.PassiveExecute(HeatTriggerType.OnStart);
        }
    }

    /// <summary>
    /// 使用卡时调用
    /// </summary>
    public void OnUseCard()
    {
        foreach (var item in tolHeroList)
        {
            item.PassiveExecute(HeatTriggerType.OnUseCard);
        }
    }

    /// <summary>
    /// 事件函数，用于好汉噶了之后
    /// </summary>
    /// <param name="hero"></param>
    public void OnHeroDie(object hero)
    {
        if (hero is Hero)
        {
            var hr = hero as Hero;

            tolHeroList.Remove(hr);
            stageHeroList.Remove(hr);
            supportHeroList.Remove(hr);
        }
    }

    #endregion
}

