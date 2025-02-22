using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeSpeedEffect", menuName = "Effect/HeatEffect/ChangeSpeedEffect")]
public class ChangeSpeedEffect : HeatEffect
{
    public override void Execute(object user, object receiver)
    {
        if (receiver is Enm)
        {
            var temp = receiver as Enm;
            temp.SetSpeed(value);
        }
        else if (receiver is Player)
        {
            var temp = receiver as Player;
            temp.SetSpeed(value);
        }
    }

    public override void Execute(object hero)
    {
    }

}
