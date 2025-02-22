using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Player player;
    public CardDataSO cardData;
    public bool isAvailable;
    [Header("组件")]
    public SpriteRenderer cardSprite; //卡牌图片
    public SpriteRenderer frameSprite; //边框图片
    public TextMeshPro cost;
    public CharacterBase tarCharacter;//目标

    [Header("原始数据")]
    public Vector3 originalPos;
    public quaternion originalRot;
    public int originalLayerOrder;
    [Header("广播")]
    public ObjectEventSO DiscardCardEvent;
    public IntEventSO costEvent;
    public bool isAnimating;

    public Enm enm;
    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        Init(cardData);
    }
    public void Init(CardDataSO data)
    {
        //if (cardData.type == CardType.Soldier)
        //{
        //    Sprite frame = Resources.Load<Sprite>("CardFrameWork/" + cardData.elem.ToString() + "Frame") as Sprite;
        //    if (frame == null) { Debug.LogWarning("加载边框失败"); }
        //    frameSprite.sprite = frame;
        //}
        cardData = data;
        cardSprite.sprite = data.sprite;
        cost.text = data.cost.ToString();
        cost.color = Color.green;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void UpdateOriginData(Vector3 pos, Quaternion rot)
    {
        originalPos = pos;
        originalRot = rot;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAnimating) return;
        var highlightShowPos = new Vector3(originalPos.x, -3.5f, 0);
        transform.SetPositionAndRotation(highlightShowPos, quaternion.identity);
        //至高图层
        GetComponent<SortingGroup>().sortingOrder = 100;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating) return;

        //重置位置
        ResetCardPos();
    }

    public void ResetCardPos()
    {
        transform.SetPositionAndRotation(originalPos, originalRot);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    /// <summary>
    /// 卡牌执行效果
    /// </summary>
    /// 
    /// <param name="tar"></param>
    public void ExecuteCardEff(object tar)
    {
        foreach (var item in cardData.cardEffs)
        {
            switch (item.targetType)
            {
                case EffectTargetType.Player:
                    item.Execute(enm, player);
                    break;
                case EffectTargetType.Enm:
                    item.Execute(player, enm);
                    break;
                case EffectTargetType.SingleHero:
                    if (tar is Hero)
                        item.Execute(tar as Hero);
                    break;
            }
        }
        costEvent.RaiseEvent(cardData.cost, this);
        DiscardCardEvent.RaiseEvent(this, this);
    }
    public bool isEffectArrow() //判断是否需要箭头
    {
        foreach (var item in cardData.cardEffs)
        {
            if (item.targetType == EffectTargetType.SingleHero)
            {
                return true;
            }
        }
        return false;
    }
    public void UpdateCardState()
    {
        isAvailable = cardData.cost <= player.curPoints;
        cost.color = isAvailable ? Color.green : Color.red;
        //Debug.Log(this + ":" + isAvailable);
    }
}
