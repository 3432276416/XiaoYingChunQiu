using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CardLayoutMgr : MonoBehaviour
{
    public bool isHorizental;
    public float maxWidth = 7f;
    [SerializeField] List<Vector3> cardPos;
    [SerializeField] List<Quaternion> cardRot;
    float cardSpacing = 2;
    [Header("弧形参数")]
    public float angBetweenCards = 7.5f;
    public float radius = 17f;
    public Vector3 centerPoint;

    private void Awake() {
        centerPoint = isHorizental ? Vector3.up*-4.5f : Vector3.up*-21.5f;
    }
    void CalcPos(int num, bool horizental)
    {
        cardPos.Clear();
        cardRot.Clear();
        
        if (horizental)
        {
            float totalWidth = Mathf.Min(maxWidth, cardSpacing * (num - 1));
            float curSpacing = totalWidth > 0 ? totalWidth / (num - 1) : 0;

            for (int i = 0; i < num; i++)
            {
                float posx = 0-(totalWidth/2) + (curSpacing*i);

                cardPos.Add(new Vector3(posx,centerPoint.y,0f));
                cardRot.Add(quaternion.identity);
            }
        }
        else
        {
            angBetweenCards = Mathf.Min(7.5f,50/num);
            
            float cardAngle = Mathf.Min(num - 1) * angBetweenCards / 2;

            for (int i = 0; i < num; i++)
            {
                var pos = FanCardPosition(cardAngle - i * angBetweenCards);

                var rotation = Quaternion.Euler(0, 0, cardAngle - i * angBetweenCards);
                cardPos.Add(pos);
                cardRot.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
        );
    }

    public CardTransform GetCardTransForm(int index,int totalCards)
    {
        CalcPos(totalCards, isHorizental);
        return new CardTransform(cardPos[index],cardRot[index]);
    }
}
