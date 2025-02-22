using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SearchService;

public class CardDragHandler : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Card curCard;
    bool canMove;
    bool canExecute;
    public GameObject arrowPrefab;
    GameObject Arrow; //选择多个时候的箭头
    object effectTarget; //效果目标,可以是好汉，不是好汉就是玩家或者敌方
    public Player player;
    public Enm enm;

    private void Awake() {
        curCard = GetComponent<Card>();
    }

    private void OnEnable() {
        player = FindObjectOfType<Player>();
        enm = FindObjectOfType<Enm>();
    }
    private void OnDisable() {
        canMove = false;
        canExecute = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!curCard.isAvailable) return;
        switch (curCard.cardData.type)
        {
            case CardType.Item:
                canMove = true;
                break;
            case CardType.Soldier:
                canMove = false;
                Arrow = Instantiate(arrowPrefab, transform.position, quaternion.identity, transform);
                break;
            case CardType.Hero:
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!curCard.isAvailable) return;

        if (canMove)
        {
            curCard.isAnimating = true;//拖拽时不执行划入划出事件
            Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);//z=10是因为摄像机的z坐标是-10
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);//屏幕坐标转化为世界坐标
            curCard.transform.position = worldPos;

            canExecute = worldPos.y > 1f;
        }
        else
        {
            if (eventData.pointerEnter == null) return;
            if (eventData.pointerEnter.CompareTag("PlayerHero"))
            {
                canExecute = true;
                effectTarget = eventData.pointerEnter.GetComponent<Hero>();
                return;
            }
            else
            {
                Debug.Log("没有选择英雄");
            }
            canExecute = false;
            effectTarget = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!curCard.isAvailable) return;

        Destroy(Arrow);
        if (canExecute)
        {
            curCard.ExecuteCardEff(effectTarget);
            player.OnUseCard();
        }
        else
        {
            curCard.ResetCardPos();
            curCard.isAnimating = false;
        }
    }

}
