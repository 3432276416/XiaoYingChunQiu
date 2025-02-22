using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectContent : MonoBehaviour
{
    public ObjectEventSO UpdateChooseEvent;

    public GameObject CardShowPrefab;
    public Sprite OriginSprite;
   

    public CardMenu cardMenu;


    public void UpdateChooseCard()
    {

        Debug.Log("更新卡牌显示");
      for(int i = 0;i<this.transform.childCount;i++)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite = OriginSprite;
            this.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false); //不显示数量logo


        }
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite = cardMenu.selectedCards[i].sprite;
            this.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true); //显示logo
            Text numText=this.transform.GetChild(i).GetComponentInChildren<Text>();
            numText.text = cardMenu.cardNumDic[cardMenu.selectedCards[i]].ToString();
        }


        //for (int i = 0;i<cardMenu.selectedCards.Count;i++)
        //{
        //    if(i>this.transform.childCount-1)
        //    {
        //        Instantiate(CardShowPrefab, this.transform).GetComponent<Image>().sprite=cardMenu.selectedCards[i].sprite;
        //        continue;
        //    }
        //    this.transform.GetChild(i).GetComponent<Image>().sprite = cardMenu.selectedCards[i].sprite;
        //}

        //for(int i = cardMenu.selectedCards.Count;i<this.transform.childCount;i++) //删除多余的物体
        //{
        //    Destroy(this.transform.GetChild(i).gameObject);
        //}
    }

}
