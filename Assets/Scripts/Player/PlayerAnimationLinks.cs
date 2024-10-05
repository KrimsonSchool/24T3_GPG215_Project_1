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
        PlayerCombatHandler.PlayerBlockStart += PlayerBlockAnimation;
    }

    private void OnDisable()
    {
        PlayerCombatHandler.PlayerAttackStart -= PlayerAttackAnimation;
        PlayerCombatHandler.PlayerDodgeRight -= PlayerDodgeRightAnimation;
        PlayerCombatHandler.PlayerDodgeLeft -= PlayerDodgeLeftAnimation;
        PlayerCombatHandler.PlayerBlockStart -= PlayerBlockAnimation;
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

    private void PlayerBlockAnimation()
    {
        animator.Play("PlayerBlock");
    }
}
