using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Hero player;
    Animator animator;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        player = GetComponent<Hero>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        animator.Play("sleep");
        animator.SetBool("isSleep", true);
    }

    public void PlayerTurnBeginAni()
    {
        animator.SetBool("isSleep",false);
        animator.SetBool("isParry",false);
    }

    public void PlayerTurnEndAni()
    {
        
    }
    public void PlayerAttackEvent(object obj)
    {
        Card card= obj as Card;

        switch (card.cardData.type)
        {
            case CardType.Item:
                animator.SetTrigger("attack");
                break;
            case CardType.Hero:
                break;
            case CardType.Soldier:
                animator.SetTrigger("skill");
                break;
            default:
                break;
        }
    }

    public void SetSleepAni()
    {
        animator.Play("death");
    }
}
