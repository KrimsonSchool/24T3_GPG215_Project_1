using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemies;

    private void Awake()
    {
        Instantiate(Enemies[Random.Range(0, Enemies.Length)]);
    }
}
