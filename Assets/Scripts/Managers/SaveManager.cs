using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static GameObject instance;

    public GameObject gearPrefab;
    GameManager gameManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;

    #region Initialization
    void Awake()
    {
        SetupSingleton();
        FindReferences();
    }

    private void SetupSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void FindReferences()
    {
        if (GameManager.instance == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        else
        {
            gameManager = GameManager.instance.GetComponent<GameManager>();
        }

        if (PlayerSingleton.instance == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
            playerInventory = FindObjectOfType<PlayerInventory>();
        }
        else
        {
            playerStats = PlayerSingleton.instance.GetComponent<PlayerStats>();
            playerInventory = PlayerSingleton.instance.GetComponent<PlayerInventory>();
        }
    }

    private void OnEnable()
    {
        GameManager.StartRoomTransition += Save;
        PlayerStats.PlayerDiedEvent += PreventLoad;
    }

    private void OnDisable()
    {
        GameManager.StartRoomTransition -= Save;
        PlayerStats.PlayerDiedEvent -= PreventLoad;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("CanLoad") == 1)
        {
            Load();
        }
        else
        {
            ResetSaveData();
        }
        print("Loading now allowed...");
        PlayerPrefs.SetInt("CanLoad", 1);
    }
    #endregion

    private void PreventLoad()
    {
        print("Loading now prevented. Deleting save manager...");
        PlayerPrefs.SetInt("CanLoad", 0);
        Destroy(gameObject);
    }

    public void ResetSaveData()
    {
        print("Save data reset");

        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("AttackDamage");
        PlayerPrefs.DeleteKey("Level");


        PlayerPrefs.DeleteKey("HasWeapon");

        PlayerPrefs.DeleteKey("WeaponDamage");
        PlayerPrefs.DeleteKey("WeaponAttackSpeed");
        PlayerPrefs.DeleteKey("WeaponCritChance");
        PlayerPrefs.DeleteKey("WeaponCritAmount");


        PlayerPrefs.DeleteKey("HasArmour");

        PlayerPrefs.DeleteKey("ArmourDefence");
        PlayerPrefs.DeleteKey("ArmourHealth");
        PlayerPrefs.DeleteKey("ArmourAbilityCooldown");
        PlayerPrefs.DeleteKey("ArmourBlockAmount");
        PlayerPrefs.DeleteKey("ArmourDodgeSpeed");
    }

    public void Save()
    {
        print("Saving game...");

        PlayerPrefs.SetInt("Health", playerStats.CurrentHealth);
        PlayerPrefs.SetInt("AttackDamage", playerStats.AttackDamage);
        PlayerPrefs.SetInt("Level", gameManager.RoomLevel);

        if (playerInventory.weapon != null)
        {
            PlayerPrefs.SetInt("HasWeapon", 1);

            PlayerPrefs.SetInt("WeaponDamage", playerInventory.weapon.damage);
            PlayerPrefs.SetInt("WeaponAttackSpeed", playerInventory.weapon.attackSpeed);
            PlayerPrefs.SetInt("WeaponCritChance", playerInventory.weapon.critChance);
            PlayerPrefs.SetInt("WeaponCritAmount", playerInventory.weapon.critAmount);
        }

        if (playerInventory.armour != null)
        {
            PlayerPrefs.SetInt("HasArmour", 1);

            PlayerPrefs.SetInt("ArmourDefence", playerInventory.armour.defence);
            PlayerPrefs.SetInt("ArmourHealth", playerInventory.armour.health);
            PlayerPrefs.SetInt("ArmourAbilityCooldown", playerInventory.armour.abilityCooldown);
            PlayerPrefs.SetInt("ArmourBlockAmount", playerInventory.armour.blockAmount);
            PlayerPrefs.SetInt("ArmourDodgeSpeed", playerInventory.armour.dodgeSpeed);
        }
    }

    public void Load()
    {
        print("Loading game...");
        playerStats.CurrentHealth = PlayerPrefs.GetInt("Health");
        playerStats.AttackDamage = PlayerPrefs.GetInt("AttackDamage");
        //gm.RoomLevel = PlayerPrefs.GetInt("Level");

        if (PlayerPrefs.GetInt("HasWeapon") == 1)
        {
            //print("Has weapon, loading...");
            //spawn gear
            Gear gear = Instantiate(gearPrefab, transform).GetComponent<Gear>();
            gear.icon = playerInventory.weaponSprite;

            //give saved stats
            gear.type = Gear.GearType.Weapon;
            gear.damage = PlayerPrefs.GetInt("WeaponDamage");
            gear.attackSpeed = PlayerPrefs.GetInt("WeaponAttackSpeed");
            gear.critChance = PlayerPrefs.GetInt("WeaponCritChance");
            gear.critAmount = PlayerPrefs.GetInt("WeaponCritAmount");

            //equip
            ItemPickup ip = FindObjectOfType<ItemPickup>(true);
            //print("Setting ip's gear");
            ip.gear = gear;
            //print("ip's gear: " + ip.gear);
            gear.gameObject.SetActive(false);
            ip.inventory = FindObjectOfType<PlayerInventory>();
            FindObjectOfType<MenuManager>().openMenus++;
            ip.gear.drop = false;
            ip.Equip();
        }
        if (PlayerPrefs.GetInt("HasArmour") == 1)
        {
            //print("Has weapon, loading...");
            //spawn gear
            Gear gear = Instantiate(gearPrefab, transform).GetComponent<Gear>();
            gear.icon = playerInventory.armourSprite;

            //give saved stats
            gear.type = Gear.GearType.Armour;
            gear.defence = PlayerPrefs.GetInt("ArmourDefence");
            gear.health = PlayerPrefs.GetInt("ArmourHealth");
            gear.abilityCooldown = PlayerPrefs.GetInt("ArmourAbilityCooldown");
            gear.blockAmount = PlayerPrefs.GetInt("ArmourBlockAmount");
            gear.dodgeSpeed = PlayerPrefs.GetInt("ArmourDodgeSpeed");

            //equip
            ItemPickup ip = FindObjectOfType<ItemPickup>(true);
            ip.gear = gear;
            gear.gameObject.SetActive(false);
            ip.inventory = FindObjectOfType<PlayerInventory>();
            FindObjectOfType<MenuManager>().openMenus++;
            ip.gear.drop = false;
            ip.Equip();
        }
    }
}
