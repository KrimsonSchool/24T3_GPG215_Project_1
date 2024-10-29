using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationLinks : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EnemyCombatHandler.EnemyDeadEvent += EnemyDeadAnimation;
        EnemyCombatHandler.EnemyAttackEvent += EnemyAttackAnimation;
        EnemyCombatHandler.EnemyWindupEvent += WindupStart;
    }

    private void OnDisable()
    {
        EnemyCombatHandler.EnemyDeadEvent -= EnemyDeadAnimation;
        EnemyCombatHandler.EnemyAttackEvent -= EnemyAttackAnimation;
        EnemyCombatHandler.EnemyWindupEvent -= WindupStart;
    }

    private void EnemyAttackAnimation(int damage, PlayerCombatStates defenceRequirement)
    {
        //animator.SetBool("Windup", false);
        //// can put in different animations depending on attack type
        //animator.SetBool("Attack", true);

        animator.Play("Slime_Attack");
    }

    private void EnemyDeadAnimation()
    {
        //animator.SetBool("Dead", true);

        animator.Play("Slime_Die");
    }

    private void AttackEnd()
    {
        animator.SetBool("Attack", false);
    }

    private void WindupStart(PlayerCombatStates defenceRequirement)
    {
        //animator.SetBool("Windup", true);

        animator.Play("Slime_Windup");
    }

    private void SignalDead()
    {

    }
}
