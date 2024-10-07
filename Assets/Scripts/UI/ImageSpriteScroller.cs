using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSpriteScroller : MonoBehaviour
{
    private Image image;
    [SerializeField] private Vector2 scrollSpeed;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.material = new Material(image.material); // DO NOT FORGET THIS OR YOU'LL CHANGE THE DEFAULT SPRITE MATERIAL!!!
    }

    private void Update()
    {
        image.material.mainTextureOffset -= scrollSpeed * Time.unscaledDeltaTime;
    }
}
