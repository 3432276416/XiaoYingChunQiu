using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider : CharacterBase
{
    public CardDataSO cardData;
    public Elem element;
    public SoldierType soldierType;
    public Hero belongHero;
    public List<Effect> effects;
    [Header("显示相关")]
    [SerializeField]List<Sprite> elementSpriteList;
    [SerializeField]List<Sprite> soldierElementSpriteList;
    public SpriteRenderer typeRenderer;
    public SpriteRenderer soldierTypeRenderer;
    public void SetAssistHero(Hero hero)
    {
        belongHero = hero;
        hero.curHp += curHp;
        hero.maxHp += maxHp;
        hero.Attack += Attack;
        hero.ownSoliders.Add(this);
    }

    public void OnSoliderDead()
    {
        belongHero.ownSoliders.Remove(this);
        belongHero.curHp -= curHp;
        belongHero.Attack -= Attack;
        StartCoroutine("DeadDisappear");
    }

    public Solider(CardDataSO cardData, int Hp, int Attack,SoldierType soldierType)
    {
        this.cardData = cardData;
        this.curHp = maxHp = Hp;
        this.Attack = Attack;
        this.soldierType = soldierType;
        this.soldierTypeRenderer.sprite = soldierElementSpriteList[(int)soldierType];
        this.typeRenderer.sprite = elementSpriteList[(int)cardData.elem];
    }

}
