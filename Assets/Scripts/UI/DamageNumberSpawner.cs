using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    [SerializeField] private GameObject damageNumberPrefab;

    private void OnEnable()
    {
        PlayerCombatHandler.PlayerDamaged += SpawnPlayerDamageNumber;
        EnemyCombatHandler.EnemyDamaged += SpawnEnemyDamageNumber;
    }

    private void OnDisable()
    {
        PlayerCombatHandler.PlayerDamaged -= SpawnPlayerDamageNumber;
        EnemyCombatHandler.EnemyDamaged -= SpawnEnemyDamageNumber;
    }

    // Kinda wishing health was just it's own script
    private void SpawnPlayerDamageNumber(int damage, bool hasBlocked)
    {
        var screenPoint = Camera.main.WorldToScreenPoint(Player.Instance.transform.position);
        var damageNumber = Instantiate(damageNumberPrefab, screenPoint, transform.rotation, transform).GetComponent<DamageNumber>();
        damageNumber.WorldSpawnPoint = Player.Instance.transform.position;
        damageNumber.Damage = damage;
        damageNumber.WasBlocking = hasBlocked;
    }

    private void SpawnEnemyDamageNumber(int damage)
    {
        var screenPoint = Camera.main.WorldToScreenPoint(FindObjectOfType<EnemyCombatHandler>().transform.position);
        var damageNumber = Instantiate(damageNumberPrefab, screenPoint, transform.rotation, transform).GetComponent<DamageNumber>();
        damageNumber.WorldSpawnPoint = FindObjectOfType<EnemyCombatHandler>().transform.position;
        damageNumber.Damage = damage;
    }
}