using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomHeader : MonoBehaviour
{
    private TextMeshProUGUI roomHeaderText;
    private GameManager gameManager;

    private void Awake()
    {
        roomHeaderText = GetComponent<TextMeshProUGUI>();
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
            roomHeaderText.text = "The Angery Chicken";
        }
        else
        {
            roomHeaderText.text = $"Room {roomLevel}";
        }
    }
}
