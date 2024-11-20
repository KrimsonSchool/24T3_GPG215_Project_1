using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public string startScene;
    public TMP_InputField username;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("PlayerName") != "")
        {
            username.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void LoginGuest()
    {
        StartCoroutine(LoginRoutine());
    }

    IEnumerator LoginRoutine()
    {
        yield return LoginRoutineGuest();
        yield return SetPlayerNameRoutine(); 
        SceneManager.LoadSceneAsync(startScene);
    }

    IEnumerator SetPlayerNameRoutine()
    {
        if (username.text != "")
        {
            bool done = false;
            LootLockerSDKManager.SetPlayerName(username.text, (response) =>
            {
                if (response.success)
                {
                    print("Set player name!");
                    done = true;
                    PlayerPrefs.SetString("PlayerName", username.text);
                }
                else
                {
                    Debug.LogError("Failed to set player name! " + response.errorData);
                    done = true;
                }
            });
            while (!done)
            {
                yield return null;
            }
            yield return true;
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator LoginRoutineGuest()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                print("Player Logged In!");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                done = true;
            }
            else
            {
                Debug.LogError("Player Login Failed! " + response.errorData);
                done = true;
            }
        });

        //yield return new WaitWhile(() => done = false);
        while (!done)
        {
            yield return null;
        }
        yield return true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartCoroutine(FindObjectOfType<Leaderboard>().SubmitScoreRoutine(882));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //StartCoroutine(FindObjectOfType<Leaderboard>().FetchTopHighScoresRountine());
        }
    }
}
