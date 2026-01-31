using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.InputSystem;
using UnityEditor;
using System.IO;
using System;
using Mono.Cecil;

public class PaintingManager : MonoBehaviour
{
    public GameObject frame;
    Vector2 frameSize;
    public bool offlineMode = true;
    SpriteRenderer frameSprite;
    [SerializeField] public TextAsset offlinePaintings;
    [SerializeField] public TextAsset onlinePaintings;



    List<Artwork> offlineCollection;
    List<Artwork> onlineCollection;
    void Start()
    {
        
        offlineCollection = JsonConvert.DeserializeObject<List<Artwork>>(offlinePaintings.text);
        onlineCollection = JsonConvert.DeserializeObject<List<Artwork>>(onlinePaintings.text);
        frameSprite = frame.GetComponent<SpriteRenderer>();
        frameSize = frameSprite.bounds.size;
        
        setFrame();
        randomizePainting();
        
    }



    Artwork getRandomPainting(List<Artwork> collection)
    {
        int choice = UnityEngine.Random.Range(0,collection.Count);
        Artwork randomPainting = collection[choice];
        return randomPainting;
    }
    void randomizePainting()
    {
        Artwork painting;

        
        if (offlineMode)
        {
            painting = getRandomPainting(offlineCollection);
        }
        else
        {
            painting = getRandomPainting(onlineCollection);
        }
        

        setPaintingTexture(painting);
    }
    void setPaintingTexture(Artwork painting)
    {
        Debug.Log(painting.title["en"]);
        Debug.Log("URL: " + painting.multimedia[0].jpg[4000]);
        string URL = painting.multimedia[0].jpg[4000];
        if (offlineMode)
        {
            getPaintingTextureOffline(URL);
        }
        else
        {
            StartCoroutine(GetPaintingTexture(URL));
        }

        
        
    }

    void setFrame()
    {
        Vector2 paintingSize = GetComponent<SpriteRenderer>().bounds.size;
        float heightRatio = paintingSize.y / frameSize.y;
        float widthRatio = paintingSize.x/ frameSize.x ;
        frameSprite.size = new Vector2(widthRatio*1.05f,heightRatio*1.05f);
    }

    void getPaintingTextureOffline(string URL)
    {
        string path = URL.Replace("/","-").TrimStart(Convert.ToChar("-"));
        string resourcePath = "paintings/" + Path.GetFileNameWithoutExtension(path);
        Debug.Log(resourcePath);
        Texture2D paintingTexture = Resources.Load<Texture2D>(resourcePath);
        Rect paintingRect = new Rect(0,0,paintingTexture.width,paintingTexture.height);
        Vector2 paintingPivot = new Vector2(0.5f,0.5f);
        Sprite paintingSprite = Sprite.Create(paintingTexture,paintingRect,paintingPivot, 512);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = paintingSprite;
        setFrame();
    }

    IEnumerator GetPaintingTexture(string URL) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.kansallisgalleria.fi"+URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            Texture2D paintingTexture = DownloadHandlerTexture.GetContent(www);
            Rect paintingRect = new Rect(0,0,paintingTexture.width,paintingTexture.height);
            Vector2 paintingPivot = new Vector2(0.5f,0.5f);
            Sprite paintingSprite = Sprite.Create(paintingTexture,paintingRect,paintingPivot, 512);
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = paintingSprite;
            setFrame();
        }
    }
    
}
