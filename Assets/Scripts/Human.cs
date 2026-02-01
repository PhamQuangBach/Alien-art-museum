using UnityEngine;
using System.Collections.Generic;

public class Human : MonoBehaviour
{

    float duration = 0.3f;
    float elapsedTime = 0f;
    [SerializeField] public bool isUndercoverAlien = false;
    [SerializeField] public Emotion emotion = 0;

    private float animSize = 1f;
    

    private AudioManager audioManager;
    private AlienAnimController anim;

    public enum Emotion
    {
        Happy,
        Sad,
        Surprised,
        Angry
    }

    public static Dictionary<char, Emotion> charEmotionMap = new Dictionary<char, Emotion>
    {
        {'w', Emotion.Happy},
        {'d', Emotion.Sad},
        {'s', Emotion.Surprised},
        {'a', Emotion.Angry},
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        anim = gameObject.GetComponent<AlienAnimController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        AnimationUpdate();
    }

    public void OnSpeak(int emotionOverride = -1)
    {
        if (emotionOverride != -1) emotion = (Emotion)emotionOverride;

        initSpeakAnimation();
        audioManager.PlaySound(emotion.ToString());
    }

    public void OnBeat()
    {
        initBeatAnimation();
    }

    void initSpeakAnimation()
    {
        elapsedTime = 0;
        transform.localScale = Vector3.one * 1.5f;
        animSize = 1.5f;
        duration = 0.3f;
        anim.PlayAnimation((int) AlienAnimController.alienAnimations.Turn);//turning animation
    }
    void initBeatAnimation()
    {
        elapsedTime = 0;
        transform.localScale = Vector3.one * 1.05f;
        animSize = 1.05f;
        duration = 0.15f;
        anim.PlayAnimation((int) AlienAnimController.alienAnimations.LookAt);//look at painting animation
    }

    void AnimationUpdate()
    {
        if (transform.localScale == Vector3.one)
        {
            anim.PlayAnimation((int) AlienAnimController.alienAnimations.Idle);//idle animation
            return;
        }
        else if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(Vector3.one * animSize, Vector3.one, t);
        }
        else
        {
            anim.PlayAnimation((int) AlienAnimController.alienAnimations.Idle);//idle animation
        }
    }
    
}
