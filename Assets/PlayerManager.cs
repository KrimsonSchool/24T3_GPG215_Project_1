using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoginRoutine());
        
    }

    IEnumerator LoginRoutine()
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
                Debug.LogError("Player Login Failed! "+ response.errorData);
                done = true;
            }
        });

        yield return new WaitWhile(()=>done = false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FindObjectOfType<Leaderboard>().SubmitScoreRoutine(882));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(FindObjectOfType<Leaderboard>().FetchTopHighScoresRountine());
        }
    }
}
