using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    int value;
    float timer;
    MeshRenderer mr;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();

        value = 1;
        if(Random.Range(0, 100) < 25)
        {
            if(Random.Range(0,100) < 25 )
            {
                value = 30;
                mr.material = FindObjectOfType<Global>().coinMats[2];
            }
            else
            {
                value = 10;
                mr.material = FindObjectOfType<Global>().coinMats[1];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            Destroy(GetComponent<Rigidbody>());
            transform.position = Vector3.MoveTowards(transform.position, Camera.main.transform.position, Time.deltaTime*100);

            if(Vector3.Distance(transform.position, Camera.main.transform.position) < 1)
            {
                FindObjectOfType<Inventory>().coins += value;
                Destroy(gameObject);
            }
        }
    }
}
