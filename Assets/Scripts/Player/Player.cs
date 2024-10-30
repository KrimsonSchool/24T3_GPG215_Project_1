using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PersistentSingleton<Player>
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            Instance.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation); // Sets the player in the same position
        }
        base.Awake();
    }
}
