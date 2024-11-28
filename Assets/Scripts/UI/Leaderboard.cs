using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    string leaderboardKey = "globalHighscore";
    public TextMeshProUGUI playerNames;
    public TextMeshProUGUI playerScores;
    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        bool done = false;
        string playerID = PlayerPrefs.GetString("PlayerID");
        LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardKey, (response) =>
        {
            if (response.success)
            {
                print("Uploaded Score!");
                done = true;
            }
            else
            {
                Debug.LogError("Score Upload Failed! " + response.errorData);
                done = true;
            }
        });
        while (!done)
        {
            yield return null;
        }
        yield return true;
    }

    public IEnumerator FetchTopHighScoresRoutine()
    {
        bool done = false;
        LootLockerSDKManager.GetScoreList(leaderboardKey, 5, 0, (response) =>
        {
            if (response.success)
            {
                string tempPlayerNames = "Names\n";
                string tempPlayerScores = "Scores\n";

                LootLockerLeaderboardMember[]members = response.items;
                
                for (int i = 0; i<members.Length; i++)
                {
                    tempPlayerNames += members[i].rank + ". ";
                    if (members[i].player.name != "")
                    {
                        tempPlayerNames += members[i].player.name;
                    }
                    else
                    {
                        tempPlayerNames += members[i].player.id;
                    }
                    tempPlayerScores+= members[i].score+"\n";
                    tempPlayerNames += "\n";
                }
                
                print("Got Leaderboard Scores!");
                done = true;
                playerNames.text = tempPlayerNames;
                playerScores.text = tempPlayerScores;
            }
            else
            {
                Debug.LogError("Failed to get scores! "+response.errorData);
                SceneManager.LoadSceneAsync("Login");
                done= true;
            }
        });
        while (!done)
        {
            yield return null;
        }
        yield return true;
    }
}
