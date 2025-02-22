using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using DG.Tweening;
using System.Runtime.ConstrainedExecution;
using TMPro;

public class HeroMenu : MonoBehaviour
{
    #region UI部分

    public Button confirmBtn; //确认按钮
    public Button clearBtn;
    public Button returnToMenuBtn; //回到菜单按钮
    public Button showBtn;
    public GameObject scrollView;
    public GameObject selecView;
    public Tweener viewTwe;
    public Text heroText;
    public Text passvieText;
    public Text costText;
    public Text unlockText;


    #endregion

    public bool isShow; //是否被收纳
    public HeroLibSO allHeroLib;  //所有英雄库
    public HeroLibSO newGameHeroLib; 
    public List<HeroDataSO> selectedHeros;
    public Transform contentTrs; //卡牌内容存放位置
    public GameObject contentPrefab; //一行的预制体，3个卡牌展示

    public ObjectEventSO LoadCardMenuEvent; //加载选择卡牌
    public ObjectEventSO UpdateSelectedCardEvent; //更新已选择菜单
    public ObjectEventSO LoadMenuEvent; //加载主菜单
    public HeroFormationConfigSO formationConfig;
    public int rowCnt;
    int selectHeroNum; //需要选择的英雄  

    public void OnEnable()
    {
        viewTwe = scrollView.transform.DOMoveX(-150f, 1f);
        viewTwe.SetAutoKill(false);// 不让Do方法返回的Tweener对象自动销毁，默认情况下是自动销毁的。
        viewTwe.Pause();
        isShow = true;
        formationConfig = new HeroFormationConfigSO();
        formationConfig.heroDataList.Capacity = 6;
        if (allHeroLib == null) return;
        selectedHeros=new List<HeroDataSO>();
        confirmBtn.onClick.AddListener(ConfirmLib);
        showBtn.onClick.AddListener(ShowMenu);
        returnToMenuBtn.onClick.AddListener(() =>
        {
            
            LoadMenuEvent.RaiseEvent(this, this); //加载菜单
        });
        clearBtn.onClick.AddListener(() =>
        {
            
            selectedHeros.Clear(); //清空已经选择的卡牌
            UpdateSelectedCardEvent.RaiseEvent(this, this);

        });
        InitSelecMenu();
    }


    public void InitSelecMenu()
    {
      for (int i = 0; i < allHeroLib.heroLibList.Count; i++)
        {
           var item=Instantiate(contentPrefab, contentTrs);
              HeroContentBtn btn =item.gameObject.GetComponent<HeroContentBtn>();
                btn.Init(allHeroLib.heroLibList[i].heroData);
            clearBtn.onClick.AddListener(() =>
            {
                btn.isSelect = false;
                btn.UpdateButton();
            });

           
        }
    }

    /// <summary>
    /// 确认完成就添加牌库
    /// </summary>
    public void ConfirmLib()
    {
        if (selectedHeros.Count!=6)
        {
            return;
        }


        foreach(var item in selectedHeros)
        {
            HeroLibEntry entry= new HeroLibEntry();
            entry.amount = 1;
            entry.heroData = item;
            newGameHeroLib.heroLibList.Add(entry);

            formationConfig.heroDataList.Add(item);
        }

        
        LoadCardMenuEvent.RaiseEvent(this, this);
    }


    public void ShowMenu()
    {
        

    Debug.Log("切换视图");
        if(isShow)
        {
            isShow = false;
            scrollView.transform.DOPlayForward();
            
        }
        else if(!isShow)
        {
            isShow = true;
            //scrollView.transform.DOMoveX(400f, 1f);
           scrollView.transform.DOPlayBackwards();
          
        }
    }
    /// <summary>
    /// 监听选择
    /// </summary>
    /// <param name="card"></param>
    public void OnCardMenuSelect(object selec)
    {
       
        if(selectedHeros.Count>=6)
        {
            return;
        }

        HeroContentBtn btn = selec as HeroContentBtn;
        HeroDataSO data=btn.heroData;
        if (data == null) return;

       
        if (!btn.isSelect)
        {
            selectedHeros.Add(data);

           
        }
        else
        {
            selectedHeros.Remove(data);
        }
      
        UpdateSelecView(data);
        UpdateSelectedCardEvent.RaiseEvent(this, this);
      
    }

    public void UpdateSelecView(HeroDataSO data) //显示当前选中的好汉牌
    {
        selecView.SetActive(true);
        selecView.GetComponent<Image>().sprite = data.heroSprite; //更新
        heroText.text=data.Title+"--"+data.Name;
        costText.text = data.costSkillName;
        passvieText.text = data.passiveSkillName;
        unlockText.text = data.unlockSkillName;
        
    }

}
