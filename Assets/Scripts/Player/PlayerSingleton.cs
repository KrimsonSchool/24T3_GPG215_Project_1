using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static GameObject instance;

    private void Awake()
    {
        if (instance != null)
        {
            instance.gameObject.transform.position = gameObject.transform.position;
            instance.gameObject.transform.rotation = gameObject.transform.rotation;
            Destroy(gameObject);
        }
        else
        {
            instance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
    }
}
