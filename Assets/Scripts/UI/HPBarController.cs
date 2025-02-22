using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HPBarController : MonoBehaviour
{
    public CharacterBase curCharacter;
    [Header("Elements")]
    public Transform HpBarTransform;
    UIDocument HpBarDocument;
    ProgressBar HpBar;
    [Header("防御")]
    Label defenceCountLabel;
    VisualElement defenceElement;
    [Header("buff")]
    VisualElement buffRound;
    Label buffCount;
    public List<Sprite> spritesList;
    public Dictionary<BuffType,Sprite> spriteDict = new();
    [Header("Enm")]
    Enm enm;
    VisualElement intent;
    Label intentLabel;


    private void Awake() {
        HpBarDocument = GetComponent<UIDocument>();
        enm = GetComponent<Enm>();
        curCharacter = GetComponent<CharacterBase>();

        //为buff图片添加字典链接，编号请查看Enums
        foreach (var item in Enum.GetValues(typeof(BuffType)))
        {
            spriteDict.Add((BuffType)item,spritesList[(int)item]);
        }
    }

    void OnEnable()
    {
        InitHpBar();
    }
    private void Update() {
        UpdateHpBar();
    }
    public void MoveToWorldPos(VisualElement element,Vector3 worldPos,Vector2 size)
    {
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel,worldPos,size,Camera.main);

        HpBar.transform.position = rect.position;
        HpBar.highValue = curCharacter.maxHp;
    }

[ContextMenu("修正血条位置")]
    public void InitHpBar()
    {
        HpBar = HpBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");

        defenceElement = HpBarDocument.rootVisualElement.Q<VisualElement>("Defence");
        defenceCountLabel = defenceElement.Q<Label>("DefenceCount");
        defenceElement.style.display = DisplayStyle.None;

        buffRound = HpBarDocument.rootVisualElement.Q<VisualElement>("Buff");
        buffCount = buffRound.Q<Label>("BuffCount");
        buffRound.style.display = DisplayStyle.None;

        intent = HpBarDocument.rootVisualElement.Q<VisualElement>("Intent");
        intentLabel = intent.Q<Label>("IntentCount");
        intent.style.display = DisplayStyle.None;

        MoveToWorldPos(HpBar,HpBarTransform.position,Vector2.zero);
    }

    public void UpdateHpBar()
    {
        //防御回合更新
        defenceElement.style.display = curCharacter.defense > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        defenceCountLabel.text = curCharacter.defense.ToString();
        //buff回合更新

        //TODO:对于多buff显示的支持,需要配合界面实现
        buffRound.style.display = curCharacter.buffs.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        buffCount.text = curCharacter.buffs.Count > 0 ? curCharacter.buffs[0]?.buffRound.ToString() : "";

        buffRound.style.backgroundImage =
            curCharacter.baseStrength > 1 ? new StyleBackground(spriteDict[BuffType.Strength]) : new StyleBackground(spriteDict[BuffType.Weak]);

        if (curCharacter.isDead)
        {
            HpBar.style.display = DisplayStyle.None;
            return;
        }
        if (HpBar != null)
        {
            //HpBar.value = curCharacter.;
            //HpBar.title = $"{curCharacter.hp}/{curCharacter.maxHp}";

            //根据血量改变样式
            HpBar.RemoveFromClassList("highHealth");
            HpBar.RemoveFromClassList("midHealth");
            HpBar.RemoveFromClassList("lowHealth");

            //    //float percentage = curCharacter.hp / (float)curCharacter.maxHp;
            //    //Debug.Log(this+"=>"+percentage);
            //    if (percentage < 0.3f)
            //    {
            //        HpBar.AddToClassList("lowHealth");
            //    }
            //    else if (percentage < 0.6f)
            //    {
            //        HpBar.AddToClassList("midHealth");
            //    }
            //    else if (percentage >= 0.6f)
            //    {
            //        HpBar.AddToClassList("highHealth");
            //    }
            //}
        }
    }
/// <summary>
/// 事件函数，玩家回合开始时调用
/// </summary>
    public void SetIntentElement()
    {
        intent.style.display = DisplayStyle.Flex;
        //int value = enm.curAction.effect.value;
        //if (enm.curAction.effect.GetType() == typeof(DamageEffect))
        //{
        //    value = (int)math.round(value*enm.baseStrength);
        //}

        //intentLabel.text = value.ToString();

        //intent.style.backgroundImage = new StyleBackground(enm.curAction.intent);
    }
/// <summary>
/// 事件函数，敌方回合结束时调用
/// </summary>
    public void HideIntentElement()
    {
        intent.style.display = DisplayStyle.None;
    }
}
