using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationLinks : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        PlayerCombatHandler.PlayerAttackStart += PlayerAttackAnimation;
        PlayerCombatHandler.PlayerDodging += PlayerDodgeAnimation;
        PlayerCombatHandler.PlayerBlockStart += PlayerBlockAnimation;
        PlayerCombatHandler.PlayerBlockEnd += PlayerBlockEndAnimation;
    }

    private void OnDisable()
    {
        PlayerCombatHandler.PlayerAttackStart -= PlayerAttackAnimation;
        PlayerCombatHandler.PlayerDodging -= PlayerDodgeAnimation;
        PlayerCombatHandler.PlayerBlockStart -= PlayerBlockAnimation;
        PlayerCombatHandler.PlayerBlockEnd -= PlayerBlockEndAnimation;
    }

    private void PlayerAttackAnimation()
    {
        animator.Play("PlayerAttack");
    }

    private void PlayerDodgeAnimation(PlayerCombatStates state)
    {
        switch (state)
        {
            case PlayerCombatStates.DodgingRight:
                animator.Play("PlayerDodgeRight");
                break;
            case PlayerCombatStates.DodgingLeft:
                animator.Play("PlayerDodgeLeft");
                break;
            case PlayerCombatStates.DodgingUp:
                animator.Play("PlayerDodgeUp");
                break;
        }
    }

    private void PlayerBlockAnimation()
    {
        animator.Play("PlayerBlock");
    }

    private void PlayerBlockEndAnimation()
    {
        animator.Play("PlayerBlockEnd");
    }
}
