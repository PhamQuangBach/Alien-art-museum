using UnityEngine;

public class Emotion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Color emotionColor;
    public void activateEmotion()
    {
        gameObject.GetComponent<SpriteRenderer>().color = emotionColor;

    }

    // Update is called once per frame
}
