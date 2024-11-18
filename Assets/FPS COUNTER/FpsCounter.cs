using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    TMPro.TextMeshProUGUI fpsText;

    float timer;

    float frameS;
    float frameE;
    // Start is called before the first frame update
    void Start()
    {
        fpsText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.25f)
        {
            frameE = Time.frameCount;

            fpsText.text = "FPS: " + (frameE - frameS)*4;

            timer = 0;
            frameS = Time.frameCount;
        }
    }
}
