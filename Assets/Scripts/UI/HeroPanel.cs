using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/// <summary>
/// 挂载好汉的界面脚本
/// </summary>
public class HeroPanel : MonoBehaviour
{
    public List<GameObject> heroList = new();

    [Header("广播")]
    public ObjectEventSO HeroDeadEvent;
    public Transform Hero;
    public Transform SupportTrs1;
    public Transform SupportTrs2;

    //public void AddHeroToStage(GameObject hero)
    //{
    //    hero.transform.SetParent(StageTrs);
    //}


    //public void Awake()
    //{
    //    Instantiate(HeroPrefab, StageTrs);

    //    SetStageArea();

    //}

    //public void ClearDeadHero() //HeroDeadEvent监听事件
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        Hero hero = transform.GetChild(i).GetComponent<Hero>();
    //        if (hero == null) continue;
    //        if (hero.curHp <= 0)
    //        {
    //            Destroy(hero.gameObject);
    //        }
    //    }
    //}


    //public void SetStageArea()
    //{
    //    if (StageTrs?.childCount > 2)
    //        return;
    //    if (StageTrs?.childCount == 1)
    //    {
    //        GameObject hero = StageTrs.GetChild(0).gameObject;
    //        hero.transform.localPosition = new Vector3(0.09f, -0.55f, 0);
    //        hero.transform.DOScale(new Vector3(2.4f, 2f, .04f), 1f);
    //    }

    //    if (StageTrs?.childCount == 2)
    //    {
    //        GameObject hero1 = StageTrs.GetChild(0).gameObject;
    //        GameObject hero2 = StageTrs.GetChild(1).gameObject;
    //        hero1?.transform.DOScale(new Vector3(2.4f, 1.3f, 0.039f), 1f);
    //        hero2?.transform.DOScale(new Vector3(2.4f, 1.3f, 0.039f), 1f);
    //        hero1.transform.localPosition = new Vector3(0.17f, 1.42f, 0);
    //        hero2.transform.localPosition = new Vector3(0.23f, -1.53f, 0);
    //    }

    //}


}
