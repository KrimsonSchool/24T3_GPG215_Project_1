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
    [SerializeField] private float minAttackSpeedMultiplier = 0.7f;
    [SerializeField] private float firstAttackDelay = 1f;
    private bool isAttacking = false;
    private bool isAlive = true;
    private static readonly PlayerCombatStates[] playerDodgeStates = { PlayerCombatStates.DodgingRight, PlayerCombatStates.DodgingLeft, PlayerCombatStates.DodgingUp };

    /// <summary>
    /// 1. &lt;PlayerCombatStates&gt; : StateToDodge
    /// </summary>
    public static event Action<PlayerCombatStates> AttackWindingUp;

    /// <summary>
    /// 1. &lt;PlayerCombatStates&gt; : StateToDodge
    /// </summary>
    public static event Action<PlayerCombatStates> StartingAttackWarning;

    /// <summary>
    /// 1. &lt;int&gt; : Damage <br></br>
    /// 2. &lt;PlayerCombatStates&gt; : StateToDodge
    /// </summary>
    public static event Action<int, PlayerCombatStates> EnemyAttacked;

    public static event Action<int> EnemyDamaged;

    private void Awake()
    {
        FindReferences();
    }

    private void Start()
    {
        attackTimer = firstAttackDelay;
    }

    protected virtual void FindReferences()
    {
        enemyStats = GetComponent<EnemyStats>();
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

        if (attackTimer <= 0f)
        {
            StartCoroutine(Attack());
        }
    }

    protected virtual IEnumerator Attack()
    {
        isAttacking = true;
        attackTimer = UnityEngine.Random.Range(enemyStats.AttackSpeed * minAttackSpeedMultiplier, enemyStats.AttackSpeed);
        PlayerCombatStates randomState = playerDodgeStates[UnityEngine.Random.Range(0, playerDodgeStates.Length)];
        AttackWindingUp?.Invoke(randomState);
        yield return new WaitForSeconds(enemyStats.AttackWindup);
        for (int i = 0; i < enemyStats.AttackCombos; i++)
        {
            //Debug.LogWarning($"Enemy about to attack. Requires: {randomState}");
            StartingAttackWarning?.Invoke(randomState);
            yield return new WaitForSeconds(enemyStats.AttackWarning);
            EnemyAttacked?.Invoke(enemyStats.AttackDamage, randomState);
            if (attackClip != null)
            {
                AudioManager.Instance.PlaySoundEffect2D(attackClip, 1, UnityEngine.Random.Range(0.9f, 1.1f));
            }
            if (enemyStats.AttackCombos > 1)
            {
                randomState = playerDodgeStates[UnityEngine.Random.Range(0, playerDodgeStates.Length)];
                yield return new WaitForSeconds(enemyStats.AttackComboSpeed);
            }
        }
        isAttacking = false;
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
