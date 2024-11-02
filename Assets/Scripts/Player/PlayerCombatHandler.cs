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
    private Coroutine dodgeRecovery;
    private PlayerCombatStates currentPlayerState = PlayerCombatStates.Idle;
    private float attackPrevention = 0f;
    private bool isDodging = false;

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
    /// 1. &lt;int&gt; : PlayerStats.AttackDamage
    /// </summary>
    public static event Action<int> PlayerAttacked;

    // could refactor these into params too
    public static event Action PlayerAttackStart;
    public static event Action PlayerBlockStart;
    public static event Action PlayerBlockEnd;
    public static event Action PlayerDodgeRight;
    public static event Action PlayerDodgeLeft;
    public static event Action PlayerDodgeUp;
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
        PlayerCombatControls.PlayerTapInputEvent += Attack;
        PlayerCombatControls.PlayerSwipeRightInputEvent += DodgeRight;
        PlayerCombatControls.PlayerSwipeLeftInputEvent += DodgeLeft;
        PlayerCombatControls.PlayerSwipeUpInputEvent += DodgeUp;
        PlayerCombatControls.PlayerSwipeDownInputEvent += Block;
        PlayerCombatControls.PlayerReleaseInputEvent += EndBlock;
        EnemyCombatHandler.EnemyAttacked += DamagePlayer;
    }

    private void OnDisable()
    {
        PlayerCombatControls.PlayerTapInputEvent -= Attack;
        PlayerCombatControls.PlayerSwipeRightInputEvent -= DodgeRight;
        PlayerCombatControls.PlayerSwipeLeftInputEvent -= DodgeLeft;
        PlayerCombatControls.PlayerSwipeUpInputEvent -= DodgeUp;
        PlayerCombatControls.PlayerSwipeDownInputEvent -= Block;
        PlayerCombatControls.PlayerReleaseInputEvent -= EndBlock;
        EnemyCombatHandler.EnemyAttacked -= DamagePlayer;
    }
    #endregion

    private void Update()
    {
        if (attackPrevention > 0f)
        {
            attackPrevention -= Time.deltaTime;
        }
    }

    private void Attack()
    {
        if (currentPlayerState == PlayerCombatStates.Idle && attackPrevention <= 0f)
        {
            FindObjectOfType<FxPlayer>().PlaySound("PlayerAttack");

            currentPlayerState = PlayerCombatStates.Attacking;
            PlayerAttackStart?.Invoke();
            StartCoroutine(StartAttackWindup());
            //Debug.Log("ATACKING!");
        }
    }

    private IEnumerator StartAttackWindup()
    {
        yield return new WaitForSeconds(playerStats.AttackSpeed);
        PlayerAttacked?.Invoke(playerStats.AttackDamage);
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
            bool hasBlocked = false;
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
            SpawnFloatingNumber(damageAfterResistances, hasBlocked);
            //print($"Player took {damageAfterResistances} damage. {damage - damageAfterResistances} was blocked. [HP: {playerStats.CurrentHealth}/{playerStats.MaxHealth}]");
        }
        else
        {
            //print("Damage avoided");
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

    private int CalculateBlock(int damage)
    {
        // Every 10 damage resistance increases effective health by 100%, then reduces damage by a further 1
        return Mathf.RoundToInt(Mathf.Clamp((damage * (10 / (10 + playerStats.DamageResistance))) - 1, 0, float.MaxValue));
    }

    private void SpawnFloatingNumber(int damageDone, bool hasBlocked)
    {
        var prefab = Instantiate(floatingDamageNumberPrefab, transform.position, transform.rotation);
        if (hasBlocked)
        {
            prefab.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
        }
        if (damageDone == 0)
        {
            prefab.GetComponentInChildren<TextMeshProUGUI>().text = "Blocked";
        }
        else
        {
            prefab.GetComponentInChildren<TextMeshProUGUI>().text = $"-{damageDone}HP";
        }
    }
}
