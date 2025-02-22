using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Enm : MonoBehaviour
{
    public EnmActionDataSO actionDataSO;
    public EnmAction curAction;
    protected Player player;
    protected Animator animator;
    public EnmFormationConfigSO enmDataConfig;
    public List<EnmSoldier> enms; //敌方单位
    List<int> randomIndexList = new();
    public int heat; //热度
    public IntVariable EnmSpeed;
    public int maxSpeed;
    public int speed { get => EnmSpeed.curValue; set => EnmSpeed.SetValue(value); }    
    public void Init()
    {
        enms = GetComponentsInChildren<EnmSoldier>().ToList();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        /* for (int i = 0; i < enms.Count; i++)
        {
            enms[i].Init(enmDataConfig.enmDataList[i]);
        } */
    }
    public virtual void OnPlayerTurnBegin()
    {
        randomIndexList.Clear();
        for (int i = 0; i < enms.Count; i++)
        {
            int ranIndex = Random.Range(0, actionDataSO.Actions.Count);
            randomIndexList.Add(ranIndex);
        }
    }

    public virtual void OnEnmTurnBegin()
    {
        for (int i = 0; i < enms.Count; i++)
        {
            StartCoroutine(ProcessDelayAction(i));
        }
    }

    IEnumerator ProcessDelayAction(int i)
    {
        curAction = actionDataSO.Actions[randomIndexList[i]];

        yield return new WaitForSeconds(1.25f * i);
        //动画
        AttackAnimation(i);

        switch (curAction.effect.targetType)
        {
            case EffectTargetType.SingleHero:
            case EffectTargetType.MultiHero:
                curAction.effect.Execute(player.tolHeroList);
                break;
            case EffectTargetType.Player:
                curAction.effect.Execute(player);
                break;
            case EffectTargetType.Enm:
                curAction.effect.Execute(this);
                break;
            case EffectTargetType.All:
                curAction.effect.Execute(this);
                curAction.effect.Execute(player);
                break;
            default:
                break;
        }
    }

    public void AttackAnimation(int i)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Insert(0, enms[i].spriteTransform.DOScale(1.14f, 0.25f))
        .Insert(1f, enms[i].spriteTransform.DOScale(1f, 0.25f));
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
        Debug.Log(speed);
    }
}
