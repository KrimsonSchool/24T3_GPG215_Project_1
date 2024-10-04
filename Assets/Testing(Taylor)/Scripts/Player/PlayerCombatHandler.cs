using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats)), DisallowMultipleComponent]
public class PlayerCombatHandler : MonoBehaviour
{
    private PlayerStats playerStats;

    public enum PlayerStates { Idle, Attacking, DodgingRight, DodgingLeft, Blocking, Recovering }
    private PlayerStates currentPlayerState = PlayerStates.Idle;
    public PlayerStates CurrentPlayerState { get { return currentPlayerState; } }

    public delegate void PlayerAttackDelegate(int damage);
    public static PlayerAttackDelegate PlayerAttackEvent;

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
        EnemyCombatHandler.EnemyAttackEvent += PlayerAttacked;
    }

    private void OnDisable()
    {
        InputMotionsHandler.PlayerAttackInputEvent -= Attack;
        InputMotionsHandler.PlayerDodgeRightInputEvent -= DodgeRight;
        InputMotionsHandler.PlayerDodgeLeftInputEvent -= DodgeLeft;
        InputMotionsHandler.PlayerBlockInputEvent -= Block;
        EnemyCombatHandler.EnemyAttackEvent -= PlayerAttacked;
    }

    private void Attack()
    {
        if (currentPlayerState == PlayerStates.Idle)
        {
            currentPlayerState = PlayerStates.Attacking;
            StartCoroutine(StartAttackWindup());
            Debug.Log("Entering attacking state");
        }
    }

    private IEnumerator StartAttackWindup()
    {
        yield return new WaitForSeconds(playerStats.AttackSpeed);
        PlayerAttackEvent?.Invoke(playerStats.BaseAttackDamage);
        StartCoroutine(Recovery(0f, playerStats.AttackRecovery));
    }

    private void DodgeRight()
    {
        if (currentPlayerState == PlayerStates.Idle)
        {
            currentPlayerState = PlayerStates.DodgingRight;
            StartCoroutine(Recovery(playerStats.DodgeWindow, playerStats.DodgeRecovery));
            Debug.Log("Entering right dodge state");
        }
    }

    private void DodgeLeft()
    {
        if (currentPlayerState == PlayerStates.Idle)
        {
            currentPlayerState = PlayerStates.DodgingLeft;
            StartCoroutine(Recovery(playerStats.DodgeWindow, playerStats.DodgeRecovery));
            Debug.Log("Entering left dodge state");
        }
    }

    private void Block()
    {
        if (currentPlayerState == PlayerStates.Idle)
        {
            currentPlayerState = PlayerStates.Blocking;
            StartCoroutine(Recovery(playerStats.BlockWindow, playerStats.BlockRecovery));
            Debug.Log("Entering block state");
        }
    }

    private IEnumerator Recovery(float stateDuration, float recoveryTime)
    {
        yield return new WaitForSeconds(stateDuration);
        currentPlayerState = PlayerStates.Recovering;
        Debug.Log("Enterting recovery state");

        yield return new WaitForSeconds(recoveryTime);
        currentPlayerState = PlayerStates.Idle;
        Debug.Log("Entering idle state");
    }

    public void PlayerAttacked(int damage, PlayerStates requiredState)
    {
        if (currentPlayerState != requiredState)
        {
            int damageAfterResistances = Mathf.RoundToInt(damage / (1 + playerStats.DamageResistance));
            playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth - damageAfterResistances, 0, int.MaxValue);
            Debug.Log($"{damageAfterResistances} damage dealt to player. {playerStats.CurrentHealth}/{playerStats.MaxHealth} health remaining.");
        }
        else
        {
            Debug.Log("Attacked successfully avoided");
        }
    }

    private void FindReferneces()
    {
        playerStats = GetComponent<PlayerStats>();
    }
}
