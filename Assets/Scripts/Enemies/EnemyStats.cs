using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private GameManager gameManager;
    /// <summary>
    /// &lt;CurrentHealth, MaxHealth&gt;
    /// </summary>
    public static event Action<int, int> HealthValueChangedEvent;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth = 3;

    [Header("Offensive Stats")]
    [SerializeField] private int attackDamage = 1;

    [Tooltip("The amount of time before triggering the attack warning")]
    [SerializeField] private float attackWindup = 1f;

    [Tooltip("The amount of time the threat direction is shown before the attack triggers")]
    [SerializeField] private float attackWarning = 0.5f;

    [Tooltip("The amount of time between starting an attack phase")]
    [SerializeField] private float attackSpeed = 3f;

    [Tooltip("The amount of attacks per attack phase")]
    [SerializeField, Range(1, 10)] private int attackCombos = 1;

    [Tooltip("The amount of time before triggering the attack warning between combo attacks (essentially the attack windup before combo attacks)")]
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

    /// <summary>
    /// The amount of time before triggering the attack warning
    /// </summary>
    public float AttackWindup { get { return attackWindup; } set { attackWindup = value; } }

    /// <summary>
    /// The amount of time the threat direction is shown before the attack triggers
    /// </summary>
    public float AttackWarning { get { return attackWarning; } set { attackWarning = value; } }

    /// <summary>
    /// The amount of time between starting an attack phase
    /// </summary>
    public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }

    /// <summary>
    /// The amount of attacks per attack phase
    /// </summary>
    public int AttackCombos { get { return attackCombos; } set { attackCombos = value; } }

    /// <summary>
    /// The amount of time before triggering the attack warning between combo attacks (essentially the attack windup before combo attacks)
    /// </summary>
    public float AttackComboSpeed { get { return attackComboSpeed; } set { attackComboSpeed = value; } }
    #endregion

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
        }
        else
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void Start()
    {
        MaxHealth = Mathf.RoundToInt(maxHealth * (1 + ((gameManager.RoomLevel - 1) * 0.5f)));
        CurrentHealth = MaxHealth;
        AttackDamage = Mathf.RoundToInt(attackDamage * (1 + ((gameManager.RoomLevel - 1) * 0.2f)));
        // need some difficulty balancing, only did some arbitrary stuff
    }
}
