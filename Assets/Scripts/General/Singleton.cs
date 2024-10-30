using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; protected set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
    }

    public static void DestroySingleton(bool destroyGameObject = true)
    {
        if (Instance != null)
        {
            if (destroyGameObject)
            {
                Destroy(Instance.gameObject);
            }
            else
            {
                Destroy(Instance);
            }
            Instance = null;
        }
    }
}

public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
    }
}
