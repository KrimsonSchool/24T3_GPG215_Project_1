using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        animator.SetBool("Windup", false);
        // can put in different animations depending on attack type
        animator.SetBool("Attack", true);
    }

    private void EnemyDeadAnimation()
    {
        animator.SetBool("Dead", true);
    }

    private void AttackEnd()
    {
        animator.SetBool("Attack", false);
    }

    private void WindupStart(PlayerCombatStates defenceRequirement)
    {
        animator.SetBool("Windup", true);
    }

    private void SignalDead()
    {

    }
}
