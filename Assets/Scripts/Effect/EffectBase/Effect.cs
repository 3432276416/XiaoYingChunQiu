using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/Effect")]
public abstract class  Effect : EffectBase
{

   
    [SerializeField]string Description; //Ч��˵��

    public abstract void Execute(object user, object receiver);
    public abstract void Execute(object hero);

}
