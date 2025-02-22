using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class CardMenu : MonoBehaviour
{
    #region UI部分
    public Transform contentTrs; //卡牌内容存放位置
    public Button confirmBtn; //确认按钮
    public Button addBtn; //加牌按钮
    public Button reduceBtn; //减牌
    public Button toHeroMenuBtn; //返回英雄选择界面
    public Text tolNumText;  //总共选了的文字
    public Text cardNumText; //当前牌的文字
    public Text nameText; //名字文字
    public Text costText; //消耗
    public Text descriptionText; //描述
    public Button clearBtn;
    public Button foldBtn; //收纳选择菜单按钮
    public Tween foldTwe;
    public GameObject scrollView;
    public GameObject cardBtnPrefab; //一行的预制体，3个卡牌展示
    public Image showCardImage; //展示选中的卡牌
    #endregion

    public bool isFold; //是否被折叠了
    public int cardNum; //目前的卡牌数量，需要24
    public CardDataSO curCardData; //当前数据
    public CardContentBtn curCardBtn; //当前点击的
    public CardLibSO allCardLib;  //所有英雄库
    public CardLibSO newGameCardLib; //新游戏的牌库
    public List<CardDataSO> selectedCards; //选择的卡牌
    public Dictionary<CardDataSO, int> cardNumDic; //用来查询选了多少张牌

    public ObjectEventSO NewGameEvent; //加载地图
    public ObjectEventSO LoadHeroMenuEvent; //加载好汉选择
    public ObjectEventSO UpdateSelectedCardEvent; //更新已选择菜单
    public int rowCnt;
    public int tolNum; //当前已经选择了的总和
    int selectHeroNum; //需要选择的英雄  

    public void OnEnable()
    {


        isFold = false;
          foldTwe = scrollView.transform.DOMoveX(-140f, 1f);
        foldTwe.SetAutoKill(false);// 不让Do方法返回的Tweener对象自动销毁，默认情况下是自动销毁的。
        foldTwe.Pause();
        cardNumDic = new Dictionary<CardDataSO, int>();
        selectedCards = new List<CardDataSO>();

        #region 初始化按钮
        confirmBtn.onClick.AddListener(ConfirmLib);
        foldBtn.onClick.AddListener(() =>
        {
            if (isFold)
            {
                scrollView.transform.DOPlayBackwards();
                isFold = false;
            }
            else
            {
                scrollView.transform.DOPlayForward();
                isFold = true;
            }

        });
        toHeroMenuBtn.onClick.AddListener(() =>
        {
            LoadHeroMenuEvent.RaiseEvent(this, this);
        });
        clearBtn.onClick.AddListener(() =>
        {
            ClearMenu();
        });
        #endregion

        InitSelecMenu();
        
    }

 

    public void ClearMenu()
    {
        selectedCards.Clear();
        for(int i = 0;i<contentTrs.childCount;i++)
        {
            Destroy(contentTrs.GetChild(i).gameObject);
        }
        tolNum = 0;
        tolNumText.text = "0 / 24";
        InitSelecMenu();
        UpdateSelectedCardEvent.RaiseEvent(this, this);
    }
    /// <summary>
    /// 初始化函数
    /// </summary>
    public void InitSelecMenu()
    {
       
        addBtn.onClick.AddListener(AddNum);
        reduceBtn.onClick.AddListener(ReduceNum);

        foreach(var item in allCardLib.cardLibList)
        {
            CardContentBtn Btn=Instantiate(cardBtnPrefab, contentTrs).GetComponent<CardContentBtn>();
            Btn.Init(item.cardData);
            cardNumDic[item.cardData] = 0;
        }
    }

    /// <summary>
    /// 确认完成就添加牌库
    /// </summary>
    public void ConfirmLib()
    {

        foreach (var item in selectedCards)
        {
            CardLibEntry entry = new CardLibEntry();
            entry.amount = cardNumDic[item];
            entry.cardData = item;
            newGameCardLib.cardLibList.Add(entry);
        }
        NewGameEvent.RaiseEvent(this, this);
    }

    /// <summary>
    /// 两个加减按钮函数
    /// </summary>
    public void AddNum()
    {
        cardNumDic[curCardData]++;
        tolNum++;
       UpdateText();
        if (cardNumDic[curCardData]>0&&!selectedCards.Contains(curCardData))
        {
            selectedCards.Add(curCardData);
        }
        UpdateSelectedCardEvent.RaiseEvent(this, this);
    }

    public void ReduceNum()
    {
        cardNumDic[curCardData]--;
        tolNum--;

        UpdateText();
        if (cardNumDic[curCardData] <= 0 && selectedCards.Contains(curCardData))
        {
            selectedCards.Remove(curCardData);
        }
        UpdateSelectedCardEvent.RaiseEvent(this, this);
    }

    public void UpdateText()
    {
        cardNumText.text = cardNumDic[curCardData].ToString();
        curCardBtn.numText.text = cardNumDic[curCardData].ToString();
        tolNumText.text=tolNum.ToString()+" / 24";
    }

    /// <summary>
    /// 监听选择
    /// </summary>
    /// <param name="card"></param>
    public void OnCardSelect(object selec)
    {
        Debug.Log("有卡牌被选中");

        CardContentBtn btn= selec as CardContentBtn;
        curCardBtn= btn;
        CardDataSO data = btn.cardData;
        curCardData = data;
        showCardImage.gameObject.SetActive(true);
        showCardImage.sprite = data.sprite;
        cardNumText.text = cardNumDic[data].ToString();
        nameText.text = data.cardName;
        costText.text=data.cost.ToString();
        descriptionText.text = data.description.ToString();

        if (data == null) return;

        UpdateSelectedCardEvent.RaiseEvent(this, this);
    
    }
}
