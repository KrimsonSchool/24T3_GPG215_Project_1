using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;

    [Header("Offensive Stats")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackSpeed = 0.2f;
    [SerializeField] private float attackRecovery = 0.4f;
    [SerializeField] private float critMultiplier = 1f;

    [Header("Defensive Stats")]
    [SerializeField] private float dodgeWindow = 0.4f;
    [SerializeField] private float dodgeRecovery = 0.4f;
    [SerializeField] private float blockWindow = 0.4f;
    [SerializeField] private float blockRecovery = 0.4f;
    [SerializeField] private float damageResistance = 0f;

    [Header("Ability Stats")]
    [SerializeField] private float abilityDamageMultiplier = 1f;
    [SerializeField] private float abilityCDRMultiplier = 1f;

    /// <summary>
    /// &lt;CurrentHealth, MaxHealth&gt;
    /// </summary>
    public static event Action<int, int> HealthValueChangedEvent;
    public static event Action PlayerDiedEvent;

    #region Health Getters & Setters
    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            var previousMaxHealth = maxHealth;
            maxHealth = Mathf.Clamp(value, 1, int.MaxValue);
            //print($"Player's max health set to {maxHealth}");
            HealthValueChangedEvent?.Invoke(currentHealth, maxHealth);
        }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            var previousCurrentHealth = currentHealth;
            currentHealth = (Mathf.Clamp(value, 0, int.MaxValue));
            //print($"{currentHealth - previousCurrentHealth} applied to player health. [HP: {currentHealth}/{maxHealth}]");
            HealthValueChangedEvent?.Invoke(currentHealth, maxHealth);
            if (currentHealth <= 0)
            {
                //print("Player died...");
                PlayerDiedEvent?.Invoke();
            }
        }
    }
    #endregion

    #region Offensive Stats Getters & Setters
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

    public float CritMultiplier
    {
        get { return critMultiplier; }
        set
        {
            critMultiplier = Mathf.Clamp(value, 1f, float.MaxValue);
        }
    }
    #endregion

    #region Defensive Stats Getters & Setters
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

    public float BlockWindow
    {
        get { return blockWindow; }
        set
        {
            blockWindow = Mathf.Clamp(value, 0f, float.MaxValue);
        }
    }

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

    #region Ability Stats Getters & Setters
    public float AbilityDamageMultiplier
    {
        get { return abilityDamageMultiplier; }
        set
        {
            abilityDamageMultiplier = Mathf.Clamp(value, 0f, float.MaxValue);
        }
    }

    public float AbilityCDRMultiplier
    {
        get { return abilityCDRMultiplier; }
        set
        {
            abilityCDRMultiplier = Mathf.Clamp(value, 0f, float.MaxValue);
        }
    }
    #endregion
}
