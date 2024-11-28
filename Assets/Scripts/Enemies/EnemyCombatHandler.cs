using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats)), DisallowMultipleComponent]
public class EnemyCombatHandler : MonoBehaviour
{
    [SerializeField] private AudioClip attackClip;
    private EnemyStats enemyStats;
    private float attackTimer = 0f;
    [SerializeField] private List<WeightedItem<EnemyAttack>> attackPool = new();
    [SerializeField] private float minAttackSpeedMultiplier = 0.7f;
    [SerializeField] private float firstAttackDelay = 1f;
    private bool isAttacking = false;
    private bool isAlive = true;
    private static readonly PlayerCombatStates[] playerDodgeStates = { PlayerCombatStates.DodgingRight, PlayerCombatStates.DodgingLeft, PlayerCombatStates.DodgingUp };

    #region Events
    /// <summary>
    /// 1. &lt;PlayerCombatStates&gt; : StateToDodge
    /// </summary>
    public static event Action<PlayerCombatStates> AttackWindingUp;
    public static void OnAttackWindingUp(PlayerCombatStates attackDirection)
    {
        AttackWindingUp?.Invoke(attackDirection);
    }

    /// <summary>
    /// 1. &lt;PlayerCombatStates&gt; : StateToDodge
    /// </summary>
    public static event Action<PlayerCombatStates> StartingAttackWarning;
    public static void OnStartingAttackWarning(PlayerCombatStates attackDirection)
    {
        StartingAttackWarning?.Invoke(attackDirection);
    }

    /// <summary>
    /// 1. &lt;int&gt; : Damage <br></br>
    /// 2. &lt;PlayerCombatStates&gt; : StateToDodge
    /// </summary>
    public static event Action<int, PlayerCombatStates> EnemyAttacked;
    public static void OnEnemyAttacked(int damage, PlayerCombatStates attackDirection)
    {
        EnemyAttacked?.Invoke(damage, attackDirection);
    }

    /// <summary>
    /// &lt;int&gt; : EnemyDamageTaken
    /// </summary>
    public static event Action<int> EnemyDamaged;
    #endregion

    private void Awake()
    {
        FindReferences();
    }

    protected virtual void FindReferences()
    {
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        attackTimer = firstAttackDelay;
    }

    protected virtual void OnEnable()
    {
        PlayerCombatHandler.PlayerAttacked += TakeDamage;
    }

    protected virtual void OnDisable()
    {
        PlayerCombatHandler.PlayerAttacked -= TakeDamage;
    }

    private void Update()
    {
        EnemyAttackLogic();
    }

    private void EnemyAttackLogic()
    {
        if (!isAttacking && isAlive)
        {
            attackTimer -= Time.deltaTime;
        }

        if (attackTimer <= 0f && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    protected virtual IEnumerator Attack()
    {
        isAttacking = true;
        yield return StartCoroutine(GetRandomAttack(attackPool).Attack(enemyStats.AttackWindup, enemyStats.AttackWarning, enemyStats.AttackDamage, attackClip));
        attackTimer = UnityEngine.Random.Range(enemyStats.AttackSpeed * minAttackSpeedMultiplier, enemyStats.AttackSpeed);
        isAttacking = false;
    }

    private EnemyAttack GetRandomAttack(List<WeightedItem<EnemyAttack>> weightedList)
    {
        var totalWeight = 0f;
        foreach (var item in weightedList)
        {
            totalWeight += item.Weight;
        }
        var randomWeight = UnityEngine.Random.Range(0, totalWeight);
        var processedWeight = 0f;
        foreach (var item in weightedList)
        {
            processedWeight += item.Weight;
            if (processedWeight >= randomWeight)
            {
                return item.Item;
            }
        }
        print("Random weight was higher than total weight");
        return null;
    }

    public void TakeDamage(int damage)
    {
        if (isAlive)
        {
            enemyStats.CurrentHealth = Mathf.Clamp(enemyStats.CurrentHealth - damage, 0, int.MaxValue);
            EnemyDamaged?.Invoke(damage);
            if (enemyStats.CurrentHealth > 0)
            {
                //Debug.Log($"{gameObject.name} took {damage} damage. [HP: {enemyStats.CurrentHealth}/{enemyStats.MaxHealth}]");
            }
            else
            {
                isAlive = false;
                StopAllCoroutines();
                //Debug.Log($"{gameObject.name} died");
            }
        }
    }
}
