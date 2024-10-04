using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats)), DisallowMultipleComponent]
public class EnemyCombatHandler : MonoBehaviour
{
    private EnemyStats enemyStats;
    private float attackTimer = 0f;
    private bool isAttacking = false;
    private bool isAlive = true;
    private PlayerCombatHandler.PlayerStates[] defensivePlayerStates = {PlayerCombatHandler.PlayerStates.DodgingRight, PlayerCombatHandler.PlayerStates.DodgingLeft, PlayerCombatHandler.PlayerStates.Blocking};

    public delegate void EnemyAttackDelegate(int damage, PlayerCombatHandler.PlayerStates defenceRequirement);
    public static event EnemyAttackDelegate EnemyAttackEvent;
    public static event Action EnemyDeadEvent;

    private void Awake()
    {
        FindReferences();
    }

    private void Update()
    {
        EnemyAttackLogic();
    }

    private void EnemyAttackLogic()
    {
        if (!isAttacking && isAlive)
        {
            attackTimer += Time.deltaTime;
        }
        if (attackTimer >= enemyStats.AttackSpeed)
        {
            isAttacking = true;
            attackTimer = 0f;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        PlayerCombatHandler.PlayerStates randomState = defensivePlayerStates[UnityEngine.Random.Range(0, defensivePlayerStates.Length)];
        Debug.Log($"Enemy attack winding up. Requires: {randomState}");
        yield return new WaitForSeconds(enemyStats.AttackWindup);
        EnemyAttackEvent?.Invoke(enemyStats.BaseAttackDamage, randomState);
        isAttacking = false;
    }

    private void OnEnable()
    {
        PlayerCombatHandler.PlayerAttackEvent += TakeDamage;
    }

    private void OnDisable()
    {
        PlayerCombatHandler.PlayerAttackEvent -= TakeDamage;
    }

    private void TakeDamage(int damage)
    {
        enemyStats.CurrentHealth = Mathf.Clamp(enemyStats.CurrentHealth - damage, 0, int.MaxValue);
        if (enemyStats.CurrentHealth > 0)
        {
            Debug.Log($"{damage} damage dealt to enemy. {enemyStats.CurrentHealth}/{enemyStats.MaxHealth} enemy health remaining.");
        }
        else
        {
            Debug.Log("it dieded");
            isAlive = false;
            StopAllCoroutines();
            EnemyDeadEvent?.Invoke();
        }
    }

    private void FindReferences()
    {
        enemyStats = GetComponent<EnemyStats>();
    }
}
