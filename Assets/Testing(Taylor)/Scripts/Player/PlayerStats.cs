using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerStats : MonoBehaviour
{
    // Value Changed Events
    public static event Action HealthValueChangedEvent;
    public static event Action StatValueChangedEvent;

    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int currentHealth = 10;

    [Header("Offensive Stats")]
    [SerializeField] private int baseAttackDamage = 1;
    [SerializeField] private float attackSpeed = 0.4f;
    [SerializeField] private float attackRecovery = 0.4f;

    [Header("Defensive Stats")]
    [SerializeField] private float dodgeWindow = 0.4f;
    [SerializeField] private float dodgeRecovery = 0.4f;
    [SerializeField] private float blockWindow = 0.4f;
    [SerializeField] private float blockRecovery = 0.4f;
    [SerializeField] private float damageResistance = 0f;

    [Header("Ability Stats")]
    [SerializeField] private float abilityDamageMultiplier = 1f;
    [SerializeField] private float abilityCDRMultiplier = 1f;

    #region Health Getters & Setters
    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = Mathf.Clamp(value, 1, int.MaxValue);
            HealthValueChangedEvent?.Invoke();
        }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = (Mathf.Clamp(value, 0, int.MaxValue));
            HealthValueChangedEvent?.Invoke();
        }
    }
    #endregion

    #region Offensive Stats Getters & Setters
    public int BaseAttackDamage
    {
        get { return baseAttackDamage; }
        set
        {
            baseAttackDamage = Mathf.Clamp(value, 0, int.MaxValue);
            StatValueChangedEvent?.Invoke();
        }
    }

    public float AttackSpeed
    {
        get { return attackSpeed; }
        set
        {
            attackSpeed = Mathf.Clamp(value, 0f, float.MaxValue);
            StatValueChangedEvent?.Invoke();
        }
    }

    public float AttackRecovery
    {
        get { return attackRecovery; }
        set
        {
            attackRecovery = Mathf.Clamp(value, 0f, float.MaxValue);
            StatValueChangedEvent?.Invoke();
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
            StatValueChangedEvent?.Invoke();
        }
    }

    public float DodgeRecovery
    {
        get { return dodgeRecovery; }
        set
        {
            dodgeRecovery = Mathf.Clamp(value, 0f, float.MaxValue);
            StatValueChangedEvent?.Invoke();
        }
    }

    public float BlockWindow
    {
        get { return blockWindow; }
        set
        {
            blockWindow = Mathf.Clamp(value, 0f, float.MaxValue);
            StatValueChangedEvent?.Invoke();
        }
    }

    public float BlockRecovery
    {
        get { return blockRecovery; }
        set
        {
            blockRecovery = Mathf.Clamp(value, 0f, float.MaxValue);
            StatValueChangedEvent?.Invoke();
        }
    }

    public float DamageResistance
    {
        get { return damageResistance; }
        set
        {
            damageResistance = Mathf.Clamp(value, 0f, float.MaxValue);
            StatValueChangedEvent?.Invoke();
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
            StatValueChangedEvent?.Invoke();
        }
    }

    public float AbilityCDRMultiplier
    {
        get { return abilityCDRMultiplier; }
        set
        {
            abilityCDRMultiplier = Mathf.Clamp(value, 0f, float.MaxValue);
            StatValueChangedEvent?.Invoke();
        }
    }
    #endregion
}
