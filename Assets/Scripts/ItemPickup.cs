using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    [HideInInspector] public Gear gear;
    public TMPro.TextMeshProUGUI itemText;
    public TMPro.TextMeshProUGUI statsText;
    public Image itemImage;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<MenuManager>().openMenus ++;
    }

    // Update is called once per frame
    void Update()
    {
        itemImage.sprite = gear.icon;
        itemText.text = "Congrats!\nYou picked up ["+gear.name+"]";

        if (gear.type == Gear.GearType.Weapon)
        {
            statsText.text = "+ [" + gear.damage + "] Attack\n+ [" + gear.critChance + "] Crit Chance\n+ [" + gear.critAmount + "] Crit Amount";
        }
        if (gear.type == Gear.GearType.Armour)
        {
            statsText.text = "+ [" + gear.defence + "] Defence";
        }
    }

    public void Equip()
    {
        if(gear.type == Gear.GearType.Weapon)
        {
            FindObjectOfType<Inventory>().weapon = gear;
        }
        if (gear.type == Gear.GearType.Armour)
        {
            FindObjectOfType<Inventory>().armour = gear;
        }

        FindObjectOfType<MenuManager>().openMenus--;
        gameObject.SetActive(false);
    }
}
