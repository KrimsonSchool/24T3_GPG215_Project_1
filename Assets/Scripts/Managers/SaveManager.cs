using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    bool cansave = true;
    bool canLoad;
    PlayerStats ps;
    GameManager gm;
    PlayerInventory inv;

    public GameObject gearPrefab;

    void Awake()
    {
        //PlayerPrefs.SetInt("HasWeapon", 0);
        //PlayerPrefs.SetInt("HasArmour", 0);

        ps = GetComponent<PlayerStats>();
        inv = GetComponent<PlayerInventory>();
        if (GameManager.instance == null)
            gm = FindObjectOfType<GameManager>();
        else
            gm = GameManager.instance.GetComponent<GameManager>();


        if ((PlayerSingleton.instance == null || PlayerSingleton.instance == gameObject) && PlayerPrefs.GetInt("CanLoad") == 1 && PlayerPrefs.GetInt("Health") > 0)
        {
            Load();
        }
    }

    private void Start()
    {
        PlayerPrefs.SetInt("CanLoad", 1);
        Save();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.StartRoomTransition += Save;
        PlayerStats.HealthValueChangedEvent += CanLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.StartRoomTransition -= Save;
        PlayerStats.HealthValueChangedEvent -= CanLoad;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cansave = true;
    }

    private void CanLoad(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            PlayerPrefs.SetInt("CanLoad", 0);
        }
    }

    public void Save()
    {
        if (cansave)
        {
            print("Saving cs: " + cansave);

            PlayerPrefs.SetInt("Health", ps.CurrentHealth);
            PlayerPrefs.SetInt("AttackDamage", ps.AttackDamage);
            PlayerPrefs.SetInt("Level", gm.RoomLevel);

            if (inv.weapon != null)
            {
                PlayerPrefs.SetInt("HasWeapon", 1);
                PlayerPrefs.SetInt("WeaponDamage", inv.weapon.damage);
                PlayerPrefs.SetInt("WeaponAttackSpeed", inv.weapon.attackSpeed);
                PlayerPrefs.SetInt("WeaponCritChance", inv.weapon.critChance);
                PlayerPrefs.SetInt("WeaponCritAmount", inv.weapon.critAmount);
            }
            if (inv.armour != null)
            {
                PlayerPrefs.SetInt("HasArmour", 1);
                PlayerPrefs.SetInt("ArmourDefence", inv.armour.defence);
                PlayerPrefs.SetInt("ArmourHealth", inv.armour.health);
                PlayerPrefs.SetInt("ArmourAbilityCooldown", inv.armour.abilityCooldown);
                PlayerPrefs.SetInt("ArmourBlockAmount", inv.armour.blockAmount);
                PlayerPrefs.SetInt("ArmourDodgeSpeed", inv.armour.dodgeSpeed);
            }
        }
        cansave = false;
    }

    public void Load()
    {
        ps.CurrentHealth = PlayerPrefs.GetInt("Health");
        ps.AttackDamage = PlayerPrefs.GetInt("AttackDamage");
        //gm.RoomLevel = PlayerPrefs.GetInt("Level");

        if (PlayerPrefs.GetInt("HasWeapon") == 1)
        {
            print("Has weapon, loading...");
            //spawn gear
            Gear gear = Instantiate(gearPrefab, transform).GetComponent<Gear>();
            gear.icon = inv.weaponSprite;

            //give saved stats
            gear.type = Gear.GearType.Weapon;
            gear.damage = PlayerPrefs.GetInt("WeaponDamage");
            gear.attackSpeed = PlayerPrefs.GetInt("WeaponAttackSpeed");
            gear.critChance = PlayerPrefs.GetInt("WeaponCritChance");
            gear.critAmount = PlayerPrefs.GetInt("WeaponCritAmount");

            //equip
            ItemPickup ip = FindObjectOfType<ItemPickup>(true);
            print("Setting ip's gear");
            ip.gear = gear;
            print("ip's gear: " + ip.gear);
            gear.gameObject.SetActive(false);
            ip.inventory = FindObjectOfType<PlayerInventory>();
            FindObjectOfType<MenuManager>().openMenus++;
            ip.gear.drop = false;
            ip.Equip();
        }
        if (PlayerPrefs.GetInt("HasArmour") == 1)
        {
            print("Has weapon, loading...");
            //spawn gear
            Gear gear = Instantiate(gearPrefab, transform).GetComponent<Gear>();
            gear.icon = inv.armourSprite;

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
