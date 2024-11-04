using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteStreamer : MonoBehaviour
{
    public string spriteName;
    string filePath;
    // Start is called before the first frame update
    void Start()
    {
        //check if file exists in StreamingAssets
        //If it does --> Use as sprite
        //Else --> use fallback sprite

        if (spriteName == "")
        {
            spriteName = name;
        }
        filePath = Path.Combine(Application.streamingAssetsPath, spriteName + ".png");

        ReadFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadFile()
    {
        if (File.Exists(filePath))
        {
            byte[] imageData = File.ReadAllBytes(filePath);

            Texture2D newTexture = new Texture2D(2, 2);
            newTexture.LoadImage(imageData);
            Sprite sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.one / 2, 256);

            GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Could not find streamed asset, loading backup...");
            filePath =  Path.Combine("Sprites", spriteName); 
            if (File.Exists(filePath))
            {
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(filePath);
            }
            else
            {
                Debug.LogError("Backup sprite not found!");
            }
        }
    }
}
