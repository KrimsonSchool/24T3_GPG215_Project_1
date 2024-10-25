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
            instance.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }
}
