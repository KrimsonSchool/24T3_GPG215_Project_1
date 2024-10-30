using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySingletons : MonoBehaviour
{
    public void DestroyAllSingletons()
    {
        DestroyGameManager();
        DestroySaveManager();
        DestroyPlayerSingleton();
    }

    public void DestroyGameManager()
    {
        Destroy(GameManager.instance);
        GameManager.instance = null;
    }

    public void DestroySaveManager()
    {
        Destroy(SaveManager.instance);
        SaveManager.instance = null;
    }

    public void DestroyPlayerSingleton()
    {
        Destroy(PlayerSingleton.instance);
        PlayerSingleton.instance = null;
    }

    // This is here just as a temporary method to clear the save upon losing
    public void ClearPlayerPrefsTemp()
    {
        PlayerPrefs.SetInt("CanLoad", 0);
        PlayerPrefs.SetInt("Health", 0);
        //PlayerPrefs.DeleteAll();
    }
}
