using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySingletons : MonoBehaviour
{
    public void DestroyAllSingletons()
    {
        DestroyGameManagerSingleton();
        DestroyPlayerSingleton();
    }

    public void DestroyGameManagerSingleton()
    {
        Destroy(GameManager.instance);
        GameManager.instance = null;
    }

    public void DestroyPlayerSingleton()
    {
        Destroy(PlayerSingleton.instance);
        PlayerSingleton.instance = null;
    }

    // This is here just as a temporary method to clear the save upon losing
    public void ClearPlayerPrefsTemp()
    {
        PlayerPrefs.DeleteAll();
    }
}
