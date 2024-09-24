using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int coins;
    public int xp;
    public int xpMax;

    public int health;
    public int maxHealth;
    public RectMask2D healthBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        health = maxHealth;
        healthBar = GameObject.Find("HealthBarR2D").GetComponent<RectMask2D>();

    }
    void OnLevelWasLoaded()
    {
        healthBar = GameObject.Find("HealthBarR2D").GetComponent<RectMask2D>();
    }
    // Update is called once per frame
    void Update()
    {
        //not working yet, please hold on...
        float percentage = ((health*1.0f) / maxHealth);
        float barWidth = 1080*percentage;

        print(percentage);
        print(barWidth);

        

        healthBar.padding = new Vector4(0, 0, 1080 - barWidth, 0);
    }
}
