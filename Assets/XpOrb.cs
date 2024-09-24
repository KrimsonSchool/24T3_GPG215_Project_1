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

        if (timer >= 0.2)
        {
            Destroy(GetComponent<Rigidbody>());
            transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, Time.deltaTime * 100);

            if (Vector3.Distance(transform.position, Camera.main.transform.position) < 1)
            {
                FindObjectOfType<Inventory>().xp += 1;
                Destroy(gameObject);
            }
        }
    }
}
