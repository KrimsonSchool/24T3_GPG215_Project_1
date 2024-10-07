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
        UpdateHealthBar(0);
    }

    private void OnEnable()
    {
        PlayerStats.HealthValueChangedEvent += UpdateHealthBar;
    }

    private void OnDisable()
    {
        PlayerStats.HealthValueChangedEvent -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int damageDone)
    {
        healthSlider.maxValue = playerStats.MaxHealth;
        healthSlider.value = playerStats.CurrentHealth;
        hPText.text = $"HP: {playerStats.CurrentHealth}/{playerStats.MaxHealth}";
    }

    private void FindReferences()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        healthSlider = GetComponent<Slider>();
        hPText = GetComponentInChildren<TextMeshProUGUI>();
    }
}
