using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitTrigger : MonoBehaviour
{
    public string soundName;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<FxPlayer>().PlaySound(soundName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
