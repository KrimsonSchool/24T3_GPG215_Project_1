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

    [HideInInspector]
    public PlayerInventory inventory;

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
        FindObjectOfType<MenuManager>().openMenus++;
        Time.timeScale = 0;
        inventory = FindObjectOfType<PlayerInventory>();

        if (inventory.weapon != null)
        {
            damageOffset = (gear.damage - inventory.weapon.damage);
            //critOffset = gear.critChance - inventory.weapon.critChance;
            //critAOffset = gear.critAmount - inventory.weapon.critAmount;
            attackSpeedOffset = gear.attackSpeed - inventory.weapon.attackSpeed;

            wpm = (damageOffset + critAOffset + critOffset + attackSpeedOffset) / 4;
        }
        if (inventory.armour != null)
        {
            defenceOffset = gear.defence - inventory.armour.defence;
            healthOffset = gear.health - inventory.armour.health;
            //abilityCooldownOffset = gear.abilityCooldown - inventory.armour.abilityCooldown;
            blockAmountOffset = gear.blockRecovery - inventory.armour.blockRecovery;
            dodgeSpeedOffset = gear.dodgeSpeed - inventory.armour.dodgeSpeed;

            apm = (defenceOffset + healthOffset + abilityCooldownOffset + blockAmountOffset + dodgeSpeedOffset) / 5;
        }
    }

    private string ColourStatString(float equipped, float pickup, string stat)
    {
        if (equipped < pickup)
        {
            return $"<color=green>+{pickup - equipped} {stat}</color>";
        }
        else if (equipped > pickup)
        {
            return $"<color=red>-{equipped - pickup} {stat}</color>";
        }
        else
        {
            return $"<color=white>+{equipped - pickup} {stat}</color>";
        }
    }

    // Update is called once per frame
    void Update()
    {
        itemImage.sprite = gear.icon;
        itemText.text = "Congrats! You found\n[" + gear.name + "]";

        if (gear.type == Gear.GearType.Weapon)
        {
            DEBUG_dmg = gear.damage;
            if (inventory.weapon == null)
            {
                statsText.text = ColourStatString(1, gear.damage, "Attack Damage") + "\n" +
                    ColourStatString(0, gear.attackSpeed, "Attack Speed");
            }
            else
            {
                statsText.text = ColourStatString(inventory.weapon.damage, gear.damage, "Attack Damage") + "\n" +
                    ColourStatString(inventory.weapon.attackSpeed, gear.attackSpeed, "Attack Speed");
            }
        }
        if (gear.type == Gear.GearType.Armour)
        {
            if (inventory.armour == null)
            {
                statsText.text = ColourStatString(0, gear.health, "Health") + "\n" +
                    ColourStatString(0, gear.defence, "Defence") + "\n" +
                    ColourStatString(0, gear.blockRecovery, "Block Recovery") + "\n" +
                    ColourStatString(0, gear.dodgeSpeed, "Dodge Speed");
            }
            else
            {
                statsText.text = ColourStatString(inventory.armour.health, gear.health, "Health") + "\n" +
                    ColourStatString(inventory.armour.defence, gear.defence, "Defence") + "\n" +
                    ColourStatString(inventory.armour.blockRecovery, gear.blockRecovery, "Block Recovery") + "\n" +
                    ColourStatString(inventory.armour.dodgeSpeed, gear.dodgeSpeed, "Dodge Speed");
            }
        }
    }

    public void Equip()
    {
        print("Equipping [" + gear.name + "]");
        FindObjectOfType<FxPlayer>().PlaySound("Equip");

        gear.gameObject.SetActive(true);
        gear.gameObject.transform.parent = inventory.gameObject.transform;
        if (gear.type == Gear.GearType.Weapon)
        {
            //print("Equipping [" + gear + "] to [" + inventory + "]");
            inventory.weapon = gear;

            Destroy(inventory.equippedWeapon);

            inventory.equippedWeapon = gear.gameObject;
            gear.gameObject.transform.position = inventory.weaponSlot.transform.position;
            gear.gameObject.transform.rotation = inventory.weaponSlot.transform.rotation;
            gear.gameObject.transform.localScale = inventory.weaponSlot.transform.localScale;

            inventory.weaponSkin = gear.skinID;
        }
        else if (gear.type == Gear.GearType.Armour)
        {
            inventory.armour = gear;

            Destroy(inventory.equippedArmour);

            inventory.equippedArmour = gear.gameObject;
            gear.gameObject.transform.position = inventory.armourSlot.transform.position;
            gear.gameObject.transform.rotation = inventory.armourSlot.transform.rotation;
            gear.gameObject.transform.localScale = inventory.armourSlot.transform.localScale;

            inventory.armourSkin = gear.skinID;
        }
        else
        {
            Debug.LogError("Non valid gear type");
        }

        FindObjectOfType<MenuManager>().openMenus--;
        gameObject.SetActive(false);

        FindObjectOfType<PlayerInventory>().LoadStats();
    }

    public void Discard()
    {
        FindObjectOfType<FxPlayer>().PlaySound("Discard");

        Destroy(gear.gameObject);
        FindObjectOfType<MenuManager>().openMenus--;
        gameObject.SetActive(false);
    }
}
