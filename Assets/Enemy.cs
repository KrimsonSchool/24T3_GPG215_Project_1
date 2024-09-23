using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    float totalHealth;
    Animator animator;

    public GameObject healthBar;
    public TMPro.TextMeshPro healthText;

    public Vector3[] pos;
    public Vector3[] healthBarPos;
    public Texture2D[] texture;
    public int[] difficulty;
    public int[] amountCoins;

    public GameObject hBar;

    public int enemyId;

    public GameObject sudoSelf;

    public GameObject coinGroup;

    bool dead;
    // Start is called before the first frame update
    void Start()
    {
        //enemyId = Random.Range(0, pos.Length);
        enemyId = FindObjectOfType<RoomManager>().enemyToSpawn[FindObjectOfType<RoomManager>().room];

        animator = GetComponent<Animator>();
        health = Mathf.RoundToInt(difficulty[enemyId] +FindObjectOfType<RoomManager>().room * 0.25f);

        sudoSelf.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", texture[enemyId]);
        sudoSelf.transform.position = pos[enemyId];
        hBar.transform.localPosition = healthBarPos[enemyId];

        totalHealth=health;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.localScale = new Vector3(health/totalHealth,1,1);
        healthText.text = health + "/" + totalHealth + "HP";
        if (health <= 0 && animator != null && !dead)
        {
            dead = true;
            Instantiate(coinGroup, transform.position + transform.up, Quaternion.identity);
            animator.SetBool("Dead", true);
        }
    }

    public void SignalDead()
    {

        if (FindObjectOfType<RoomManager>() != null)
        {
            FindObjectOfType<RoomManager>().enemyDead = true;
        }
    }
}
