using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameObject instance;

    private  int roomLevel = 1;

    public static event Action RoomLevelChanging;

    public int RoomLevel {  get { return roomLevel; } set { roomLevel = value; } }

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
        EnemyLootTable.DroppedLoot += StartLootPhase;
    }

    private void OnDisable()
    {
        EnemyLootTable.DroppedLoot -= StartLootPhase;
    }

    private void StartMoveToNextRoom()
    {
        StartCoroutine(MoveToNextRoom());
    }

    private float lootingTimeThisTemporaryWillProablySwapThisToSomeEvent;

    private void StartLootPhase(bool hasLootDropped)
    {
        if (hasLootDropped)
        {
            lootingTimeThisTemporaryWillProablySwapThisToSomeEvent = 0.5f;
        }
        else
        {
            lootingTimeThisTemporaryWillProablySwapThisToSomeEvent = 0f;
        }
        StartCoroutine(MoveToNextRoom());
    }

    private IEnumerator MoveToNextRoom()
    {
        yield return new WaitForSeconds(lootingTimeThisTemporaryWillProablySwapThisToSomeEvent);
        lootingTimeThisTemporaryWillProablySwapThisToSomeEvent = 0f;
        RoomLevelChanging?.Invoke();
        yield return new WaitForSeconds(1.5f);
        roomLevel++;
        SceneManager.LoadScene("DefaultRoom");
    }
}
