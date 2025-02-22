using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StrengtHerohEffect", menuName = "Effect/StrengthHeroEffect")]
public class StrengthHeroEffect : Effect
{

    public override void Execute(object hero)
    {
        if (hero is Hero)
        {
            Hero hr = hero as Hero;
            hr.Attack += value;
        }
        else if (hero is List<Hero>)
        {
            foreach (var item in hero as List<Hero>)
            {
                item.Attack += value;
            }
        }
    }

    public override void Execute(object user, object receiver)
    {
       if(receiver is Player) //增加所有好汉攻击力
        {
            Player player = receiver as Player;
            foreach (var item in player.stageHeroList) {
                item.Attack += value;
            }
        }
    }

}