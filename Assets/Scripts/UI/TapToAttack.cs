using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToAttack : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            gameManager = GameManager.instance.GetComponent<GameManager>();
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

    private void OnEnable()
    {
        PlayerCombatControls.PlayerTapInputEvent += DisableTapToAttack;
    }

    private void OnDisable()
    {
        PlayerCombatControls.PlayerTapInputEvent -= DisableTapToAttack;
    }

    private void DisableTapToAttack()
    {
        gameObject.SetActive(false);
    }
}
