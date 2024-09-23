using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGPos : MonoBehaviour
{
    public float x;
    public float y;
    public float minPos;
    public float maxPos;
    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0, 100) < 20)
        {
            Destroy(gameObject);
        }

        transform.position = new Vector3(x, y, Random.Range(minPos, maxPos));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
