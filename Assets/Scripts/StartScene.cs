using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenScene(string scene)
    {
        if (PlayerPrefs.GetInt("Level") % 10 != 0 && PlayerPrefs.GetInt("Level") != 0)
        {
            SceneManager.LoadSceneAsync(scene);
        }
        else
        {
            SceneManager.LoadSceneAsync("BossRoom");
        }
    }

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }
    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
    }
}
