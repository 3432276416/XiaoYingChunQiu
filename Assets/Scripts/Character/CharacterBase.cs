using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp; //最大生命值
    public int curHp; //当前生命值
    protected Animator animator;
    public SpriteRenderer sprite;
    public Transform spriteTransform;
    public bool isDead;
    public int defense;
    public int Attack;  //攻击
    [Header("组件")]
    public GameObject hpChangeDisplay;
    public HeroInfoDisplayController heroInfoDisplayController;
    [Header("buff/debuff")]
    /*     public GameObject buff;
        public GameObject debuff; */
    [SerializeField] public List<Buff> buffs = new();
    //public IntVariable buffRound;
    [Header("增伤乘区")]
    public float baseStrength = 1f;
    float strengthEff = 0.5f;
    [Header("广播")]
    public ObjectEventSO characterDeadEvent;
    public IntEventSO characterHurtEvent;
    /// <summary>
    /// 获取组件
    /// </summary>
    protected virtual void Awake()
    {
        //animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        //animator.SetBool("isDead", isDead);
    }
    /// <summary>
    /// 游戏一开始的时候赋值
    /// </summary>
    protected virtual void Start()
    {
        curHp = maxHp;

        //buffs.Clear();
        ResetDefence();
    }

    /// <summary>
    /// 人物受伤时扣血
    /// </summary>
    public void TakeDamage(int dmg)
    {
        var curDmg = Math.Max(dmg - defense, 0);
        var curDef = Math.Max(defense - dmg, 0);

        defense = curDef;

        HurtAnimation(curDmg);

        if (curHp > curDmg)
        {
            //animator.SetTrigger("hit");
            curHp -= curDmg;
            characterHurtEvent.RaiseEvent(curDmg, this);
            UpdateHeroHp(curDmg);
            Debug.Log($"{this}受到了{dmg}伤害");
        }
        else
        {
            curHp = 0;
            isDead = true;

            UpdateHeroHp(curDmg);
            Debug.Log(this + "噶了");
            StartCoroutine(DeadDisappear());
        }
    }

    /// <summary>
    /// 受伤动画
    /// </summary>
    public void HurtAnimation(int dmg)
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        
        sequence.Insert(0f,spriteTransform.DOPunchPosition(new Vector3(UnityEngine.Random.Range(0.12f, 0.24f) * dmg,
        UnityEngine.Random.Range(0.12f, 0.24f) * dmg, 0), 0.9f))
        .Insert(0f,sprite.DOColor(Color.red, 0.15f))
        .Insert(0.85f,sprite.DOColor(Color.white,0.15f));
    }

    /// <summary>
    /// 更新防御值
    /// </summary>
    /// <param name="val"></param>
    public void UpdateDefence(int val)
    {
        int value = defense + val;
        defense = value;
    }
    /// <summary>
    /// 回合开始时重置
    /// </summary>
    public void ResetDefence()
    {
        defense = 0;
    }
    /// <summary>
    /// 更新生命值，回血
    /// </summary>
    /// <param name="val"></param>
    public void HealCharacter(int val)
    {
        int value = Mathf.Min(curHp + val, maxHp);
        curHp = value;
        //buff.SetActive(true);
    }

    /// <summary>
    /// 给自己加增伤
    /// </summary>
    /// <param name="round">持续回合</param>
    /// <param name="isPositive">是buff还是debuff</param>
    public void SetupStrength(int round, bool isPositive)
    {
        if (isPositive)
        {
            float newStrength = strengthEff + baseStrength;

            baseStrength = Mathf.Min(1.5f, newStrength);
            //启动动画
            //buff.SetActive(true);
        }
        else
        {
            float newStrength = baseStrength - strengthEff;

            baseStrength = Mathf.Max(0.5f, newStrength);
            //启动动画
            //debuff.SetActive(true);
        }

        var tarBuff = buffs.Find(t => t.type == BuffType.Strength || t.type == BuffType.Weak);
        if (baseStrength == 1)
        {
            buffs.Remove(tarBuff);
            return;
        }

        if (buffs.Contains(tarBuff))
        {
            var curRound = tarBuff.buffRound + round;
            tarBuff.buffRound = curRound;
        }
        else
        {
            var strengthBuff = new Buff
            {
                type = isPositive ? BuffType.Strength : BuffType.Weak,
                buffRound = round
            };
            buffs.Add(strengthBuff);
        }
    }

    #region 回合转换事件函数
    public void UpdateRound()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].buffRound = Math.Max(buffs[i].buffRound - 1, 0);
            switch (buffs[i].type)
            {
                case BuffType.Strength:
                case BuffType.Weak:
                    UpdateStrengthRound(buffs[i]);
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 回合转换事件函数
    /// </summary>
    public void UpdateStrengthRound(Buff buff)
    {
        if (buff.buffRound <= 0)
        {
            baseStrength = 1;
            buffs.Remove(buff);
        }

    }
    #endregion
    IEnumerator DeadDisappear()
    {
        yield return new WaitForSeconds(1.5f);

        characterDeadEvent.RaiseEvent(this, this);
        gameObject.SetActive(false);//以后还要用毁他干什么
    }

    /// <summary>
    /// 更新好汉血量与显示
    /// </summary>
    /// <param name="val">受伤点数</param>
    public void UpdateHeroHp(int val)
    {
        heroInfoDisplayController.UpdateUI(curHp);
        hpChangeDisplay.SetActive(true);
        hpChangeDisplay.GetComponentInChildren<TextMeshPro>().text = (-val).ToString();
    }

}