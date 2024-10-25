using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Declared outside of class for ease of use
public enum PlayerCombatStates { Idle, Attacking, DodgingRight, DodgingLeft, DodgingUp, Blocking, Recovering }

[RequireComponent(typeof(PlayerStats)), DisallowMultipleComponent]
public class PlayerCombatHandler : MonoBehaviour
{
    private PlayerStats playerStats;
    [SerializeField] private GameObject floatingDamageNumberPrefab;

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

    private float attackPrevention = 0f;
    private bool isDodging = false;
    private Coroutine dodgeRecovery;

    private void Awake()
    {
        FindReferneces();
    }

    private void Update()
    {
        if (attackPrevention > 0f)
        {
            attackPrevention -= Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        PlayerCombatControls.PlayerTapInputEvent += Attack;
        PlayerCombatControls.PlayerSwipeRightInputEvent += DodgeRight;
        PlayerCombatControls.PlayerSwipeLeftInputEvent += DodgeLeft;
        PlayerCombatControls.PlayerSwipeUpInputEvent += DodgeUp;
        PlayerCombatControls.PlayerSwipeDownInputEvent += Block;
        PlayerCombatControls.PlayerReleaseInputEvent += EndBlock;
        EnemyCombatHandler.EnemyAttackEvent += DamagePlayer;
    }

    private void OnDisable()
    {
        PlayerCombatControls.PlayerTapInputEvent -= Attack;
        PlayerCombatControls.PlayerSwipeRightInputEvent -= DodgeRight;
        PlayerCombatControls.PlayerSwipeLeftInputEvent -= DodgeLeft;
        PlayerCombatControls.PlayerSwipeUpInputEvent -= DodgeUp;
        PlayerCombatControls.PlayerSwipeDownInputEvent -= Block;
        PlayerCombatControls.PlayerReleaseInputEvent -= EndBlock;
        EnemyCombatHandler.EnemyAttackEvent -= DamagePlayer;
    }

    private void Attack()
    {
        if (currentPlayerState == PlayerCombatStates.Idle && attackPrevention <= 0f)
        {
            currentPlayerState = PlayerCombatStates.Attacking;
            PlayerAttackStart?.Invoke();
            StartCoroutine(StartAttackWindup());
            //Debug.Log("ATACKING!");
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
            isDodging = true;
            dodgeRecovery = StartCoroutine(Recovery(playerStats.DodgeWindow, playerStats.DodgeRecovery));
            //Debug.Log("DODGING RIGHT!");
        }
    }

    private void DodgeLeft()
    {
        if (currentPlayerState == PlayerCombatStates.Idle)
        {
            currentPlayerState = PlayerCombatStates.DodgingLeft;
            PlayerDodgeLeft?.Invoke();
            isDodging = true;
            dodgeRecovery = StartCoroutine(Recovery(playerStats.DodgeWindow, playerStats.DodgeRecovery));
            //Debug.Log("DODGING LEFT!");
        }
    }

    private void DodgeUp()
    {
        if (currentPlayerState == PlayerCombatStates.Idle)
        {
            currentPlayerState = PlayerCombatStates.DodgingUp;
            PlayerDodgeUp?.Invoke();
            isDodging = true;
            dodgeRecovery = StartCoroutine(Recovery(playerStats.DodgeWindow, playerStats.DodgeRecovery));
            //Debug.Log("JUMPING!");
        }
    }

    private void Block()
    {
        if (currentPlayerState == PlayerCombatStates.Idle)
        {
            currentPlayerState = PlayerCombatStates.Blocking;
            PlayerBlockStart?.Invoke();
            //Debug.Log("BLOCKING!");
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
        //Debug.Log("Recovering...");

        yield return new WaitForSeconds(recoveryTime);
        isDodging = false;
        currentPlayerState = PlayerCombatStates.Idle;
        //Debug.Log("Now idle");
    }

    public void DamagePlayer(int damage, PlayerCombatStates combatStateToAvoid)
    {
        if (currentPlayerState != combatStateToAvoid)
        {
            // some arbitrary placeholder block calculations
            int damageAfterResistances;
            if (currentPlayerState == PlayerCombatStates.Blocking)
            {
                damageAfterResistances = Mathf.RoundToInt(damage * (1 - (playerStats.DamageResistance * 0.01f)));
            }
            else
            {
                damageAfterResistances = damage;
            }
            playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth - damageAfterResistances, 0, int.MaxValue);
            SpawnFloatingNumber(damageAfterResistances);
            //Debug.Log($"Player took {damageAfterResistances} damage. {damage - damageAfterResistances} was blocked. [HP: {playerStats.CurrentHealth}/{playerStats.MaxHealth}]");
        }
        else
        {
            //Debug.Log("Damage avoided");
        }

        // Dodge reset so the player isn't overwhelmed during combos
        if (dodgeRecovery != null && isDodging)
        {
            StopCoroutine(dodgeRecovery);
            isDodging = false;
            attackPrevention = 0.5f;
            currentPlayerState = PlayerCombatStates.Idle;
        }
    }

    private void SpawnFloatingNumber(int damageDone)
    {
        var prefab = Instantiate(floatingDamageNumberPrefab, transform.position, default);
        prefab.GetComponentInChildren<TextMeshProUGUI>().text = $"-{damageDone}HP";
    }

    private void FindReferneces()
    {
        playerStats = GetComponent<PlayerStats>();
    }
}
