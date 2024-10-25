using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomNumber : MonoBehaviour
{
    private TextMeshProUGUI roomNumberText;
    private GameManager gameManager;

    private void Awake()
    {
        roomNumberText = GetComponent<TextMeshProUGUI>();
        if (GameManager.instance != null)
        {
            gameManager = GameManager.instance.GetComponent<GameManager>();
        }
        else
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        roomNumberText.text = $"Room {gameManager.RoomLevel}";
    }

    private void OnEnable()
    {
        GameManager.RoomLevelChanged += OnRoomLevelChanged;
    }
    private void OnDisable()
    {
        GameManager.RoomLevelChanged -= OnRoomLevelChanged;
    }

    private void OnRoomLevelChanged(int roomLevel)
    {
        roomNumberText.text = $"Room {roomLevel}";
    }
}
