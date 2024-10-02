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


    [HideInInspector] public RectMask2D healthBar;
    [HideInInspector] public Slider xpBar;
    [HideInInspector] public TMPro.TextMeshProUGUI xpText;
    [HideInInspector] public TMPro.TextMeshProUGUI hpText;
    [HideInInspector] public TMPro.TextMeshProUGUI levelText;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        xpMax = 25;
        maxHealth = 20;
        health = maxHealth;


        healthBar = GameObject.Find("HealthBarR2D").GetComponent<RectMask2D>();
        xpBar = GameObject.Find("XpBar").GetComponent<Slider>();
        xpText = GameObject.Find("XpText").GetComponent<TMPro.TextMeshProUGUI>();
        hpText = GameObject.Find("HpText").GetComponent<TMPro.TextMeshProUGUI>();
        levelText = GameObject.Find("LevelText").GetComponent<TMPro.TextMeshProUGUI>();

    }
    void OnLevelWasLoaded()
    {
        healthBar = GameObject.Find("HealthBarR2D").GetComponent<RectMask2D>(); 
        xpBar = GameObject.Find("XpBar").GetComponent<Slider>();
        xpText = GameObject.Find("XpText").GetComponent<TMPro.TextMeshProUGUI>();
        hpText = GameObject.Find("HpText").GetComponent<TMPro.TextMeshProUGUI>();
        levelText = GameObject.Find("LevelText").GetComponent<TMPro.TextMeshProUGUI>();

    }
    // Update is called once per frame
    void Update()
    {
        //not working yet, please hold on...
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
    }
}
