using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroll : MonoBehaviour
{
    public RawImage image;
    public float speedX;
    public float speedY;

    float x;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x += speedX * Time.deltaTime;
        y += speedY * Time.deltaTime;
        image.uvRect = new Rect(new Vector2(x, y), new Vector2(image.uvRect.width, image.uvRect.height));
    }
}
