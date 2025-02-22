using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnmHeroInfoDisplayController : MonoBehaviour
{
    [Header("组件")]
    [SerializeField]SpriteRenderer heroSprite;
    public GameObject heroSpriteObj;
    [Header("显示相关")]
    public TextMeshPro atkText;
    public TextMeshPro HPText;

    /// <summary>
    /// 初始化显示
    /// </summary>
    /// <param name="heroData"></param>
    public void Init(HeroDataSO heroData)
    {
        heroSprite = heroSpriteObj.GetComponent<SpriteRenderer>();
        heroSprite.sprite = heroData.heroSprite;
        atkText.text = heroData.Attack.ToString();
        HPText.text = heroData.HP.ToString();
    }

    public void UpdateHeroHp(int val)
    {
        HPText.text = val.ToString();
    }
}
