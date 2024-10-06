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

    PlayerInventory inventory;

    int damageOffset;
    int critOffset;
    int critAOffset;
    int pm;

    int defenceOffset;

    public int DEBUG_dmg;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<MenuManager>().openMenus ++;
        inventory = FindObjectOfType<PlayerInventory>();

        if (inventory.weapon != null)
        {
            damageOffset = (gear.damage - inventory.weapon.damage);
            critOffset = gear.critChance - inventory.weapon.critChance;
            critAOffset = gear.critAmount - inventory.weapon.critAmount;

            pm = (damageOffset + critAOffset + critOffset) / 3;
        }
        if (inventory.armour != null)
        {
            defenceOffset = gear.defence - inventory.armour.defence;
        }
    }

    // Update is called once per frame
    void Update()
    {
        itemImage.sprite = gear.icon;
        itemText.text = "Congrats!\nYou picked up [" + gear.name + "]";

        if (gear.type == Gear.GearType.Weapon)
        {
            DEBUG_dmg = gear.damage;
            if (inventory.weapon == null)
            {
                statsText.text = "+ [" + gear.damage + "] Attack\n+ [" + gear.critChance + "] Crit Chance\n+ [" + gear.critAmount + "] Crit Amount";
            }
            else
            {
                if (pm <= 0)
                {
                    statsText.color = Color.red;
                }
                statsText.text = "+ [" + (damageOffset) + "] Attack\n+ [" + (critOffset) + "] Crit Chance\n+ [" + (critAOffset) + "] Crit Amount";
            }
        }
        if (gear.type == Gear.GearType.Armour)
        {
            if (inventory.armour == null)
            {
                statsText.text = "+ [" + gear.defence + "] Defence";
            }
            else
            {
                if (defenceOffset <= 0)
                {
                    statsText.color = Color.red;
                }
                statsText.text = "+ [" + defenceOffset + "] Defence";
            }
        }
    }

    public void Equip()
    {
        if(gear.type == Gear.GearType.Weapon)
        {
            inventory.weapon = gear;
        }
        if (gear.type == Gear.GearType.Armour)
        {
            inventory.armour = gear;
        }

        FindObjectOfType<MenuManager>().openMenus--;
        gameObject.SetActive(false);
    }

    public void Discard()
    {
        Destroy(gear.gameObject);
        FindObjectOfType<MenuManager>().openMenus--;
        gameObject.SetActive(false);
    }
}
