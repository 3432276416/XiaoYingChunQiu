using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class CardMgr : MonoBehaviour
{
    public PoolTool poolTool;
    [SerializeField]List<CardDataSO> cardDataList; public List<HeroDataSO> heroDataList;
    public CardLibSO newGameCardLib;
    public CardLibSO curCardLib;
    public HeroLibSO newGameHeroLib;
    public HeroLibSO curHeroLib; //当前的好汉池
    int previousIndex = 0;
    private void OnEnable() {
        

        InitCardDataList();
        InitHeroDataList();
        InitCurLib();
       
    }

    private void OnDisable() {
        curCardLib.cardLibList.Clear();
        curHeroLib.heroLibList.Clear();
    }
    
    public void InitCurLib()
    {
        foreach (var item in newGameCardLib.cardLibList)
        {
            curCardLib.cardLibList.Add(item);
        }
        foreach (var item in newGameHeroLib.heroLibList) //寻找6张好汉
        {
            //if(curHeroLib.heroLibList.Count>6)
            //{
            //    break;
            //}
            curHeroLib.heroLibList.Add(item);
        }
    }
        

    #region 获取项目卡牌

    #region 初始化牌库和所有牌紫苑
    /// <summary>
    /// 初始化卡牌资源
    /// </summary>
    void InitCardDataList() //初始化卡牌资源列表
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData",null).Completed += OnCardDataLoaded;
    }
    void InitHeroDataList() //初始化英雄资源列表
    {
        Addressables.LoadAssetsAsync<HeroDataSO>("HeroData", null).Completed += OnHeroDataLoaded;
    }

    #endregion


    /// <summary>
    /// 回调函数
    /// </summary>
    /// <param name="handle"></param>
    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }else
        {
            Debug.LogError("No CardData Found!");
        }
    }
    private void OnHeroDataLoaded(AsyncOperationHandle<IList<HeroDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            heroDataList = new List<HeroDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("No CardData Found!");
        }
    }
    #endregion

    #region 卡牌库相关
    public GameObject GetCard()
{
    var card = poolTool.GetObjectFromPool();
    card.transform.localScale = Vector3.zero;
    return card;
}

public void DiscardCard(GameObject obj)
{
    poolTool.ReturnObjectFromPool(obj);
}

#endregion

#region 抽牌相关
    public CardDataSO GetNewCardData()
    {
        int randomIndex = 0;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, cardDataList.Count);
        } while (previousIndex == randomIndex);
        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    public HeroDataSO GetNewHeroData()
    {
        //int randomIndex = 0;
        //do
        //{
        //    randomIndex = UnityEngine.Random.Range(0, heroDataList.Count);
        //} while (previousIndex == randomIndex);
        //previousIndex = randomIndex;
        //return heroDataList[randomIndex];
        int randomIndex = UnityEngine.Random.Range(0, heroDataList.Count-1); ;

        while (heroDataList[randomIndex].heroSprite==null)
        {
            randomIndex= UnityEngine.Random.Range(0, heroDataList.Count - 1); 
        }

        return heroDataList[randomIndex];
    }

    public void UnlockCard(CardDataSO newData)
    {
        var newCard = new CardLibEntry{
            cardData = newData,
            amount = 1
        };

        var temp = curCardLib.cardLibList.Find(t => t.cardData == newData);
        if (temp.cardData != null)
        {
            newCard.amount = temp.amount+1;
            curCardLib.cardLibList.Remove(temp);
        }
        curCardLib.cardLibList.Add(newCard);
    }
#endregion
}
