using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declared outside of class for ease of use
public enum PlayerCombatStates { Idle, Attacking, DodgingRight, DodgingLeft, DodgingUp, Blocking, Recovering }

[RequireComponent(typeof(PlayerStats)), DisallowMultipleComponent]
public class PlayerCombatHandler : MonoBehaviour
{
    private PlayerStats playerStats;

    private PlayerCombatStates currentPlayerState = PlayerCombatStates.Idle;
    public PlayerCombatStates CurrentPlayerState { get { return currentPlayerState; } }

    public static event Action<int> PlayerAttackEvent;

    // could refactor these into params too
    public static event Action PlayerAttackStart;
    public static event Action PlayerBlockStart;
    public static event Action PlayerBlockEnd;
    public static event Action PlayerDodgeRight;
    public static event Action PlayerDodgeLeft;
    public static event Action PlayerDodgeUp;

    private void Awake()
    {
        FindReferneces();
    }

    private void OnEnable()
    {
        InputMotionsHandler.PlayerTapInputEvent += Attack;
        InputMotionsHandler.PlayerSwipeRightInputEvent += DodgeRight;
        InputMotionsHandler.PlayerSwipeLeftInputEvent += DodgeLeft;
        InputMotionsHandler.PlayerSwipeUpInputEvent += DodgeUp;
        InputMotionsHandler.PlayerSwipeDownInputEvent += Block;
        InputMotionsHandler.PlayerReleaseInputEvent += EndBlock;
        EnemyCombatHandler.EnemyAttackEvent += DamagePlayer;
    }

    private void OnDisable()
    {
        InputMotionsHandler.PlayerTapInputEvent -= Attack;
        InputMotionsHandler.PlayerSwipeRightInputEvent -= DodgeRight;
        InputMotionsHandler.PlayerSwipeLeftInputEvent -= DodgeLeft;
        InputMotionsHandler.PlayerSwipeUpInputEvent -= DodgeUp;
        InputMotionsHandler.PlayerSwipeDownInputEvent -= Block;
        InputMotionsHandler.PlayerReleaseInputEvent -= EndBlock;
        EnemyCombatHandler.EnemyAttackEvent -= DamagePlayer;
    }

    private void Attack()
    {
        if (currentPlayerState == PlayerCombatStates.Idle)
        {
            currentPlayerState = PlayerCombatStates.Attacking;
            PlayerAttackStart?.Invoke();
            StartCoroutine(StartAttackWindup());
            Debug.Log("ATACKING!");
        }
    }

    private IEnumerator StartAttackWindup()
    {
        yield return new WaitForSeconds(playerStats.AttackSpeed);
        PlayerAttackEvent?.Invoke(playerStats.AttackDamage);
        StartCoroutine(Recovery(0f, playerStats.AttackRecovery));
    }

    private void DodgeRight()
    {
        if (currentPlayerState == PlayerCombatStates.Idle)
        {
            currentPlayerState = PlayerCombatStates.DodgingRight;
            PlayerDodgeRight?.Invoke();
            StartCoroutine(Recovery(playerStats.DodgeWindow, playerStats.DodgeRecovery));
            Debug.Log("DODGING RIGHT!");
        }
    }

    private void DodgeLeft()
    {
        if (currentPlayerState == PlayerCombatStates.Idle)
        {
            currentPlayerState = PlayerCombatStates.DodgingLeft;
            PlayerDodgeLeft?.Invoke();
            StartCoroutine(Recovery(playerStats.DodgeWindow, playerStats.DodgeRecovery));
            Debug.Log("DODGING LEFT!");
        }
    }

    private void DodgeUp()
    {
        if (currentPlayerState == PlayerCombatStates.Idle)
        {
            currentPlayerState = PlayerCombatStates.DodgingUp;
            PlayerDodgeUp?.Invoke();
            StartCoroutine(Recovery(playerStats.DodgeWindow, playerStats.DodgeRecovery));
            Debug.Log("JUMPING!");
        }
    }

    private void Block()
    {
        if (currentPlayerState == PlayerCombatStates.Idle)
        {
            currentPlayerState = PlayerCombatStates.Blocking;
            PlayerBlockStart?.Invoke();
            Debug.Log("BLOCKING!");
        }
    }

    private void EndBlock()
    {
        if (currentPlayerState == PlayerCombatStates.Blocking)
        {
            PlayerBlockEnd?.Invoke();
            StartCoroutine(Recovery(0f, playerStats.BlockRecovery));
        }
    }

    private IEnumerator Recovery(float stateDuration, float recoveryTime)
    {
        yield return new WaitForSeconds(stateDuration);
        currentPlayerState = PlayerCombatStates.Recovering;
        Debug.Log("Recovering...");

        yield return new WaitForSeconds(recoveryTime);
        currentPlayerState = PlayerCombatStates.Idle;
        Debug.Log("Now idle");
    }

    public void DamagePlayer(int damage, PlayerCombatStates requiredState)
    {
        if (currentPlayerState != requiredState)
        {
            // some arbitrary placeholder block calculations
            int damageAfterResistances;
            if (currentPlayerState == PlayerCombatStates.Blocking)
            {
                damageAfterResistances = Mathf.RoundToInt(damage / (1 + playerStats.DamageResistance));
            }
            else
            {
                damageAfterResistances = damage;
            }
            playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth - damageAfterResistances, 0, int.MaxValue);
            Debug.Log($"Player took {damageAfterResistances} damage. [HP: {playerStats.CurrentHealth}/{playerStats.MaxHealth}]");
        }
        else
        {
            Debug.Log("Damage avoided");
        }
    }

    private void FindReferneces()
    {
        playerStats = GetComponent<PlayerStats>();
    }
}
