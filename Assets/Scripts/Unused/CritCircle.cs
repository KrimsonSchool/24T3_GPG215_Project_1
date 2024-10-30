using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CritCircle : MonoBehaviour
{
    public Vector2 posMin;
    public Vector2 posMax;

    public float shrinkSpeed;
    float sx;
    float sy;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector2(Random.Range(posMin.x, posMax.x), Random.Range(posMin.y, posMax.y));

        sx = transform.localScale.x;
        sy = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        sx-=Time.deltaTime * shrinkSpeed;
        sy-=Time.deltaTime * shrinkSpeed;

        transform.localScale = new Vector2(sx, sy);

        if(sx< 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void Crit()
    {
        FindObjectOfType<PlayerOLD>().AttackDone();
        Destroy(gameObject);
    }
}
