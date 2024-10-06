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
    int attackSpeedOffset;
    int wpm;

    int defenceOffset;
    int healthOffset;
    int abilityCooldownOffset;
    int blockAmountOffset;
    int dodgeSpeedOffset;
    int apm;

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
            attackSpeedOffset = gear.attackSpeed - inventory.weapon.attackSpeed;

            wpm = (damageOffset + critAOffset + critOffset + attackSpeedOffset) / 4;
        }
        if (inventory.armour != null)
        {
            defenceOffset = gear.defence - inventory.armour.defence;
            healthOffset = gear.health - inventory.armour.health;
            abilityCooldownOffset = gear.abilityCooldown - inventory.armour.abilityCooldown;
            blockAmountOffset = gear.blockAmount - inventory.armour.blockAmount;
            dodgeSpeedOffset = gear.dodgeSpeed - inventory.armour.dodgeSpeed;

            apm = (defenceOffset + healthOffset + abilityCooldownOffset + blockAmountOffset + dodgeSpeedOffset) / 5;
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
                statsText.text = "+ [" + gear.damage + "] Attack\n+ [" + gear.critChance + "] Crit Chance\n+ [" + gear.critAmount + "] Crit Amount\n+ [" + gear.attackSpeed+"] Attack Speed";
            }
            else
            {
                if (wpm <= 0)
                {
                    statsText.color = Color.red;
                }
                statsText.text = "+ [" + (damageOffset) + "] Attack\n+ [" + (critOffset) + "] Crit Chance\n+ [" + (critAOffset) + "] Crit Amount\n+ ["+attackSpeedOffset+"] Attack Speed";
            }
        }
        if (gear.type == Gear.GearType.Armour)
        {
            if (inventory.armour == null)
            {
                statsText.text = "+ [" + gear.defence + "] Defence\n+ ["+gear.health+"] Health\n+ ["+gear.abilityCooldown+"] Ability Cooldown\n+ [" + gear.blockAmount+"] Block Amount\n+ ["+gear.dodgeSpeed+"] Dodge Speed";
            }
            else
            {
                if (defenceOffset <= 0)
                {
                    statsText.color = Color.red;
                }
                statsText.text = "+ [" + defenceOffset + "] Defence\n+ [" + healthOffset + "] Health\n+ [" + abilityCooldownOffset + "] Ability Cooldown\n+ [" + blockAmountOffset + "] Block Amount\n+ [" + dodgeSpeedOffset + "] Dodge Speed";
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
