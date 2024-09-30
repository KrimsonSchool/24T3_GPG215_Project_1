using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    //dropdown - GearType(helmet)
    //Dropdown - Teir(1)

    public GearType type = new GearType();
    public GearTeir teir = new GearTeir();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum GearType
    {
        Helmet,
        Chestplate,
        Leggings,
        Gauntlet,
        Boots,
        Weapon
    };

    public enum GearTeir
    {
        One,
        Two,
        Three,
        Four,
        Five
    };
}
