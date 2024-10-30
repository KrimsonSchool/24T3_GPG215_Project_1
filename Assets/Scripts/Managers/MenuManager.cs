using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    GameManager gameManager;

    public TMPro.TextMeshProUGUI madeItToText;

    void Awake()
    {
        if (PlayerSingleton.instance != null)
        {
            playerStats = PlayerSingleton.instance.GetComponent<PlayerStats>();
        }
        else
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }
        if (GameManager.instance != null)
        {
            gameManager = GameManager.instance.GetComponent<GameManager>();
        }
        else
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (openMenus == 0)
        {
            Time.timeScale = 1;
        }
        else if(openMenus > 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Debug.LogError("Less than 0 menus are open");
        }

        //print("Current Health: "+playerStats.CurrentHealth);
        //if (playerStats.CurrentHealth <= 0 && !deathScreen.activeSelf)
        //{
        //    deathScreen.SetActive(true);
        //    madeItToText.text = "Made it to room " + gameManager.RoomLevel;
        //    openMenus++;
        //}
    }

    private void OnEnable()
    {
        PlayerStats.PlayerDiedEvent += CheckForPlayerDeath;
    }

    private void OnDisable()
    {
        PlayerStats.PlayerDiedEvent -= CheckForPlayerDeath;
    }

    private void CheckForPlayerDeath()
    {
        if (playerStats.CurrentHealth <= 0 && !deathScreen.activeSelf)
        {
            deathScreen.SetActive(true);
            madeItToText.text = "Made it to room " + gameManager.RoomLevel;
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

    public void ChangeScene(string scene)
    {
        //FindObjectOfType<SaveManager>().Save();
        //Destroy(FindObjectOfType<SaveManager>().gameObject);
        //SceneManager.LoadScene(scene);
        gameManager.LoadScene(scene);
    }
}
