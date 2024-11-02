using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInventory : Singleton<PlayerInventory>
{
    public int coins;

    [HideInInspector] public Gear weapon;
    [HideInInspector] public Gear armour;
    public Ability[] abilities;



    [HideInInspector] public GameObject itemPickupUi;
    [HideInInspector] public Image weaponInv;
    [HideInInspector] public Image armourInv;
    [HideInInspector] public TMPro.TextMeshProUGUI weaponStatsText;
    [HideInInspector] public TMPro.TextMeshProUGUI armourStatsText;

    public Sprite weaponSprite;
    public Sprite armourSprite;

    public GameObject armourSlot;
    public GameObject weaponSlot;

    PlayerStats playerStats;

    protected override void Awake()
    {
        base.Awake();
        LoadDependecies();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadDependecies();
    }

    void Update()
    {
        //print(itemPickupUi.activeSelf);
        //float percentage = ((health * 1.0f) / maxHealth);
        //float barWidth = 1080 * percentage;

        //print(percentage);
        //print(barWidth);

        /*if (xp >= xpMax)
        {
            xp -= xpMax;
            xpMax = Mathf.RoundToInt(xpMax * 1.25f);
            level++;
            maxHealth += 5;
        }*/

        /*defence;
          health;
          abilityCooldown;
          blockAmount;
          dodgeSpeed;*/



        if (weaponInv.gameObject.activeSelf && weapon != null)
        {
            weaponInv.sprite = weapon.icon;
            weaponStatsText.text = "Attack: " + weapon.damage + "\nAttack Speed: " + weapon.attackSpeed;
            //"\nCrit Chance: " + weapon.critChance + "%\nCrit Amount: " + weapon.critAmount + 
        }

        if (armourInv.gameObject.activeSelf && armour != null)
        {
            armourInv.sprite = armour.icon;
            armourStatsText.text = "Defence: " + armour.defence + "\nHealth: " + armour.health + "\nBlock Amount: " + armour.blockAmount + "\nDodge Speed: " + armour.dodgeSpeed;
            //"\nAbility Cooldown: " + armour.abilityCooldown +
        }

        if (weapon != null)
        {
            playerStats.AttackDamage = 1 + weapon.damage;

            playerStats.AttackSpeed = 0.2f - (weapon.attackSpeed * 0.01f);
            playerStats.AttackRecovery = 0.4f - (weapon.attackSpeed * 0.01f);
        }

        if (armour != null)
        {
            if (playerStats.MaxHealth != Mathf.RoundToInt(10 + armour.health))
            {
                var previousMaxHealth = playerStats.MaxHealth;
                playerStats.MaxHealth = Mathf.RoundToInt(10 + armour.health);
                playerStats.CurrentHealth = Mathf.Clamp(playerStats.CurrentHealth + (playerStats.MaxHealth - previousMaxHealth), 1, int.MaxValue);
            }

            playerStats.DodgeWindow = 0.4f * (1 + (armour.dodgeSpeed * 0.05f));
            playerStats.DodgeRecovery = Mathf.Clamp(0.8f - playerStats.DodgeWindow, 0, 1f);

            playerStats.DamageResistance = armour.defence;
            playerStats.BlockRecovery = 0.4f / armour.blockAmount;
        }
    }

    public void LoadDependecies()
    {
        //!!! CHECK IF ALL EXIST PLS!!!
        //The true in the below find obejcts means it will find disabled objects
        itemPickupUi = FindObjectOfType<ItemPickup>(true).gameObject;

        weaponInv = FindObjectOfType<MenuManager>().weaponInv.GetComponent<Image>();
        armourInv = FindObjectOfType<MenuManager>().armourInv.GetComponent<Image>();

        weaponStatsText = FindObjectOfType<MenuManager>().weaponStatsText;
        armourStatsText = FindObjectOfType<MenuManager>().armourStatsText;

        playerStats = GetComponent<PlayerStats>();
    }
}
