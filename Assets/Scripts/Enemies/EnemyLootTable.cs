using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLootTable : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float dropChance;
    [SerializeField] private bool forceGearType;
    [SerializeField] private Gear.GearType gearType;
    [SerializeField] private List<GameObject> lootTable = new List<GameObject>();
    public static event Action<bool> DroppedLoot;

    private void OnEnable()
    {
        EnemyStats.EnemyDied += DropLoot;
    }

    private void OnDisable()
    {
        EnemyStats.EnemyDied -= DropLoot;
    }

    private void DropLoot()
    {
        if (UnityEngine.Random.Range(0f, 100f) < dropChance)
        {
            int index = UnityEngine.Random.Range(0, lootTable.Count);
            var loot = Instantiate(lootTable[index], transform.position, transform.rotation);
            SetLootVariables(loot);
            DroppedLoot?.Invoke(true);
        }
        else
        {
            DroppedLoot?.Invoke(false);
        }
    }

    private void SetLootVariables(GameObject loot)
    {
        var gearInfo = loot.GetComponent<Gear>();
        gearInfo.teir = Mathf.RoundToInt(1 + (GameManager.Instance.RoomLevel / 10));
        if (forceGearType)
        {
            gearInfo.ForceGearType = true;
            gearInfo.type = gearType;
        }
    }
}
