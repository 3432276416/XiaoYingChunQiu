using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "SoliderCardEffect", menuName = "Effect/SoliderCardEffect")]

public class SoliderCardEffect : Effect
{
    public int Hp;
    public int Attack;
    public SoldierType soldierType;
    public override void Execute(object hero)
    {
        if (!(hero is Hero))
        {
            return; //只能辅助单个好汉
        }
        CardDataSO cardData = this.GetComponent<CardDataSO>();
        if (cardData != null)
        {
            Hero hr = hero as Hero;
            hr.AddSolider(new Solider(cardData, Hp, Attack, soldierType));
        }
    }

    public override void Execute(object user, object receiver)
    {
        throw new System.NotImplementedException();
    }
}
