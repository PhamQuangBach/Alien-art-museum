using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;
using System;


public class PaintingManager : MonoBehaviour
{
    public GameObject frame;
    Vector2 frameSize;
    public bool offlineMode = true;
    SpriteRenderer frameSprite;
    [SerializeField] public TextAsset offlinePaintings;
    [SerializeField] public TextAsset onlinePaintings;

    public GameObject gameState;

    List<Artwork> offlineCollection;
    List<Artwork> onlineCollection;
    void Start()
    {
        gameState = GameObject.FindGameObjectWithTag("GameController");
        offlineMode = gameState.GetComponent<OnlineMode>().globalOfflineMode;
        Debug.Log("Painting init");
        offlineCollection = JsonConvert.DeserializeObject<List<Artwork>>(offlinePaintings.text);
        onlineCollection = JsonConvert.DeserializeObject<List<Artwork>>(onlinePaintings.text);
        frameSprite = frame.GetComponent<SpriteRenderer>();
        frameSize = frameSprite.bounds.size;
        setFrame();
        randomizePainting();
        Debug.Log("Painting init done");
        
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
        Debug.Log("Painting randomized");
        
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
        string URL;
        if (offlineMode)
        {
            URL = painting.multimedia[0].jpg[1000];
            getPaintingTextureOffline(URL);
        }
        else
        {
            URL = painting.multimedia[0].jpg[1000];
            StartCoroutine(GetPaintingTexture(URL));
        }

        
        
    }

    void setFrame()
    {
        Vector2 paintingSize = GetComponent<SpriteRenderer>().bounds.size;
        float heightRatio = paintingSize.y / frameSize.y;
        float widthRatio = paintingSize.x/ frameSize.x ;
        frameSprite.size = new Vector2(widthRatio*1.02f,heightRatio*1.02f);
    }

    void getPaintingTextureOffline(string URL)
    {
        int resolution = 200;
        string path = URL.Replace("/","-").TrimStart(Convert.ToChar("-"));
        string resourcePath = "paintings/" + Path.GetFileNameWithoutExtension(path);
        Debug.Log(resourcePath);
        Texture2D paintingTexture = Resources.Load<Texture2D>(resourcePath);
        Rect paintingRect = new Rect(0,0,paintingTexture.width,paintingTexture.height);
        Vector2 paintingPivot = new Vector2(0.5f,0.5f);
        resolution =paintingTexture.height/5;
        Sprite paintingSprite = Sprite.Create(paintingTexture,paintingRect,paintingPivot, resolution);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = paintingSprite;
        setFrame();
    }

    IEnumerator GetPaintingTexture(string URL) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://www.kansallisgalleria.fi"+URL);
        yield return www.SendWebRequest();
        Debug.Log("Fetching painting");
        int resolution = 200;
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
            offlineMode = true;
            randomizePainting();
            gameState.GetComponent<OnlineMode>().globalOfflineMode = true;
        }
        else {
            Texture2D paintingTexture = DownloadHandlerTexture.GetContent(www);
            Rect paintingRect = new Rect(0,0,paintingTexture.width,paintingTexture.height);
            Vector2 paintingPivot = new Vector2(0.5f,0.5f);
            resolution =paintingTexture.height/5;
            Sprite paintingSprite = Sprite.Create(paintingTexture,paintingRect,paintingPivot, resolution);
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = paintingSprite;
            setFrame();
        }
    }
    
}
