using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public float speed;
    public TMPro.TextMeshPro tmp;
    float a;
    public float aSpeed;
    // Start is called before the first frame update
    void Start()
    {
        a = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * speed;
        
        a-=Time.deltaTime*aSpeed;
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, a);
    }
}
