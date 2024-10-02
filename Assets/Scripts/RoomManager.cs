using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public string nextScene;
    [HideInInspector]
    public bool enemyDead;
    GameObject camera;
    Player player;
    Enemy enemy;

    float timer;

    public int room;

    private static GameObject instance;

    public TMPro.TextMeshProUGUI roomText;
    public TMPro.TextMeshProUGUI currencyText;

    public int[] enemyToSpawn;

    Inventory inventory;

    public GameObject tta;
    // Start is called before the first frame update
    void Start()
    {
        //make it so it wont get destroyed on scene change
        DontDestroyOnLoad(this);
        //check to see if this is the original
        if (instance == null)
        {
            instance = gameObject;
            tta.SetActive(true);
        }
        else
        {
            //destroy if not the original
            Destroy(gameObject);
        }

        //run LoadPrerequisites function
        LoadPrerequisites();
    }

    //when the level loads in
    void OnLevelWasLoaded()
    {
        //debug that it has loaded in
        print("Level Loaded");
        //run LoadPrerequisites function
        LoadPrerequisites();
    }

    // Update is called once per frame
    void Update()
    {
        //set the room text to be Room + the current room number
        roomText.text = "Room: " + room;
        //set the currency text to be Coins + the amount of coins
        currencyText.text = "Coins: " + inventory.coins;
        //if the rooms enemy is dead then
        if (enemyDead)
        {
            //enable the cameras animator
            camera.GetComponent<Animator>().enabled = true;
            //play the players Move On animation
            player.animator.SetBool("MoveOn", true);

            //count up a timer in seconds
            timer += Time.deltaTime;

            //after 1.5 seconds
            if (timer > 1.5f)
            {
                //run the MoveToNextRoom function
                MoveToNextRoom();
            }
        }
    }

    public void MoveToNextRoom()
    {
        //incriment the room number
        room++;
        //set enemyDead bool to false
        enemyDead = false;
        //set the timer to 0
        timer = 0;
        //load the next scene
        SceneManager.LoadScene(nextScene);
    }

    public void LoadPrerequisites()
    {
        //this function grabs all the required prerequisites from the current scene
        camera = FindObjectOfType<Camera>().gameObject;
        player = FindObjectOfType<Player>();
        roomText = GameObject.Find("RoomText").GetComponent<TMPro.TextMeshProUGUI>();
        currencyText = GameObject.Find("CurrencyText").GetComponent<TMPro.TextMeshProUGUI>();
        enemy = FindObjectOfType<Enemy>();
        inventory = GetComponent<Inventory>();
    }
}
