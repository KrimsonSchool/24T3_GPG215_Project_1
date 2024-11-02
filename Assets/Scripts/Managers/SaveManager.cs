using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : PersistentSingleton<SaveManager>
{
    public GameObject gearPrefab;
    GameManager gameManager;
    PlayerStats playerStats;
    PlayerInventory playerInventory;

    #region Initialization
    protected override void Awake()
    {
        base.Awake();
        FindReferences();
    }

    private void FindReferences()
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
        }
        else
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (Player.Instance != null)
        {
            playerStats = PlayerStats.Instance;
            playerInventory = PlayerInventory.Instance;
        }
        else
        {
            playerStats = FindObjectOfType<PlayerStats>();
            playerInventory = FindObjectOfType<PlayerInventory>();
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
            //PlayerPrefs.SetInt("WeaponCritChance", playerInventory.weapon.critChance);
            //PlayerPrefs.SetInt("WeaponCritAmount", playerInventory.weapon.critAmount);
        }

        if (playerInventory.armour != null)
        {
            PlayerPrefs.SetInt("HasArmour", 1);

            PlayerPrefs.SetInt("ArmourDefence", playerInventory.armour.defence);
            PlayerPrefs.SetInt("ArmourHealth", playerInventory.armour.health);
            //PlayerPrefs.SetInt("ArmourAbilityCooldown", playerInventory.armour.abilityCooldown);
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

        ItemPickup ip = FindObjectOfType<ItemPickup>(true);
        ip.inventory = FindObjectOfType<PlayerInventory>();

        if (PlayerPrefs.GetInt("HasWeapon") == 1)
        {
            print("Has weapon, loading...");
            //spawn gear
            Gear gear = Instantiate(gearPrefab, transform).GetComponent<Gear>();
            gear.icon = playerInventory.weaponSprite;

            //give saved stats
            gear.type = Gear.GearType.Weapon;
            gear.damage = PlayerPrefs.GetInt("WeaponDamage");
            gear.attackSpeed = PlayerPrefs.GetInt("WeaponAttackSpeed");
            //gear.critChance = PlayerPrefs.GetInt("WeaponCritChance");
            //gear.critAmount = PlayerPrefs.GetInt("WeaponCritAmount");
            gear.name = "Weapon";
            //equip
            
            //print("Setting ip's gear");
            //ip.gear = gear;
            //print("ip's gear: " + ip.gear);
            gear.gameObject.SetActive(false);
            FindObjectOfType<MenuManager>().openMenus++;
            //ip.gear.drop = false;
            gear.drop = false;
            ip.gear = gear;
            ip.Equip();
        }
        if (PlayerPrefs.GetInt("HasArmour") == 1)
        {
            print("Has armour, loading...");
            //spawn gear
            Gear gear = Instantiate(gearPrefab, transform).GetComponent<Gear>();
            gear.icon = playerInventory.armourSprite;

            //give saved stats
            gear.type = Gear.GearType.Armour;
            gear.defence = PlayerPrefs.GetInt("ArmourDefence");
            gear.health = PlayerPrefs.GetInt("ArmourHealth");
            //gear.abilityCooldown = PlayerPrefs.GetInt("ArmourAbilityCooldown");
            gear.blockAmount = PlayerPrefs.GetInt("ArmourBlockAmount");
            gear.dodgeSpeed = PlayerPrefs.GetInt("ArmourDodgeSpeed");
            gear.name = "Armour";

            //equip
            //ip.gear = gear;
            gear.gameObject.SetActive(false);
            FindObjectOfType<MenuManager>().openMenus++;
            gear.drop = false;
            ip.gear = gear;
            ip.Equip();
        }
    }
}
