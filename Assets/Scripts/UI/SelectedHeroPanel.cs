using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedHeroPanel : MonoBehaviour
{
    public HeroMenu cardMenu;
    public Sprite originSprite;

    public void UpdateSelec()
    {
        Debug.Log("¸üÐÂÑ¡Ôñ");

        for (int i = 0; i < this.transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = originSprite;
        }
        for (int i = 0;i<this.transform.childCount;i++)
        {
            if (i >= cardMenu.selectedHeros.Count) break;
            transform.GetChild(i).GetComponent<Image>().sprite = cardMenu.selectedHeros[i].heroSprite;
        }
    }
}
