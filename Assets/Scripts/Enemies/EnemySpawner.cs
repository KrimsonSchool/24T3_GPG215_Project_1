using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemies;
    [SerializeField] private float[] enemySpawnWeighting;

    private void Start()
    {
        float weightSum = 0f;
        for (int i = 0; i < enemySpawnWeighting.Length; i++)
        {
            weightSum += enemySpawnWeighting[i];
        }
        float randomNum = Random.Range(0, weightSum);
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
        Instantiate(Enemies[enemyIteration]);
    }
}
