using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectToPlayer : EffectBase
{
    public abstract void Execute(Player player,Enm enm);
}
