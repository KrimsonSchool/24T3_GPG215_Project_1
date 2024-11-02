using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSlider : MonoBehaviour
{
    private EnemyStats enemyStats;
    private Slider healthSlider;
    private TextMeshProUGUI sliderText;

    private void Awake()
    {
        FindReferences();
    }

    private void Start()
    {
        UpdateHealthBar(enemyStats.CurrentHealth, enemyStats.MaxHealth);
    }

    private void OnEnable()
    {
        EnemyStats.HealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        EnemyStats.HealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        sliderText.text = $"{currentHealth}/{maxHealth}";
    }

    private void FindReferences()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        healthSlider = GetComponent<Slider>();
        sliderText = GetComponentInChildren<TextMeshProUGUI>();
    }
}
