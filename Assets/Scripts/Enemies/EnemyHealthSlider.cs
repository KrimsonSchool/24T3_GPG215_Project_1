using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    private EnemyStats enemyStats;
    private Slider healthSlider;
    private TextMeshProUGUI hPText;

    private void Awake()
    {
        FindReferences();
    }

    private void Start()
    {
        UpdateHealthBar();
    }

    private void OnEnable()
    {
        EnemyStats.HealthValueChangedEvent += UpdateHealthBar;
    }

    private void OnDisable()
    {
        EnemyStats.HealthValueChangedEvent -= UpdateHealthBar;
    }

    private void UpdateHealthBar()
    {
        healthSlider.maxValue = enemyStats.MaxHealth;
        healthSlider.value = enemyStats.CurrentHealth;
        hPText.text = $"{enemyStats.CurrentHealth}/{enemyStats.MaxHealth}";
    }

    private void FindReferences()
    {
        enemyStats = FindObjectOfType<EnemyStats>();
        healthSlider = GetComponent<Slider>();
        hPText = GetComponentInChildren<TextMeshProUGUI>();
    }
}
