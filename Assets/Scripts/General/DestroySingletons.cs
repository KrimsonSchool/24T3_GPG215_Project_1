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
        Destroy(FindObjectOfType<GameManager>().gameObject);
        GameManager.instance = null;
    }

    public void DestroyPlayerSingleton()
    {
        Destroy(FindObjectOfType<PlayerSingleton>().gameObject);
        PlayerSingleton.instance = null;
    }

    // This is here just as a temporary method to clear the save upon losing
    public void ClearPlayerPrefsTemp()
    {
        PlayerPrefs.DeleteAll();
    }
}
