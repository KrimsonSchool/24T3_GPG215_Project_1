using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Declared outside of class for ease of use
public enum PlayerCombatStates { Idle, Attacking, DodgingRight, DodgingLeft, DodgingUp, Blocking, Recovering }

[RequireComponent(typeof(PlayerStats)), DisallowMultipleComponent]
public class PlayerCombatHandler : Singleton<PlayerCombatHandler>
{
    #region References
    private PlayerStats playerStats;
    [SerializeField] private GameObject floatingDamageNumberPrefab;
    #endregion

    #region Fields
    private PlayerCombatStates currentPlayerState = PlayerCombatStates.Idle;
    private float recoveryTimer = 0f;
    private float attackPreventionTimer = 0f;
    private float inputQueueLeeway = 0.4f;
    private float attackInputQueueLeeway = 0.1f;
    private bool inputQueued = false;
    private bool declineInput = false;
    private bool inDodgeRecovery = false;

    // Set these up for use in future to work with player stats/gear better
    //private float baseAttackSpeed = 0.2f;
    //private float baseAttackRecovery = 0.4f;
    //private float baseDodgeWindow = 0.4f;
    //private float baseDodgeRecovery = 0.4f;
    //private float baseBlockRecovery = 0.4f;
    #endregion

    #region Properties
    public PlayerCombatStates CurrentPlayerState { get { return currentPlayerState; } }
    #endregion

    #region Events
    /// <summary>
    /// 1. &lt;int&gt; : damageAfterResistances <br></br>
    /// 2. &lt;bool&gt; : hasBlocked
    /// </summary>
    public static event Action<int, bool> PlayerDamaged;
    /// <summary>
    /// 1. &lt;int&gt; : PlayerStats.AttackDamage
    /// </summary>
    public static event Action<int> PlayerAttacked;

    // could refactor these into params too
    public static event Action PlayerAttackStart;
    public static event Action PlayerBlockStart;
    public static event Action PlayerBlockEnd;

    public static event Action<PlayerCombatStates> PlayerDodging;
    #endregion

    #region Initialization & Decommission
    protected override void Awake()
    {
        base.Awake();
        FindReferneces();
    }

    private void FindReferneces()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        PlayerCombatControls.PlayerControlInput += OnPlayerInput;
        EnemyCombatHandler.EnemyAttacked += DamagePlayer;
        EnemyStats.EnemyDied += LockCombat;
        GameManager.RoomLevelChanged += UnlockCombat;
    }

    private void OnDisable()
    {
        PlayerCombatControls.PlayerControlInput -= OnPlayerInput;
        EnemyCombatHandler.EnemyAttacked -= DamagePlayer;
        EnemyStats.EnemyDied -= LockCombat;
        GameManager.RoomLevelChanged -= UnlockCombat;
    }
    #endregion

    private void Update()
    {
        HandleTimers();
    }

    private void HandleTimers()
    {
        if (recoveryTimer > 0f)
        {
            recoveryTimer -= Time.deltaTime;
        }
        else
        {
            inDodgeRecovery = false;
            if (currentPlayerState == PlayerCombatStates.Recovering && !inputQueued)
            {
                currentPlayerState = PlayerCombatStates.Idle;
            }
        }

        if (attackPreventionTimer > 0f)
        {
            attackPreventionTimer -= Time.deltaTime;
        }
    }

    private void OnPlayerInput(CombatInputs input)
    {
        if (!declineInput)
        {
            if (input == CombatInputs.Release)
            {
                EndBlock();
            }
            else if (!inputQueued && (currentPlayerState == PlayerCombatStates.Idle || (currentPlayerState == PlayerCombatStates.Recovering && recoveryTimer <= inputQueueLeeway)))
            {
                if (input == CombatInputs.Tap && attackPreventionTimer <= 0f && recoveryTimer <= attackInputQueueLeeway)
                {
                    inputQueued = true;
                    StartCoroutine(Attack());
                }
                else if (input == CombatInputs.SwipeRight)
                {
                    inputQueued = true;
                    StartCoroutine(Dodge(PlayerCombatStates.DodgingRight));
                }
                else if (input == CombatInputs.SwipeLeft)
                {
                    inputQueued = true;
                    StartCoroutine(Dodge(PlayerCombatStates.DodgingLeft));
                }
                else if (input == CombatInputs.SwipeUp)
                {
                    inputQueued = true;
                    StartCoroutine(Dodge(PlayerCombatStates.DodgingUp));
                }
                else if (input == CombatInputs.SwipeDown && recoveryTimer <= 0)
                {
                    StartCoroutine(Block());
                }
            }
        }
    }

    private void LockCombat()
    {
        StopAllCoroutines();
        declineInput = true;
        inDodgeRecovery = false;
        inputQueued = false;
    }

    private void UnlockCombat(int roomLevel)
    {
        declineInput = false;
    }

    private IEnumerator Attack()
    {
        while (recoveryTimer > 0f) { yield return null; }
        inDodgeRecovery = false;
        //Debug.Log("ATACKING!");
        currentPlayerState = PlayerCombatStates.Attacking;
        PlayerAttackStart?.Invoke();
        recoveryTimer = playerStats.AttackRecovery + playerStats.AttackSpeed;
        yield return new WaitForSeconds(playerStats.AttackSpeed);
        FindObjectOfType<FxPlayer>().PlaySound("PlayerAttack");
        PlayerAttacked?.Invoke(playerStats.AttackDamage);
        currentPlayerState = PlayerCombatStates.Recovering;
        inputQueued = false;
    }

    private IEnumerator Dodge(PlayerCombatStates dodgeState)
    {
        while (recoveryTimer > 0f) { yield return null; }
        //Debug.Log($"DODGING! {dodgeState}");
        currentPlayerState = dodgeState;
        PlayerDodging?.Invoke(dodgeState);
        recoveryTimer = playerStats.DodgeRecovery + playerStats.DodgeWindow;
        inDodgeRecovery = true;
        yield return new WaitForSeconds(playerStats.DodgeWindow);
        currentPlayerState = PlayerCombatStates.Recovering;
        inputQueued = false;
    }

    private IEnumerator Block()
    {
        //Debug.Log("BLOCKING!");
        currentPlayerState = PlayerCombatStates.Blocking;
        PlayerBlockStart?.Invoke();
        declineInput = true;
        yield return new WaitForSeconds(0.4f);
        if (!Input.GetMouseButton(0))
        {
            EndBlock();
        }
        declineInput = false;
    }

    private void EndBlock()
    {
        if (currentPlayerState == PlayerCombatStates.Blocking)
        {
            PlayerBlockEnd?.Invoke();
            recoveryTimer = playerStats.BlockRecovery;
            currentPlayerState = PlayerCombatStates.Recovering;
        }
    }

    public void DamagePlayer(int damage, PlayerCombatStates combatStateToAvoid)
    {
        bool hasBlocked = false;
        if (currentPlayerState != combatStateToAvoid)
        {
            int damageAfterResistances;
            if (currentPlayerState == PlayerCombatStates.Blocking)
            {
                hasBlocked = true;
                damageAfterResistances = CalculateBlock(damage);
            }
            else
            {
                damageAfterResistances = damage;
            }
            playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth - damageAfterResistances, 0, int.MaxValue);
            PlayerDamaged?.Invoke(damageAfterResistances, hasBlocked);
            //print($"Player took {damageAfterResistances} damage. {damage - damageAfterResistances} was blocked. [HP: {playerStats.CurrentHealth}/{playerStats.MaxHealth}]");

            // Dodge reset so the player isn't overwhelmed during combos
            if (inDodgeRecovery)
            {
                ResetToIdle();
            }
        }
    }

    private int CalculateBlock(int damage)
    {
        // Every 10 damage resistance increases effective health by 100%, then reduces damage by a further 1
        return Mathf.RoundToInt(Mathf.Clamp((damage * (10 / (10 + playerStats.DamageResistance))) - 1, 0, float.MaxValue));
    }

    private void ResetToIdle()
    {
        StopAllCoroutines();
        inputQueued = false;
        inDodgeRecovery = false;
        attackPreventionTimer = recoveryTimer;
        recoveryTimer = 0;
        currentPlayerState = PlayerCombatStates.Idle;
    }
}
