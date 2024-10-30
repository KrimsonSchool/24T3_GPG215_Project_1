using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void OpenScene(string scene)
    {
        if (PlayerPrefs.GetInt("CanLoad") == 1 && PlayerPrefs.GetInt("Level") >= 2)
        {
            GameManager.Instance.RoomLevel = PlayerPrefs.GetInt("Level");
        }
        else
        {
            PlayerPrefs.SetInt("CanLoad", 0);
        }

        if (PlayerPrefs.GetInt("CanLoad") == 1 && PlayerPrefs.GetInt("Level") >= 10 && PlayerPrefs.GetInt("Level") % 10 == 0)
        {
            SceneManager.LoadSceneAsync("BossRoom");
        }
        else
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("CanLoad", 0);
        SceneManager.LoadSceneAsync("DefaultRoom");
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
