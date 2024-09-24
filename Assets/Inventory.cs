using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int coins;
    public int health;
    public int maxHealth;
    public RectMask2D healthBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //not working yet, please hold on...
        float percentage = (health / maxHealth) * 100;
        float barWidth = 1080/percentage;

        

        healthBar.padding = new Vector4(0,barWidth,0,0);
    }
}
