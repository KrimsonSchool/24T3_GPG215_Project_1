using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [HideInInspector]
    public int openMenus;//<-- broadcast to enemies pls :)

    public GameObject weaponInv;
    public GameObject armourInv;

    public TMPro.TextMeshProUGUI weaponStatsText;
    public TMPro.TextMeshProUGUI armourStatsText;

    public GameObject deathScreen;
    PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (openMenus == 0)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

        print("Current Health: "+playerStats.CurrentHealth);
        if (playerStats.CurrentHealth <= 0)
        {
            deathScreen.SetActive(true);
            openMenus++;
        }
    }
    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
        openMenus++;
    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
        openMenus--;
    }
}
