using UnityEngine;

public class Human : MonoBehaviour
{

    float duration = 0.3f;
    float elapsedTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        AnimationUpdate();
    }

    public void OnSpeak()
    {
        elapsedTime = 0;
        //Scale to 1.5
        transform.localScale = Vector3.one * 1.5f;
    }

    void AnimationUpdate()
    {
        if (transform.localScale == Vector3.one)
        {
            return;
        }
        else if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            //Ease the scale back to 1 over 0.3 seconds
            transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one, t);
        }
    }
}
