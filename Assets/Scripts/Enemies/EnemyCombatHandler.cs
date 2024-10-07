using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyStats)), DisallowMultipleComponent]
public class EnemyCombatHandler : MonoBehaviour
{
    private EnemyStats enemyStats;
    private float attackTimer = 0f;
    private bool isAttacking = false;
    private bool isAlive = true;
    private PlayerCombatStates[] defensivePlayerStates = {PlayerCombatStates.DodgingRight, PlayerCombatStates.DodgingLeft, PlayerCombatStates.DodgingUp};

    public static event Action<PlayerCombatStates> EnemyWindupEvent; // T1 = Attack type/defence state required from player
    public static event Action<int, PlayerCombatStates> EnemyAttackEvent; // T1 = Damage dealt, T2 = Attack type/defence state required from player
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
        PlayerCombatStates randomState = defensivePlayerStates[UnityEngine.Random.Range(0, defensivePlayerStates.Length)];
        EnemyWindupEvent?.Invoke(randomState);
        Debug.LogWarning($"Enemy attack winding up. Requires: {randomState}");
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
            Debug.Log($"{gameObject.name} took {damage} damage. [HP: {enemyStats.CurrentHealth}/{enemyStats.MaxHealth}]");
        }
        else
        {
            if (isAlive)
            {
                isAlive = false;
                StopAllCoroutines();
                Debug.Log($"{gameObject.name} died");
                EnemyDeadEvent?.Invoke();
            }
        }
    }

    private void FindReferences()
    {
        enemyStats = GetComponent<EnemyStats>();
    }
}
