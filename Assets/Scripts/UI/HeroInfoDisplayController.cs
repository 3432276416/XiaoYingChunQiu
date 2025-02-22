using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroInfoDisplayController : MonoBehaviour
{
    [Header("组件")]
    [SerializeField]SpriteRenderer heroSprite;
    public GameObject heroSpriteObj;
    [Header("显示相关")]
    public SpriteRenderer elementSprite;
    public SpriteRenderer frameSprite;
    public TextMeshPro atkText;
    public TextMeshPro HPText;
    [SerializeField]List<Sprite> elementSpriteList;
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
        
        if (heroData.elem == Elem.None) return;
        elementSprite.sprite = elementSpriteList[(int)heroData.elem];
    }

    public void UpdateUI(int val)
    {
        HPText.text = val.ToString();
    }
}
