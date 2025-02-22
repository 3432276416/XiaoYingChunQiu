using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ??????????????n??
/// </summary>
[CreateAssetMenu(fileName = "RandomHurtEffect", menuName = "Effect/RandomHurtEffect")]
public class RandomHurtEffect : Effect
{
    public override void Execute(object user, object receiver)
    {

    }

    public override void Execute(object hero)
    {
        if (hero is Hero)
        {
            Hero hr = hero as Hero;
            hr.TakeDamage(value);
        }
        else if (hero is List<Hero>) //???????
        {
            List<Hero> list = hero as List<Hero>;
            var temp = list[Random.Range(0,list.Count)];
            temp.TakeDamage(1);
        }
    }
}
