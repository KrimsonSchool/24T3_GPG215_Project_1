using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerStats : Singleton<PlayerStats>
{
    #region Fields
    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;


    [Header("Offensive Stats")]
    [Tooltip("The damage of an attack")]
    [SerializeField] private int attackDamage = 1;

    [Tooltip("The amount of time between triggering the attack and the attack hitting")]
    [SerializeField] private float attackSpeed = 0.2f;

    [Tooltip("The amount of time after the attack hit in which the player is forced into a recovering state")]
    [SerializeField] private float attackRecovery = 0.4f;

    //[Tooltip("The multiplier applied to attack damage on the event of a crit")]
    //[SerializeField] private float critMultiplier = 1f;


    [Header("Defensive Stats")]
    [Tooltip("The amount of time a player is in the dodge state and can avoid damage")]
    [SerializeField] private float dodgeWindow = 0.4f;

    [Tooltip("The amount of time after leaving the dodge state in which the player is forced into a recovering state")]
    [SerializeField] private float dodgeRecovery = 0.4f;

    //[Tooltip("Depreciated")]
    //[SerializeField] private float blockWindow = 0.4f;

    [Tooltip("The amount of time after leaving the block state in which the player is forced into a recovering state")]
    [SerializeField] private float blockRecovery = 0.4f;

    [Tooltip("The amount of damage resistance the player has. Look in PlayerCombatHandler to view calculations")]
    [SerializeField] private float damageResistance = 0f;


    //[Header("Ability Stats")]
    //[Tooltip("Not yet implemented")]
    //[SerializeField] private float abilityDamageMultiplier = 1f;

    //[Tooltip("Not yet implemented")]
    //[SerializeField] private float abilityCDRMultiplier = 1f;
    #endregion

    #region Events
    /// <summary>
    /// 1. &lt;int&gt; : CurrentHealth <br></br>
    /// 2. &lt;int&gt; : MaxHealth
    /// </summary>
    public static event Action<int, int> HealthChanged;

    public static event Action PlayerDied;
    #endregion

    #region Health Properties
    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            var previousMaxHealth = maxHealth;
            maxHealth = Mathf.Clamp(value, 1, int.MaxValue);
            print($"Player's max health set to {maxHealth}");
            HealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            var previousCurrentHealth = currentHealth;
            currentHealth = (Mathf.Clamp(value, 0, int.MaxValue));
            print($"{currentHealth - previousCurrentHealth} applied to player health. [HP: {currentHealth}/{maxHealth}]");
            HealthChanged?.Invoke(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                //print("Player died...");
                PlayerDied?.Invoke();
            }
        }
    }
    #endregion

    #region Offensive Stats Properties
    public int AttackDamage
    {
        get { return attackDamage; }
        set
        {
            attackDamage = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }

    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            attackSpeed = Mathf.Clamp(value, 0f, float.MaxValue);
        }
    }

    public float AttackRecovery
    {
        get { return attackRecovery; }
        set
        {
            attackRecovery = Mathf.Clamp(value, 0f, float.MaxValue);
        }
    }

    //public float CritMultiplier
    //{
    //    get { return critMultiplier; }
    //    set
    //    {
    //        critMultiplier = Mathf.Clamp(value, 1f, float.MaxValue);
    //    }
    //}
    #endregion

    #region Defensive Stat Properties
    public float DodgeWindow
    {
        get { return dodgeWindow; }
        set
        {
            dodgeWindow = Mathf.Clamp(value, 0f, float.MaxValue);
        }
    }

    public float DodgeRecovery
    {
        get { return dodgeRecovery; }
        set
        {
            dodgeRecovery = Mathf.Clamp(value, 0f, float.MaxValue);
        }
    }

    //public float BlockWindow
    //{
    //    get { return blockWindow; }
    //    set
    //    {
    //        blockWindow = Mathf.Clamp(value, 0f, float.MaxValue);
    //    }
    //}

    public float BlockRecovery
    {
        get { return blockRecovery; }
        set
        {
            blockRecovery = Mathf.Clamp(value, 0f, float.MaxValue);
        }
    }

    public float DamageResistance
    {
        get { return damageResistance; }
        set
        {
            damageResistance = Mathf.Clamp(value, 0f, float.MaxValue);
        }
    }
    #endregion

    #region Ability Stat Properties
    //public float AbilityDamageMultiplier
    //{
    //    get { return abilityDamageMultiplier; }
    //    set
    //    {
    //        abilityDamageMultiplier = Mathf.Clamp(value, 0f, float.MaxValue);
    //    }
    //}

    //public float AbilityCDRMultiplier
    //{
    //    get { return abilityCDRMultiplier; }
    //    set
    //    {
    //        abilityCDRMultiplier = Mathf.Clamp(value, 0f, float.MaxValue);
    //    }
    //}
    #endregion
}
