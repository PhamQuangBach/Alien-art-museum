using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class PaintingManager : MonoBehaviour
{
    public GameObject frame;
    Vector2 frameSize;
    SpriteRenderer frameSprite;
    [SerializeField] public TextAsset jsonFile;
    List<Artwork> collection;
    void Start()
    {
        frameSprite = frame.GetComponent<SpriteRenderer>();
        frameSize = frameSprite.bounds.size;
        InvokeRepeating(nameof(randomizePainting),0,3);
        
    }



    Artwork getRandomPainting(List<Artwork> collection)
    {
        int choice = Random.Range(0,collection.Count);
        Artwork randomPainting = collection[choice];
        string URL = randomPainting.multimedia[0].jpg[4000];
        return randomPainting;
    }
    void randomizePainting()
    {
        collection = JsonConvert.DeserializeObject<List<Artwork>>(jsonFile.text);
        Artwork painting = getRandomPainting(collection);
        setPainting(painting);
    }
    void setPainting(Artwork painting)
    {
        Debug.Log(painting.title["en"]);
        Debug.Log("URL: " + painting.multimedia[0].jpg[4000]);
        string URL = painting.multimedia[0].jpg[4000];
        StartCoroutine(GetPaintingTexture(URL));
        
    }

    void setFrame()
    {
        Vector2 paintingSize = GetComponent<SpriteRenderer>().bounds.size;
        float heightRatio = paintingSize.y / frameSize.y;
        float widthRatio = paintingSize.x/ frameSize.x ;
        frameSprite.size = new Vector2(widthRatio*1.05f,heightRatio*1.05f);
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
            Sprite paintingSprite = Sprite.Create(paintingTexture,paintingRect,paintingPivot, 1000);
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = paintingSprite;
            setFrame();
        }
    }
    
}
