using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class PaintingManager : MonoBehaviour
{
    
    [SerializeField] public TextAsset jsonFile;
    List<Artwork> collection;
    void Start()
    {
        collection = JsonConvert.DeserializeObject<List<Artwork>>(jsonFile.text);
        
        randomizePainting(collection);
        
    }
    Artwork getRandomPainting(List<Artwork> collection)
    {
        int choice = Random.Range(0,collection.Count);
        Artwork randomPainting = collection[choice];
        string URL = randomPainting.multimedia[0].jpg[4000];
        return randomPainting;
    }
    void randomizePainting(List<Artwork> collection)
    {
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
        }
    }
}
