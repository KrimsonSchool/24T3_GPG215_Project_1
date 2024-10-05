using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLevelManager : MonoBehaviour
{
    public static GameObject instance;

    private int roomLevel = 1;

    public static event Action RoomLevelChanging;

    public int RoomLevel {  get { return roomLevel; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        EnemyCombatHandler.EnemyDeadEvent += EnemyLootStage;
    }

    private void OnDisable()
    {
        EnemyCombatHandler.EnemyDeadEvent -= EnemyLootStage;
    }

    private void EnemyLootStage()
    {
        // This is just here to move onto next room while working on a loot stage so that there is a playable game loop
        StartCoroutine(MoveToNextRoom());
    }

    private IEnumerator MoveToNextRoom()
    {
        RoomLevelChanging?.Invoke();
        yield return new WaitForSeconds(1.5f);
        roomLevel++;
        SceneManager.LoadScene("DefaultRoom");
    }
}
