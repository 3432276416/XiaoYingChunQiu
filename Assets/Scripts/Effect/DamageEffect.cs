using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Effect/DamageEffect")]
public class DamageEffect : Effect
{

    public override void Execute(object user, object receiver)
    {
        if(receiver is Player)
        {
            Player player = receiver as Player;
            player.heat -= value;
        }
    }

    public override void Execute(object hero)
    {
        if(hero is EnmSoldier)
        {
            (hero as EnmSoldier).curHp -= value;
        }
        if (hero is List<Hero>)
        {
            var temp = hero as List<Hero>;
            temp[0].TakeDamage(value);
            temp[1].TakeDamage(value);
        }
    }
}