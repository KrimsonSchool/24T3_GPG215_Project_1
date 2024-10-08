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
    // Start is called before the first frame update
    void Start()
    {
        
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
