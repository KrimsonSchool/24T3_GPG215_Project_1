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
        PlayerCombatHandler.PlayerDodgeRight += PlayerDodgeRightAnimation;
        PlayerCombatHandler.PlayerDodgeLeft += PlayerDodgeLeftAnimation;
        PlayerCombatHandler.PlayerDodgeUp += PlayerDodgeUpAnimation;
        PlayerCombatHandler.PlayerBlockStart += PlayerBlockAnimation;
        PlayerCombatHandler.PlayerBlockEnd += PlayerBlockEndAnimation;
    }

    private void OnDisable()
    {
        PlayerCombatHandler.PlayerAttackStart -= PlayerAttackAnimation;
        PlayerCombatHandler.PlayerDodgeRight -= PlayerDodgeRightAnimation;
        PlayerCombatHandler.PlayerDodgeLeft -= PlayerDodgeLeftAnimation;
        PlayerCombatHandler.PlayerDodgeUp -= PlayerDodgeUpAnimation;
        PlayerCombatHandler.PlayerBlockStart -= PlayerBlockAnimation;
        PlayerCombatHandler.PlayerBlockEnd -= PlayerBlockEndAnimation;
    }

    private void PlayerAttackAnimation()
    {
        animator.Play("PlayerAttack");
    }

    private void PlayerDodgeRightAnimation()
    {
        animator.Play("PlayerDodgeRight");
    }

    private void PlayerDodgeLeftAnimation()
    {
        animator.Play("PlayerDodgeLeft");
    }

    private void PlayerDodgeUpAnimation()
    {
        animator.Play("PlayerDodgeUp");
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
