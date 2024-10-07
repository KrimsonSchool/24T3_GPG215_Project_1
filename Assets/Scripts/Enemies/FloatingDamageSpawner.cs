using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingDamageSpawner : MonoBehaviour
{
    [SerializeField] private GameObject floatingDamageNumberPrefab;

    private void OnEnable()
    {
        PlayerCombatHandler.PlayerAttackEvent += SpawnFloatingNumber;
    }

    private void OnDisable()
    {
        PlayerCombatHandler.PlayerAttackEvent -= SpawnFloatingNumber;
    }

    private void SpawnFloatingNumber(int damageDone)
    {
        var prefab = Instantiate(floatingDamageNumberPrefab, transform.position, default);
        prefab.GetComponentInChildren<TextMeshProUGUI>().text = $"-{damageDone}HP";
    }
}
