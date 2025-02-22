using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class EffectToHero : EffectBase
{
    public int amount; //对多少个英雄，1个对单
    public abstract void Execute(Hero hero);
}
