using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    #region Fields
    [SerializeField] private int roomLevel = 1;
    #endregion

    #region Events
    public static event Action RoomTransitionStarting;

    /// <summary>
    /// 1. &lt;int&gt; : RoomLevel
    /// </summary>
    public static event Action<int> RoomLevelChanged;
    #endregion

    #region Properties
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
    #endregion

    #region Initialization & Decommission
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EnemyLootTable.DroppedLoot += StartLootPhase;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //print($"Started room {roomLevel}");
        if (scene.name == "DefaultRoom" || scene.name == "BossRoom")
        {
            RoomLevelChanged?.Invoke(RoomLevel);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EnemyLootTable.DroppedLoot -= StartLootPhase;
    }
    #endregion

    public void StartMoveToNextRoom()
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
        RoomTransitionStarting?.Invoke();
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
