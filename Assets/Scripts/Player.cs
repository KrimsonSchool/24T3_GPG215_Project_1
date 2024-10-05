using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    public int[] attackDamage;//damage for main attack, ability 1, ability 2, ability 3 etc
    int attack;
    Enemy enemy;
    public GameObject damageIndi;

    Global global;


    public GameObject weaponSlot;
    public GameObject armourSlot;

    int critChance;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = FindObjectOfType<Enemy>();
        global = FindObjectOfType<Global>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Inventory>().weapon!=null)
        {
            attackDamage[0] = 1 + FindObjectOfType<Inventory>().weapon.damage;
            critChance = FindObjectOfType<Inventory>().weapon.critChance;
        }

        if (FindObjectOfType<MenuManager>().openMenus == 0)
        {
            Time.timeScale = 1;
            if (Input.GetMouseButtonDown(0))
            {
                AttackMain();
                print("Click attack " + attackDamage[attack] + " DMG");
            }
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void AttackMain()
    {
        attack = 0;
        //would be dependent on crit chance
        if(Random.Range(0, 100) <= critChance)
        {
            Instantiate(global.critCircle, FindObjectOfType<Canvas>().transform);
        }
        AttackDone();
    }

    public void Ability1()
    {
        if (animator != null)
        {
            animator.SetBool("Attack", true);
        }
        attack = 1;
    }
    public void Ability2()
    {
        if (animator != null)
        {
            animator.SetBool("Attack", true);
        }
        attack = 2;
    }
    public void Ability3()
    {
        if (animator != null)
        {
            animator.SetBool("Attack", true);
        }
        attack = 3;
    }

    public void AttackDone()
    {
        if (animator != null)
        {
            animator.SetBool("Attack", false);
        }

        if (enemy != null)
        {
            enemy.health -= attackDamage[attack]; 
            Instantiate(damageIndi, enemy.transform.position, Quaternion.identity).GetComponent<TMPro.TextMeshPro>().text = "-" + attackDamage[attack]+"HP";
        }
    }
}
