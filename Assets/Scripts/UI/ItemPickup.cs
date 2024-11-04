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
                statsText.text =
                    $"{gear.damage} Attack Damage\n" +
                    $"{gear.attackSpeed} Attack Speed";
                //+ gear.critChance + "] Crit Chance\n+ " + gear.critAmount + "] Crit Amount\n+ ["
            }
            else
            {
                if (wpm <= 0)
                {
                    statsText.color = Color.red;
                }
                statsText.text =
                    $"{damageOffset} Attack Damage\n" +
                    $"{attackSpeedOffset} Attack Speed";
                //\n+ [" + (critOffset) + "] Crit Chance\n+ [" + (critAOffset) + "] Crit Amount
            }
        }
        if (gear.type == Gear.GearType.Armour)
        {
            if (inventory.armour == null)
            {
                statsText.text =
                    $"{gear.health} Health\n" +
                    $"{gear.defence} Defence\n" +
                    $"{gear.blockRecovery} Block Recovery\n" +
                    $"{gear.dodgeSpeed} Dodge Speed";
                //gear.abilityCooldown+"] Ability Cooldown\n+ [" +
            }
            else
            {
                if (defenceOffset <= 0)
                {
                    statsText.color = Color.red;
                }
                statsText.text =
                    $"{healthOffset} Health\n" +
                    $"{defenceOffset} Defence\n" +
                    $"{blockAmountOffset} Block Recovery\n" +
                    $"{dodgeSpeedOffset} Dodge Speed";
                //\n+ [" + abilityCooldownOffset + "] Ability Cooldown
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

            gear.gameObject.transform.position = inventory.weaponSlot.transform.position;
            gear.gameObject.transform.rotation = inventory.weaponSlot.transform.rotation;
            gear.gameObject.transform.localScale = inventory.weaponSlot.transform.localScale;
        }
        else if (gear.type == Gear.GearType.Armour)
        {
            inventory.armour = gear;

            gear.gameObject.transform.position = inventory.armourSlot.transform.position;
            gear.gameObject.transform.rotation = inventory.armourSlot.transform.rotation;
            gear.gameObject.transform.localScale = inventory.armourSlot.transform.localScale;
        }
        else
        {
            Debug.LogError("Non valid gear type");
        }

        FindObjectOfType<MenuManager>().openMenus--;
        gameObject.SetActive(false);
    }

    public void Discard()
    {
        FindObjectOfType<FxPlayer>().PlaySound("Discard");

        Destroy(gear.gameObject);
        FindObjectOfType<MenuManager>().openMenus--;
        gameObject.SetActive(false);
    }
}
