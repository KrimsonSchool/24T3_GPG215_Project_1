using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    // Value Changed Events
    public static event Action HealthValueChangedEvent;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth = 3;

    [Header("Offensive Stats")]
    [SerializeField] private int baseAttackDamage = 1;
    [SerializeField] private float attackWindup = 1f;
    [SerializeField] private float attackSpeed = 3f;

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
        }
    }

    public float AttackWindup
    {
        get { return attackWindup; }
        set
        {
            attackWindup = Mathf.Clamp(value, 0f, float.MaxValue);
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
    #endregion

    private void Awake()
    {
        RoomLevelManager roomLevelManager = FindObjectOfType<RoomLevelManager>();
        MaxHealth = Mathf.RoundToInt(3 * (1 + (roomLevelManager.RoomLevel * 0.5f)));
        CurrentHealth = MaxHealth;
        BaseAttackDamage = 1;
        AttackWindup = 1f;
        AttackSpeed = 3f;
        // need some difficulty balancing, only did some arbitrary stuff to health
    }
}
