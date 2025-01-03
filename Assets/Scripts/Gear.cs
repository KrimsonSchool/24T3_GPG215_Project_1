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


    

    bool drop = true;
    float timer;
    PlayerInventory player;

    public Sprite icon;

    [Header("Stats")]
    [HideInInspector]
    public int defence;
     public int damage;
    [HideInInspector] public int critChance;
    [HideInInspector] public int critAmount;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        player = FindObjectOfType<PlayerInventory>();

        if (type == GearType.Armour)
        {
            defence = Random.Range(teir, teir + 2);
        }

        if(type == GearType.Weapon)
        {
            damage = Random.Range(teir, teir + 4);
            critChance = Random.Range(teir, teir*20);
            critAmount = damage * teir;
        }
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
