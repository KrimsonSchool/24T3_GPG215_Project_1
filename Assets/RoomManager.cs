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
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }


        LoadPrerequisites();
    }

    void OnLevelWasLoaded()
    {
        print("Level Loaded");
        LoadPrerequisites();
    }

    // Update is called once per frame
    void Update()
    {
        roomText.text = "Room: " + room;
        currencyText.text = "Coins: " + inventory.coins;
        if (enemyDead)
        {
            camera.GetComponent<Animator>().enabled = true;
            player.animator.SetBool("MoveOn", true);

            timer += Time.deltaTime;

            if (timer > 1.5f)
            {
                MoveToNextRoom();
            }
        }
    }

    public void MoveToNextRoom()
    {
        room++;
        enemyDead = false;
        timer = 0;
        SceneManager.LoadScene(nextScene);
    }

    public void LoadPrerequisites()
    {
        camera = FindObjectOfType<Camera>().gameObject;
        player = FindObjectOfType<Player>();
        roomText = GameObject.Find("RoomText").GetComponent<TMPro.TextMeshProUGUI>();
        currencyText = GameObject.Find("CurrencyText").GetComponent<TMPro.TextMeshProUGUI>();
        enemy = FindObjectOfType<Enemy>();
        inventory = GetComponent<Inventory>();
    }
}
