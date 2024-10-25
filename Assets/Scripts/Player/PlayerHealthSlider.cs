using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSlider : MonoBehaviour
{
    private PlayerStats playerStats;
    private Slider healthSlider;
    private TextMeshProUGUI hPText;

    private void Awake()
    {
        FindReferences();
    }

    private void OnEnable()
    {
        PlayerStats.HealthValueChangedEvent += UpdateHealthBar;
    }

    private void OnDisable()
    {
        PlayerStats.HealthValueChangedEvent -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        hPText.text = $"HP: {currentHealth}/{maxHealth}";
    }

    private void FindReferences()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        healthSlider = GetComponent<Slider>();
        hPText = GetComponentInChildren<TextMeshProUGUI>();
    }
}
