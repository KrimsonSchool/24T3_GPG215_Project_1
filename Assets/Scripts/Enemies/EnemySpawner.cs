using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] allEnemies;
    [SerializeField] private GameObject[] enemiesToSpawn;
    [SerializeField] private float[] enemySpawnWeighting;
    [SerializeField] private GameObject tutorialEnemy;
    private GameManager gameManager;

    public static event Action<EnemyTypes> EnemySpawned;

    private void Awake()
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
        if (gameManager.RoomLevel == 1)
        {
            Instantiate(tutorialEnemy, transform.position, transform.rotation);
        }
        else if (PlayerPrefs.HasKey("EnemyType"))
        {
            EnemyTypes loadedEnemy = (EnemyTypes)Enum.Parse(typeof(EnemyTypes), PlayerPrefs.GetString("EnemyType"));
            for (int i = 0; i < allEnemies.Length; i++)
            {
                if (allEnemies[i].GetComponent<EnemyStats>().EnemyType == loadedEnemy)
                {
                    print("Spawning saved enemy");
                    Instantiate(allEnemies[i], transform.position, transform.rotation);
                    break;
                }
            }
        }
        else
        {
            float weightSum = 0f;
            for (int i = 0; i < enemySpawnWeighting.Length; i++)
            {
                weightSum += enemySpawnWeighting[i];
            }
            float randomNum = UnityEngine.Random.Range(0, weightSum);
            int enemyIteration = 0;
            weightSum = 0f;
            for (int i = 0; i < enemySpawnWeighting.Length; i++)
            {
                weightSum += enemySpawnWeighting[i];
                if (randomNum > weightSum)
                {
                    enemyIteration++;
                }
                else
                {
                    break;
                }
            }
            var spawnedEnemy = Instantiate(enemiesToSpawn[enemyIteration], transform.position, transform.rotation);
            EnemySpawned?.Invoke(spawnedEnemy.GetComponent<EnemyStats>().EnemyType);
        }
    }
}
