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

    private void Start()
    {
        healthSlider.maxValue = playerStats.MaxHealth;
        healthSlider.value = playerStats.CurrentHealth;
        hPText.text = $"HP: {playerStats.CurrentHealth}/{playerStats.MaxHealth}";
    }

    private void OnEnable()
    {
        PlayerStats.HealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        PlayerStats.HealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        hPText.text = $"HP: {currentHealth}/{maxHealth}";
    }

    private void FindReferences()
    {
        if (PlayerStats.Instance != null)
        {
            playerStats = PlayerStats.Instance;
        }
        else
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }
        healthSlider = GetComponent<Slider>();
        hPText = GetComponentInChildren<TextMeshProUGUI>();
    }
}
