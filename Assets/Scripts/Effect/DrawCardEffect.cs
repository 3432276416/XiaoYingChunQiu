using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Effect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;

    public override void Execute(object user, object receiver)
    {
        drawCardEvent?.RaiseEvent(value, receiver as Player);
    }

    public override void Execute(object hero)
    {
      

    }
}