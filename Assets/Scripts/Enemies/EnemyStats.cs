using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    /// <summary>
    /// &lt;CurrentHealth, MaxHealth&gt;
    /// </summary>
    public static event Action<int, int> HealthValueChangedEvent;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth = 3;

    [Header("Offensive Stats")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackWindup = 1f;
    [SerializeField] private float attackWarning = 0.5f;
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] private int attackCombos = 1;
    [SerializeField] private float attackComboSpeed = 0f;

    #region Health Getters & Setters
    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = Mathf.Clamp(value, 1, int.MaxValue);
            HealthValueChangedEvent?.Invoke(currentHealth, maxHealth);
        }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = (Mathf.Clamp(value, 0, int.MaxValue));
            HealthValueChangedEvent?.Invoke(currentHealth, maxHealth);
        }
    }
    #endregion

    #region Offensive Stats Getters & Setters
    public int AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
    public float AttackWindup { get { return attackWindup; } set { attackWindup = value; } }
    public float AttackWarning { get { return attackWarning; } set { attackWarning = value; } }
    public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
    public int AttackCombos { get { return attackCombos; } set { attackCombos = value; } }
    public float AttackComboSpeed { get { return attackComboSpeed; } set { attackComboSpeed = value; } }
    #endregion

    private void Start()
    {
        GameManager gameManager;
        if (GameManager.instance == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        else
        {
            gameManager = GameManager.instance.GetComponent<GameManager>();
        }
        MaxHealth = Mathf.RoundToInt(maxHealth * (1 + ((gameManager.RoomLevel - 1) * 0.5f)));
        CurrentHealth = MaxHealth;
        AttackDamage = Mathf.RoundToInt(attackDamage * (1 + ((gameManager.RoomLevel - 1) * 0.2f)));
        // need some difficulty balancing, only did some arbitrary stuff
    }
}
