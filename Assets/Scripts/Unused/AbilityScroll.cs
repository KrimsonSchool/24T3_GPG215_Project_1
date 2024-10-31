using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityScroll : MonoBehaviour
{
    float timer;
    GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.2)
        {
            //destroy own rigidbody
            Destroy(GetComponent<Rigidbody>());
            //destroy own collider
            Destroy(GetComponent<Collider>());
            //move toward cam camera
            transform.position = Vector3.MoveTowards(transform.position, cam.transform.position, Time.deltaTime * 100);

            //if within 1 unit of cam camera then
            if (Vector3.Distance(transform.position, Camera.main.transform.position) < 1)
            {
                //add to inv, show on cam
            }
        }
    }
}
