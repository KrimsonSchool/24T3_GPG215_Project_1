using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public int coins;
    public int xp;
    public int xpMax;

    public int level;

    public int health;
    public int maxHealth;

    public Gear weapon;
    public Gear armour;
    public Ability[] abilities;


    
    [HideInInspector] public GameObject itemPickupUi;
    [HideInInspector] public Image weaponInv;
    [HideInInspector] public Image armourInv;
    [HideInInspector] public TMPro.TextMeshProUGUI weaponStatsText;
    [HideInInspector] public TMPro.TextMeshProUGUI armourStatsText;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        xpMax = 25;
        maxHealth = 20;
        health = maxHealth;


        LoadDependecies();
    }
    void OnLevelWasLoaded()
    {
        LoadDependecies();
    }
    // Update is called once per frame
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

        if (weaponInv.gameObject.activeSelf && weapon != null)
        {
            weaponInv.sprite = weapon.icon;
            weaponStatsText.text = "Attack: " + weapon.damage + "\nCrit Chance: " + weapon.critChance + "%\nCrit Amount: " + weapon.critAmount;
        }

        if (armourInv.gameObject.activeSelf && armour != null)
        {
            armourInv.sprite = armour.icon;
        }

        if (weapon != null)
        {
            FindObjectOfType<PlayerStats>().AttackDamage = 1 + weapon.damage;
        }
    }

    public void LoadDependecies()
    {
        //The true in the below find obejcts means it will find disabled objects
        itemPickupUi = FindObjectOfType<ItemPickup>(true).gameObject;

        weaponInv = FindObjectOfType<MenuManager>().weaponInv.GetComponent<Image>();
        armourInv = FindObjectOfType<MenuManager>().armourInv.GetComponent<Image>();

        weaponStatsText = FindObjectOfType<MenuManager>().weaponStatsText;
        armourStatsText = FindObjectOfType<MenuManager>().armourStatsText;
    }
}
