using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    bool cansave = true;
    PlayerStats ps;
    RoomLevelManager rlm;
    PlayerInventory inv;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<PlayerStats>();
        inv = GetComponent<PlayerInventory>();
        rlm = FindObjectOfType<RoomLevelManager>();

        if (PlayerPrefs.GetInt("Health") > 0)
        {
            Load();
        }

        RoomLevelManager.RoomLevelChanging += Save;
    }

    private void OnLevelWasLoaded()
    {
        cansave = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Save()
    {
        if (cansave)
        {
            print("Saving cs: " + cansave);

            PlayerPrefs.SetInt("Health", ps.CurrentHealth);
            PlayerPrefs.SetInt("AttackDamage", ps.AttackDamage);
            PlayerPrefs.SetInt("Level", rlm.RoomLevel);

            if (inv.weapon != null)
            {
                PlayerPrefs.SetInt("HasWeapon", 1);
            }
            if (inv.armour != null)
            {
                PlayerPrefs.SetInt("HasArmour", 1);
            }
        }
        cansave = false;
    }

    public void Load()
    {
        ps.CurrentHealth = PlayerPrefs.GetInt("Health");
        ps.AttackDamage = PlayerPrefs.GetInt("AttackDamage");
        rlm.RoomLevel = PlayerPrefs.GetInt("Level");

        if (PlayerPrefs.GetInt("HasWeapon") == 1)
        {
            //spawn weapon
            //give saved stats
            //equip
        }
        if (PlayerPrefs.GetInt("HasArmour") == 1)
        {
            //spawn armour
            //give saved stats
            //equip
        }
    }
}
