using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayPanel : MonoBehaviour
{
    VisualElement visualElement;
    Label roundLabel, drawLabel, discardLabel,playerSpeedLabel,enmSpeedlabel;
    VisualElement[] MobilityArray = new VisualElement[6];
    VisualElement[] HpArray = new VisualElement[8];
    VisualElement[] PlayerSpeedArray = new VisualElement[3];
    VisualElement[] EnmSpeedArray = new VisualElement[3];
    Button endRoundBtn;
    [Header("广播")]
    public ObjectEventSO OnPlayerRoundEnd;
    private void OnEnable()
    {
        visualElement = GetComponent<UIDocument>().rootVisualElement;

        //ui获取
        roundLabel = visualElement.Q<Label>("RoundNum");
        drawLabel = visualElement.Q<Label>("DrawCount");
        discardLabel = visualElement.Q<Label>("DiscardCount");
        endRoundBtn = visualElement.Q<Button>("EndRoundBtn");
        playerSpeedLabel = visualElement.Q<Label>("PlayerSpeedLabel");
        enmSpeedlabel = visualElement.Q<Label>("EnmSpeedLabel");

        for (int i = 0; i < 6; i++)
        {
            MobilityArray[i] = visualElement.Q<VisualElement>($"Mobility{i + 1}");
        }
        for (int i = 0; i < 8; i++)
        {
            HpArray[i] = visualElement.Q<VisualElement>($"Hp{i+1}");
        }
        for (int i = 0; i < 6; i++)
        {
            if (i < 3)
            {
                PlayerSpeedArray[i] = visualElement.Q<VisualElement>($"Energy{i+1}");
            }
            else
            {
                EnmSpeedArray[i-3] = visualElement.Q<VisualElement>($"Energy{i+1}");
            }
        }
        endRoundBtn.clicked += OnRoundEnd;

        //改变显示数据
        roundLabel.text = drawLabel.text = discardLabel.text = 
        playerSpeedLabel.text = enmSpeedlabel.text = "0";
    }

    public void UpdateDrawDeckUI(int num)
    {
        if (drawLabel != null)
        {
            drawLabel.text = num.ToString();
        }
    }

    public void UpdateDiscardDeckUI(int num)
    {
        if (discardLabel != null)
        {
            discardLabel.text = num.ToString();
        }
    }

    public void UpdateEnergyUI(int num)
    {
        for (int i = 0; i < 6; i++)
        {
            if (i > num - 1)
            {
                MobilityArray[i].style.display = DisplayStyle.None;
            }
            else
            {
                MobilityArray[i].style.display = DisplayStyle.Flex;
            }
        }
    }

    public void UpdateHpUI(int hp)
    {
        Debug.Log(hp);
        for (int i = 0; i < 8; i++)
        {
            if (i > hp + 4)
            {
                HpArray[i].style.display = DisplayStyle.None;
            }
            else
            {
                HpArray[i].style.display = DisplayStyle.Flex;
            }
        }
    }
    public void UpdateRoundLabel(int num)
    {
        if (roundLabel != null)
        {
            roundLabel.text = $"回合:{num}";
        }
    }
    public void UpdatePlayerSpeed(int num)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i > num-1)
            {
                PlayerSpeedArray[i].style.display = DisplayStyle.None;
            }
            else
            {
                PlayerSpeedArray[i].style.display = DisplayStyle.Flex;
            }
        }

        playerSpeedLabel.text = num.ToString();
    }

    public void UpdateEnmSpeed(int num)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i > num - 1)
            {
                EnmSpeedArray[i].style.display = DisplayStyle.None;
            }
            else
            {
                EnmSpeedArray[i].style.display = DisplayStyle.Flex;
            }
        }
        
        enmSpeedlabel.text = num.ToString();
    }
    public void OnRoundEnd()
    {
        OnPlayerRoundEnd.RaiseEvent(null, this);
    }

    public void OnEnmTurnBegin()
    {
        endRoundBtn.SetEnabled(false);
    }
    public void OnPlayerTurnBegin()
    {
        endRoundBtn.SetEnabled(true);
        //Debug.Log(this + " ");
    }

}
