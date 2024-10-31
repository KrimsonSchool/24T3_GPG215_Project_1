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
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
        }
        else
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void OnEnable()
    {
        GameManager.RoomLevelChanged += ChangeRoomHeader;
    }
    private void OnDisable()
    {
        GameManager.RoomLevelChanged -= ChangeRoomHeader;
    }

    private void Start()
    {
        ChangeRoomHeader(gameManager.RoomLevel);
    }

    private void ChangeRoomHeader(int roomLevel)
    {
        if (roomLevel % 10 == 0)
        {
            // This'll need refactoring with boss additions
            roomNumberText.text = "The Angery Chicken";
        }
        else
        {
            roomNumberText.text = $"Room {roomLevel}";
        }
    }
}
