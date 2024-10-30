using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    //need to implement stats!!

    //dropdown - GearType(helmet)
    //Dropdown - Teir(1)

    public GearType type = new GearType();
    public int teir = 1;
    public int weight = 1;


    

    [HideInInspector]public bool drop = true;
    float timer;
    PlayerInventory player;

    public Sprite icon;

    [HideInInspector] public bool ForceGearType = false;

    [Header("Stats")]
    [HideInInspector] public int defence;
    [HideInInspector] public int health;
    [HideInInspector] public int abilityCooldown;
    [HideInInspector] public int blockAmount;
    [HideInInspector] public int dodgeSpeed;


    [HideInInspector] public int damage;
    [HideInInspector] public int critChance;
    [HideInInspector] public int critAmount;
    [HideInInspector] public int attackSpeed;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        player = FindObjectOfType<PlayerInventory>();

        if (!ForceGearType)
        {
            if (Random.Range(0, 2) == 0)
            {
                type = Gear.GearType.Weapon;
            }
            else
            {
                type = Gear.GearType.Armour;
            }
        }

        if (type == GearType.Armour)
        {
            name = "Armour Tier " + teir;
            icon = player.armourSprite;
            defence = Random.Range(teir, teir + 3);
            health = Random.Range(teir, teir + 3);
            abilityCooldown = Random.Range(teir, teir + 3);
            blockAmount = Random.Range(teir, teir + 3);
            dodgeSpeed = Random.Range(teir, teir + 3);
        }

        if(type == GearType.Weapon)
        {
            name = "Weapon Tier " + teir;
            icon = player.weaponSprite;
            damage = Random.Range(teir, teir + 3);
            attackSpeed = Random.Range(teir, teir + 3);
            critChance = Random.Range(teir, teir*20);
            critAmount = damage * teir;
        }

        GetComponent<SpriteRenderer>().sprite = icon;
    }

    // Update is called once per frame
    void Update()
    {
        if (drop)
        {
            timer += Time.deltaTime;
        }

        //after 0.2 seconds
        if (timer >= 0.2)
        {
            //destroy own rigidbody
            Destroy(GetComponent<Rigidbody>());
            //destroy own collider
            Destroy(GetComponent<Collider>());
            //move toward cam camera
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 100);

            //if within 1 unit of cam camera then
            if (Vector3.Distance(transform.position, player.transform.position) < 1)
            {
                //add to inv, show on cam
                drop = false;
                timer = 0;

                player.itemPickupUi.SetActive(true);
                player.itemPickupUi.GetComponent<ItemPickup>().gear = GetComponent<Gear>();
                gameObject.SetActive(false);
                //transform.position = player.weaponSlot.transform.position;
                //transform.rotation = player.weaponSlot.transform.rotation;
            }
        }
    }
    public enum GearType
    {
        Armour,
        Weapon
    };
}
