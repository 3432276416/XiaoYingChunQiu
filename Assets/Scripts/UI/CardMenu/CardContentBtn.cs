using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardContentBtn : MonoBehaviour
{
    private Button cardBtn;
    public Image buttonImage;
    public Text numText;
    public CardDataSO cardData;
   

    public ObjectEventSO CardSelectedEvent;

    private void Start()
    {
        cardBtn = GetComponent<Button>();
        //buttonImage = GetComponent<Image>();
        cardBtn.onClick.AddListener(onSelect);
    }

    public void onSelect()
    {
        CardSelectedEvent.RaiseEvent(this, this);
    }

    public void Init(CardDataSO data)
    {

         this.cardData = data;
         buttonImage.sprite = cardData.sprite;
        numText.text = "0";

  
    }
}
