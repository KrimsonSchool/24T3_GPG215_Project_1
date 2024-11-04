using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitToMenuButton : MonoBehaviour
{
    public void DestroyAllSingletons()
    {
        DestroyGameManager();
        DestroySaveManager();
        DestroyPlayerSingleton();
    }

    public void DestroyGameManager()
    {
        GameManager.DestroySingleton();
    }

    public void DestroySaveManager()
    {
        SaveManager.DestroySingleton();
    }

    public void DestroyPlayerSingleton()
    {
        Player.DestroySingleton();
    }

    public void ResetSaveData()
    {
        SaveManager.Instance.ResetSaveData();
    }
}
