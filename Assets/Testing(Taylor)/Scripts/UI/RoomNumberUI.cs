using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomNumberUI : MonoBehaviour
{
    private TextMeshProUGUI roomNumberText;
    private RoomLevelManager roomLevelManager;

    private void Awake()
    {
        roomNumberText = GetComponent<TextMeshProUGUI>();
        roomLevelManager = FindObjectOfType<RoomLevelManager>();
        roomNumberText.text = $"Room {roomLevelManager.RoomLevel}";
    }
}
