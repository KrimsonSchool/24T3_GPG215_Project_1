using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;


public class TextureLoader : MonoBehaviour
{
    [SerializeField]
    private string textureName;
    private string filePath;
    // Start is called before the first frame update
    
    void Start()
    {
        if(textureName == "")
        {
            textureName = name;
        }
        filePath = Path.Combine(Application.streamingAssetsPath, textureName + ".png");


        if (File.Exists(filePath))
        {
            byte[] imageData = File.ReadAllBytes(filePath);

            Texture2D newTexture = new Texture2D(2,2);
            newTexture.LoadImage(imageData);
            Sprite sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.one/2, 256);

            GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            Debug.LogError("Error, File [" + filePath +"] not found, please check file name/files existance");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
