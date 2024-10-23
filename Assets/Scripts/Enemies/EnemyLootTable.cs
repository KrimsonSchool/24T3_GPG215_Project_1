using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLootTable : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float dropChance;
    [SerializeField] private List<GameObject> lootTable = new List<GameObject>();
    public static event Action<bool> DroppedLoot;

    private void OnEnable()
    {
        EnemyCombatHandler.EnemyDeadEvent += DropLoot;
    }

    private void OnDisable()
    {
        EnemyCombatHandler.EnemyDeadEvent -= DropLoot;
    }

    private void DropLoot()
    {
        if (UnityEngine.Random.Range(0f, 100f) < dropChance)
        {
            int index = UnityEngine.Random.Range(0, lootTable.Count);
            Instantiate(lootTable[index], transform.position, transform.rotation);
            DroppedLoot?.Invoke(true);
        }
        else
        {
            DroppedLoot?.Invoke(false);
        }
    }
}
