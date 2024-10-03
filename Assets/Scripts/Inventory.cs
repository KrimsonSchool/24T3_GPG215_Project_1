using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
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


    [HideInInspector] public RectMask2D healthBar;
    [HideInInspector] public Slider xpBar;
    [HideInInspector] public TMPro.TextMeshProUGUI xpText;
    [HideInInspector] public TMPro.TextMeshProUGUI hpText;
    [HideInInspector] public TMPro.TextMeshProUGUI levelText;
    [HideInInspector] public GameObject itemPickupUi;
    [HideInInspector] public Image weaponInv;
    [HideInInspector] public Image armourInv;

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
        float percentage = ((health*1.0f) / maxHealth);
        float barWidth = 1080*percentage;

        //print(percentage);
        //print(barWidth);

        

        healthBar.padding = new Vector4(0, 0, 1080 - barWidth, 0);
        xpBar.maxValue = xpMax;
        xpBar.value = xp;

        xpText.text = "XP: " + xp + "/" + xpMax;
        hpText.text = "HP: " + health + "/" + maxHealth;
        levelText.text = "Level: " + level;

        if (xp >= xpMax)
        {
            xp-=xpMax;
            xpMax = Mathf.RoundToInt(xpMax * 1.25f);
            level++;
            maxHealth+=5;
        }

        if (weaponInv.gameObject.activeSelf && weapon!=null)
        {
            weaponInv.sprite = weapon.icon;
        }

        if (armourInv.gameObject.activeSelf&&armour!=null)
        {
            armourInv.sprite = armour.icon;
        }
    }

    //these names are case sensitive, if you want to rename a UI element please update it here!
    public void LoadDependecies()
    {
        healthBar = GameObject.Find("HealthBarR2D").GetComponent<RectMask2D>();
        xpBar = GameObject.Find("XpBar").GetComponent<Slider>();
        xpText = GameObject.Find("XpText").GetComponent<TMPro.TextMeshProUGUI>();
        hpText = GameObject.Find("HpText").GetComponent<TMPro.TextMeshProUGUI>();
        levelText = GameObject.Find("LevelText").GetComponent<TMPro.TextMeshProUGUI>();
        //The true in the below find obejcts means it will find disabled objects
        itemPickupUi = FindObjectOfType<ItemPickup>(true).gameObject;

        weaponInv = FindObjectOfType<MenuManager>().weaponInv.GetComponent<Image>();
        armourInv = FindObjectOfType<MenuManager>().armourInv.GetComponent<Image>();
    }
}
