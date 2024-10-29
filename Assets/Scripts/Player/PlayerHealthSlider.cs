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
        if (PlayerSingleton.instance != null)
        {
            playerStats = PlayerSingleton.instance.GetComponent<PlayerStats>();
        }
        else
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }
        healthSlider = GetComponent<Slider>();
        hPText = GetComponentInChildren<TextMeshProUGUI>();
    }
}
