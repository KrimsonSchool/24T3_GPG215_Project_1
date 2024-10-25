using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyStats)), DisallowMultipleComponent]
public class EnemyCombatHandler : MonoBehaviour
{
    [SerializeField] private GameObject floatingDamageNumberPrefab;
    private EnemyStats enemyStats;
    private float attackTimer = 0f;
    private bool isAttacking = false;
    private bool isAlive = true;
    private static readonly PlayerCombatStates[] playerDodgeStates = { PlayerCombatStates.DodgingRight, PlayerCombatStates.DodgingLeft, PlayerCombatStates.DodgingUp };

    /// <summary>
    /// &lt;Dodge state required avoid damage&gt;
    /// </summary>
    public static event Action<PlayerCombatStates> EnemyWindupEvent;
    /// <summary>
    /// &lt;Dodge state required avoid damage&gt;
    /// </summary>
    public static event Action<PlayerCombatStates> EnemyAttackWarningEvent;
    /// <summary>
    /// &lt;Damage Dealt, Dodge state required avoid damage&gt;
    /// </summary>
    public static event Action<int, PlayerCombatStates> EnemyAttackEvent;
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
            attackTimer = UnityEngine.Random.Range(0f, enemyStats.AttackSpeed * 0.25f);
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        PlayerCombatStates randomState = playerDodgeStates[UnityEngine.Random.Range(0, playerDodgeStates.Length)];
        EnemyWindupEvent?.Invoke(randomState);
        yield return new WaitForSeconds(Mathf.Clamp(enemyStats.AttackWindup - enemyStats.AttackWarning, 0, float.MaxValue));
        for (int i = 0; i < enemyStats.AttackCombos; i++)
        {
            Debug.LogWarning($"Enemy about to attack. Requires: {randomState}");
            EnemyAttackWarningEvent?.Invoke(randomState);
            yield return new WaitForSeconds(enemyStats.AttackWarning);
            EnemyAttackEvent?.Invoke(enemyStats.AttackDamage, randomState);
            yield return new WaitForSeconds(Mathf.Clamp(enemyStats.AttackComboSpeed - enemyStats.AttackWarning, 0, float.MaxValue));
            randomState = playerDodgeStates[UnityEngine.Random.Range(0, playerDodgeStates.Length)];
        }
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
        if (isAlive)
        {
            enemyStats.CurrentHealth = Mathf.Clamp(enemyStats.CurrentHealth - damage, 0, int.MaxValue);
            SpawnFloatingNumber(damage);
            if (enemyStats.CurrentHealth > 0)
            {
                Debug.Log($"{gameObject.name} took {damage} damage. [HP: {enemyStats.CurrentHealth}/{enemyStats.MaxHealth}]");
            }
            else
            {
                isAlive = false;
                StopAllCoroutines();
                Debug.Log($"{gameObject.name} died");
                EnemyDeadEvent?.Invoke();
            }
        }
    }

    private void SpawnFloatingNumber(int damageDone)
    {
        var prefab = Instantiate(floatingDamageNumberPrefab, transform.position, default);
        prefab.GetComponentInChildren<TextMeshProUGUI>().text = $"-{damageDone}HP";
    }

    private void FindReferences()
    {
        enemyStats = GetComponent<EnemyStats>();
    }
}
