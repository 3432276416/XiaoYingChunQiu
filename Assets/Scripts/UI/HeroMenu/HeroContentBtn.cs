using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeroContentBtn : MonoBehaviour
{
    private Button cardBtn;
    public Image buttonImage;
    ////public CardDataSO cardDataSO;
    public HeroDataSO heroData;
    public bool isSelect; //是否被选中
    public ObjectEventSO CardMenuSelectEvent;
   

    private void Start()
    {
        isSelect = false;
        cardBtn = GetComponent<Button>();
        //buttonImage = GetComponent<Image>();
        cardBtn.onClick.AddListener(onSelect);
    }

    public void onSelect()
    {

       
        CardMenuSelectEvent.RaiseEvent(this, this);
        isSelect = !isSelect;
        UpdateButton();

    }

    public void Init(HeroDataSO data)
    {

        if(data is HeroDataSO)
        {
            
            heroData=data as HeroDataSO;
            buttonImage.sprite = heroData.heroSprite;
          
        }
    }

    public void UpdateButton()
    {
        
       
        if (isSelect)
        {
            ColorBlock colors = cardBtn.colors;
            colors.normalColor = Color.gray;
            colors.highlightedColor = Color.gray;
            cardBtn.colors = colors;
        }
        else
        {
            ColorBlock colors = cardBtn.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = Color.white;
            cardBtn.colors = colors;
        }
    }
}
