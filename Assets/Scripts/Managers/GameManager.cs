using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private int roomLevel = 1;

    #region Events
    public static event Action StartRoomTransition;

    /// <summary>
    /// &lt;int GameManager.RoomLevel&gt;
    /// </summary>
    public static event Action<int> RoomLevelChanged;
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EnemyLootTable.DroppedLoot += StartLootPhase;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EnemyLootTable.DroppedLoot -= StartLootPhase;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //print($"Started room {roomLevel}");
        RoomLevelChanged?.Invoke(RoomLevel);
    }

    public int RoomLevel
    {
        get
        {
            return roomLevel;
        }
        set
        {
            roomLevel = value;
            RoomLevelChanged?.Invoke(roomLevel);
            print($"Room level set to {roomLevel}");
        }
    }

    private void StartMoveToNextRoom()
    {
        StartCoroutine(MoveToNextRoom());
    }

    private void StartLootPhase(bool lootHasDropped)
    {
        if (lootHasDropped)
        {
            StartCoroutine(MoveToNextRoom(0.6f));
        }
        else
        {
            StartCoroutine(MoveToNextRoom());
        }
    }

    private IEnumerator MoveToNextRoom(float delayBeforeTransition = 0f)
    {
        yield return new WaitForSeconds(delayBeforeTransition);
        StartRoomTransition?.Invoke();
        yield return new WaitForSeconds(1.5f);
        roomLevel++;
        if (RoomLevel % 10 == 0)
        {
            LoadScene("BossRoom");
        }
        else
        {
            LoadScene("DefaultRoom");
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
