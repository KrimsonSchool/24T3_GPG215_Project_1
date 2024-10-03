using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    [HideInInspector] public Gear gear;
    public TMPro.TextMeshProUGUI itemText;
    public Image itemImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        itemImage.sprite = gear.icon;
        itemText.text = "Congrats!\nYou picked up ["+gear.name+"]";
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

        gameObject.SetActive(false);
    }
}
