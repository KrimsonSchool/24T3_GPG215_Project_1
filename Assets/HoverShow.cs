using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class HoverShow : MonoBehaviour
{
    public GameObject[] toShow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HideShow()
    {
        foreach (var item in toShow)
        {
            item.SetActive(!item.activeSelf);
        }
    }
}
