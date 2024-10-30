using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeToDodge : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
        }
        else
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void Start()
    {
        if (gameManager.RoomLevel != 1)
        {
            gameObject.SetActive(false);
        }
    }
}
