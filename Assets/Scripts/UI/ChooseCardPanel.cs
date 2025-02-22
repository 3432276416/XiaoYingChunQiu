using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public enum ChooseType //选择类型，好汉，小兵，buff等
{ 
    Hero,
    Card, //包含小兵好汉
}


public class ChooseCardPanel : MonoBehaviour
{
    public Player player;
    public CardMgr cardMgr;
    VisualElement rootElement;
    public VisualTreeAsset cardTemplate; 
    VisualElement container;
    public List<HeroDataSO> heroList; //待选中的卡牌
    CardDataSO selectedCardData; //选中的卡牌
    HeroDataSO selectedHeroData; //选中的英雄
    List<Button> cardBtns = new();
    public ChooseType choice;  //选择类型
    Button confirmBtn;
    public Effect curCardEffect; //当前需要执行的效果
    [Header("广播")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO onChooseEvent;

    private void OnEnable() {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        container = rootElement.Q<VisualElement>("Container");
        confirmBtn = rootElement.Q<Button>("ConfirmBtn");
    
        confirmBtn.clicked += OnConfirmBtnClicked;
        if(player.isHeroEmpty())
        {
            choice = ChooseType.Hero;
            Debug.Log("开始选择英雄");
        }
        else
        {
            choice= ChooseType.Card;
            Debug.Log("选择卡牌");
        }

        for (int i = 0; i < 2; i++) //目前是三个
        {
            //生成卡模板
            var card = cardTemplate.Instantiate();
            var cardBtn = card.Q<Button>("Card");
            var data = player.stageHeroList[i].heroData;
            InitHero(card,data);
            heroList.Add(data);
            cardBtn.clicked += () => OnCardClicked(cardBtn, data);

            //if (choice == ChooseType.Hero)
            //{
            //    var data = cardMgr.GetNewHeroData();
            //    heroList.Add(data);
            //    cardBtn.clicked += () => OnCardClicked(cardBtn, data);
            //    InitHero(card, data);

            //}
            //else if (choice == ChooseType.Card)
            //{
            //    var data = cardMgr.GetNewCardData();
            //    cardBtn.clicked += () => OnCardClicked(cardBtn, data);
            //    InitCard(card, data);
            //}

            cardBtns.Add(cardBtn);
            container.Add(card);
        }


    }
    /// <summary>
    /// 选择后设置上场英雄
    /// </summary>
    public void ChooseHero()
    {
        foreach(var data in heroList)
        {
            if(data==selectedHeroData)
            {
                player.pushStage(data);
                continue;
            }
            player.pushSuport(data);
        }
    }

    public void InitHero(VisualElement card,HeroDataSO data)
    {
        var cardSpriteEle = card.Q<VisualElement>("CardSprite");
        cardSpriteEle.style.backgroundImage = new StyleBackground(data.heroSprite);
    }

    /// <summary>
    /// 初始化卡模板里面的数据
    /// </summary>
    /// <param name="card"></param>
    /// <param name="data"></param>
    void InitCard(VisualElement card, CardDataSO data)
    {
        var cardSpriteEle = card.Q<VisualElement>("CardSprite");
        var cost = card.Q<Label>("EnergyCost");
        var description = card.Q<Label>("CardDescription");
        var type = card.Q<Label>("CardType");

        cardSpriteEle.style.backgroundImage = new StyleBackground(data.sprite);
        //cost.text = data.cost.ToString();
        //description.text = data.description.ToString();
        //type.text = data.type switch
        //{
        //    CardType.Item => "道具",
        //    CardType.Soldier => "兵",
        //    CardType.Hero => "好汉",
        //    _ => throw new System.NotImplementedException(),
        //};
    }

    
     
/// <summary>
/// 点击卡牌按钮后当前卡牌不可用
/// </summary>
/// <param name="cardBtn"></param>
/// <param name="data"></param>
    private void OnCardClicked(Button cardBtn, object data)
    {
        if(data is HeroDataSO)
        {
            selectedHeroData = data as HeroDataSO;
        }
        else if(data is CardDataSO)
        {
            selectedCardData = data as CardDataSO;
        }

        //Debug.Log(data.description);
        //for(int i = 0;i<heroList.Count;i++)
        //{
        //    if (heroList[i]==selectedCardData)
        //    {
        //        cardBtns[i].SetEnabled(true);
        //    }
        //}

        for (int i = 0; i < cardBtns.Count; i++)
        {
            if (cardBtns[i] == cardBtn)
            {
                cardBtns[i].SetEnabled(false);
            }
            else
            {
                cardBtns[i].SetEnabled(true);
            }
        }
    }

    private void OnConfirmBtnClicked()
    {
        //cardMgr.UnlockCard(curCardData);
        //  if(heroList.Count>0)
        //{
        //    ChooseHero();
        //}


        //if(player.isHeroEmpty()) //还有空位则再次进行选择
        //{
        //    onChooseEvent.RaiseEvent(this, this);
        //}
        //loadMapEvent.RaiseEvent(null,this);


        if(curCardEffect != null)
        {
            curCardEffect.Execute(player.GetHeroFromData(selectedHeroData));
        }
        gameObject.SetActive(false);

    }
}
