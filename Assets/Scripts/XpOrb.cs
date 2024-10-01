using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpOrb : MonoBehaviour
{
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //after 0.2 seconds
        if (timer >= 0.2)
        {
            //destroy own rigidbody
            Destroy(GetComponent<Rigidbody>());
            //move toward player camera
            transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, Time.deltaTime * 100);

            //if within 1 unit of player camera then
            if (Vector3.Distance(transform.position, Camera.main.transform.position) < 1)
            {
                //add 1 xp to inventory
                FindObjectOfType<Inventory>().xp += 1;
                //destroy self
                Destroy(gameObject);
            }
        }
    }
}
