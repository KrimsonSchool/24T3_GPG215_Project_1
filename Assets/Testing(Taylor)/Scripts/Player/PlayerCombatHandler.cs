using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declared outside of class for ease of use
public enum PlayerCombatStates { Idle, Attacking, DodgingRight, DodgingLeft, Blocking, Recovering }

[RequireComponent(typeof(PlayerStats)), DisallowMultipleComponent]
public class PlayerCombatHandler : MonoBehaviour
{
    private PlayerStats playerStats;

    private PlayerCombatStates currentPlayerState = PlayerCombatStates.Idle;
    public PlayerCombatStates CurrentPlayerState { get { return currentPlayerState; } }

    public static event Action<int> PlayerAttackEvent;

    public static event Action PlayerAttackStart;
    public static event Action PlayerBlockStart;
    public static event Action PlayerDodgeRight;
    public static event Action PlayerDodgeLeft;

    private void Awake()
    {
        FindReferneces();
    }

    private void OnEnable()
    {
        InputMotionsHandler.PlayerAttackInputEvent += Attack;
        InputMotionsHandler.PlayerDodgeRightInputEvent += DodgeRight;
        InputMotionsHandler.PlayerDodgeLeftInputEvent += DodgeLeft;
        InputMotionsHandler.PlayerBlockInputEvent += Block;
        EnemyCombatHandler.EnemyAttackEvent += DamagePlayer;
    }

    private void OnDisable()
    {
        InputMotionsHandler.PlayerAttackInputEvent -= Attack;
        InputMotionsHandler.PlayerDodgeRightInputEvent -= DodgeRight;
        InputMotionsHandler.PlayerDodgeLeftInputEvent -= DodgeLeft;
        InputMotionsHandler.PlayerBlockInputEvent -= Block;
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

    private void Block()
    {
        if (currentPlayerState == PlayerCombatStates.Idle)
        {
            currentPlayerState = PlayerCombatStates.Blocking;
            PlayerBlockStart?.Invoke();
            StartCoroutine(Recovery(playerStats.BlockWindow, playerStats.BlockRecovery));
            Debug.Log("BLOCKING!");
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
            int damageAfterResistances = Mathf.RoundToInt(damage / (1 + playerStats.DamageResistance));
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
