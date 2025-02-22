using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealOneHeroEffect", menuName = "Effect/HealOneHeroEffect")]

public class HealOneHeroEffect : EffectToHero
{
    public override void Execute(Hero hero)
    {
        int healNum=Mathf.Min(hero.curHp+value, hero.maxHp);
        hero.curHp=healNum;
    }
}
