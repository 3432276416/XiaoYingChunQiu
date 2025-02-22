using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContentButton : MonoBehaviour
{
    private Button cardBtn;
    public Image buttonImage;
    ////public CardDataSO cardDataSO;
    public HeroDataSO heroData;
    public bool isSelect;
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
        this.isSelect=!isSelect;
        CardMenuSelectEvent.RaiseEvent(this, this);
    }

    public void Init(HeroDataSO data)
    {

        if(data is HeroDataSO)
        {
            Debug.Log("ÓÐÎïÌå");
            heroData=data as HeroDataSO;
            buttonImage.sprite = heroData.heroSprite;
          
        }
    }
}
