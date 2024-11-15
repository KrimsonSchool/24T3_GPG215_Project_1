using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatOffset : MonoBehaviour
{
    public Material mat;
    public float speed;
    Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offset.x += -speed*Time.deltaTime;
        mat.SetTextureOffset("_MainTex", offset);
    }
}
