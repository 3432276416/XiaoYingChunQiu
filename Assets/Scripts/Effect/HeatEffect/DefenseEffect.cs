using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Effect/HeatEffect/DefenseEffect")]
public class DefenseEffect : HeatEffect
{
    public override void Execute(object user, object receiver)
    {
        foreach (var item in (receiver as Player).tolHeroList)
        {
            item.UpdateDefence(value);
        }
    }

    public override void Execute(object hero)
    {
    }
}
