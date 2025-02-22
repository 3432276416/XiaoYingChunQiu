using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Effect/HealEffect")]

public class HealEffect : Effect
{
    public override void Execute(object hero)
    {
        if (hero is Hero)
        {
            Hero hr=hero as Hero;
            int healNum = Mathf.Min(hr.curHp + value, hr.maxHp);
            hr.curHp = healNum;
        }
        else if(hero is List<Hero>)
        {
            List<Hero> list=hero as List<Hero>;
            foreach (Hero hr in list)
            {
                int healNum = Mathf.Min(hr.curHp + value, hr.maxHp);
                hr.curHp = healNum;
            }
        }
    }
    public override void Execute(object user,object receiver)
    {
        if (user == null && receiver == null)
            return;
        if(receiver is Player)
        {
            Player player=receiver as Player;
            player.heat += value;
        }
    }
}
