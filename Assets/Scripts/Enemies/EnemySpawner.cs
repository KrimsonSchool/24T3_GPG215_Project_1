using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private GameObject tutorialEnemy;
    [SerializeField] private List<WeightedItem<GameObject>> Enemies;

    public static event Action<string> EnemySpawned;

    private void Awake()
    {
        FindReferences();
    }

    private void FindReferences()
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
        }
        else
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (gameManager.RoomLevel == 1)
        {
            Instantiate(tutorialEnemy, transform.position, transform.rotation);
        }
        else if (PlayerPrefs.HasKey("EnemyName"))
        {
            string loadedEnemy = PlayerPrefs.GetString("EnemyName");
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].Item.GetComponent<EnemyStats>().EnemyName == loadedEnemy)
                {
                    print("Spawning saved enemy");
                    Instantiate(Enemies[i].Item, transform.position, transform.rotation);
                    break;
                }
            }
        }
        else
        {
            var spawnedEnemy = Instantiate(GetRandomEnemy(Enemies), transform.position, transform.rotation);
            EnemySpawned?.Invoke(spawnedEnemy.GetComponent<EnemyStats>().EnemyName);
        }
    }

    private GameObject GetRandomEnemy(List<WeightedItem<GameObject>> weightedList)
    {
        var totalWeight = 0f;
        foreach (var item in weightedList)
        {
            totalWeight += item.Weight;
        }
        var randomWeight = UnityEngine.Random.Range(0, totalWeight);
        var processedWeight = 0f;
        foreach (var item in weightedList)
        {
            processedWeight += item.Weight;
            if (processedWeight >= randomWeight)
            {
                return item.Item;
            }
        }
        print("Random weight was higher than total weight");
        return null;
    }
}
