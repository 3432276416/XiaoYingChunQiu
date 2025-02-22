using UnityEngine;

[CreateAssetMenu(fileName = "ConditionDrawCardEffect", menuName = "Effect/HeatEffect/ConditionDrawCardEffect")]
public class ConditionDrawCardEffect : HeatEffect
{
    public IntEventSO drawCardEvent;
    public int maxUseCardCount;
    int useCardCount = 0;
    public override void Execute(object user, object receiver)
    {
        useCardCount++;
        if (useCardCount >= maxUseCardCount)
        {
            useCardCount = 0;
            drawCardEvent?.RaiseEvent(value,user);
        }
    }

    public override void Execute(object hero)
    {
      

    }
}