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
    }

    private void OnDisable()
    {
        EnemyCombatHandler.EnemyDeadEvent -= EnemyDeadAnimation;
        EnemyCombatHandler.EnemyAttackEvent -= EnemyAttackAnimation;
    }

    private void EnemyAttackAnimation(int damage, PlayerCombatHandler.PlayerStates defenceRequirement)
    {
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

    private void SignalDead()
    {

    }
}
