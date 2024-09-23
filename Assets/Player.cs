using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    public int[] attackDamage;//could be array -_(00)_-
    int attack;
    Enemy enemy;
    public GameObject damageIndi;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack1()
    {
        if (animator != null)
        {
            animator.SetBool("Attack", true);
        }
        attack = 0;
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
