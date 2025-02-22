using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;


public class CardDeck : MonoBehaviour
{
    public CardMgr cardMgr;
    public TurnBaseMgr turnBaseMgr;
    public List<CardDataSO> drawDeck = new(); //抽牌堆
    public List<CardDataSO> discardDeck = new(); //弃牌堆
    public List<Card> handCardObjList = new(); //每回合手牌堆
    public CardLayoutMgr cardLayoutMgr;
    public Vector3 deckPos;//发牌起点坐标

    [Header("广播")]
    public IntEventSO DrawDeckCountChangedEvent;
    public IntEventSO DiscardDeckCountChangedEvent;

//仅测试用
    private void Start() {
        InitCard();
    }
    
    public void InitCard()
    {
        drawDeck.Clear();

        foreach (var item in cardMgr.curCardLib.cardLibList)
        {
            for (int i = 0; i < item.amount; i++)
            {
                drawDeck.Add(item.cardData);
            }
        }
        ShuffleDeck();
    }

    [ContextMenu("测试抽卡")]
    void TestDrawCard()
    {
        DrawCard(1);
    }

    public void NewTurnDrawCard()
    {
        if (turnBaseMgr.round > 1)
        {
            DrawCard(1);
        }
        else
        {
            DrawCard(4);
            //为保险重新刷新一遍卡牌状态
            foreach (var item in handCardObjList)
            {
                item.UpdateCardState();
            }
        }
    }

    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }
            var cardData = drawDeck[0];
            drawDeck.RemoveAt(0);

            DrawDeckCountChangedEvent.RaiseEvent(drawDeck.Count,this);

            var card = cardMgr.GetCard().GetComponent<Card>();

            card.Init(cardData);
            card.transform.position = deckPos;

            handCardObjList.Add(card);

            float delay = i*0.2f;
            SetCardLayout(delay);
        }
    }

    void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjList.Count; i++)
        {
            var curCard = handCardObjList[i];
            
            CardTransform cardTransform = cardLayoutMgr.GetCardTransForm(i,handCardObjList.Count);

            //curCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rot);
            curCard.isAnimating = true;
            curCard.UpdateCardState();

            curCard.transform.DOScale(Vector3.one , 0.1f).SetDelay(delay).onComplete = () =>
            {                
                curCard.transform.DOMove(cardTransform.pos,0.5f).onComplete = () => 
                {
                    curCard.isAnimating = false; 
                };
                curCard.transform.DORotateQuaternion(cardTransform.rot,0.5f);
            };
            //设置卡牌图层
            curCard.GetComponent<SortingGroup>().sortingOrder = i;
            curCard.UpdateOriginData(cardTransform.pos,cardTransform.rot);
            
            //Debug.Log(curCard.player.curMana);
        }
    }

/// <summary>
/// 洗牌逻辑
/// </summary>
    void ShuffleDeck()
    {
        discardDeck.Clear();
        DrawDeckCountChangedEvent.RaiseEvent(drawDeck.Count,this);
        DiscardDeckCountChangedEvent.RaiseEvent(discardDeck.Count,this);

        for (int i = 0; i < drawDeck.Count; i++)
        {
            var tempData = drawDeck[i];
            var num = Random.Range(i,drawDeck.Count);
            drawDeck[i] = drawDeck[num];
            drawDeck[num] = tempData;
        }
    }

/// <summary>
/// 弃牌逻辑，事件函数
/// </summary>
/// <param name="card"></param>
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;

        discardDeck.Add(card.cardData);
        handCardObjList.Remove(card);

        cardMgr.DiscardCard(card.gameObject);
        
        DiscardDeckCountChangedEvent.RaiseEvent(discardDeck.Count,this);
        SetCardLayout(0f);
    }

    public void OnPlayerRoundEnd(){
        for (int i = 0; i < handCardObjList.Count; i++)
        {
            /* discardDeck.Add(handCardObjList[i].cardData);
            cardMgr.DiscardCard(handCardObjList[i].gameObject); */
            //防止在敌方回合时出牌
            handCardObjList[i].isAvailable = false;
        }

        //handCardObjList.Clear();
        //DiscardDeckCountChangedEvent.RaiseEvent(discardDeck.Count,this);
    }


    /// <summary>
    /// 弃所有牌
    /// </summary>
    /// <param name="obj"></param>
    public void ReleaseAllCards(object obj) 
    {
        foreach (var item in handCardObjList)
        {
            cardMgr.DiscardCard(item.gameObject);
        }

        handCardObjList.Clear();
        InitCard();
    }
}
