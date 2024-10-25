using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRoomNumber : MonoBehaviour
{
    private TextMeshProUGUI roomNumberText;
    private GameManager gameManager;

    private void Awake()
    {
        roomNumberText = GetComponent<TextMeshProUGUI>();
        gameManager = FindObjectOfType<GameManager>();
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
