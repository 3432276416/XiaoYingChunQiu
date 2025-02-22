using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmSoldier : CharacterBase
{
    public EnmSoldierType type;
    public EnmDataSO enmData;
    public void Init(EnmDataSO enmData)
    {
        buffs.Clear();
        this.enmData = enmData;
        this.curHp = enmData.HP;
        Attack = enmData.Attack;
    }
    
}
