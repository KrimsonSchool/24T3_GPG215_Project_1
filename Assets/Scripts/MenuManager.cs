using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [HideInInspector]
    public int openMenus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
