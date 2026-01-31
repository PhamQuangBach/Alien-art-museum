using UnityEngine;

public class BeatHitter : MonoBehaviour
{
    private beat beatScript;
    private AudioManager audioManager; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       beatScript = GameObject.Find("beat").GetComponent<beat>(); 
       audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputPressed()
    {
        Debug.Log("input pressed!");

        //if hit on beat
        MakeSound();
    }

    private void MakeSound()
    {
        audioManager.PlaySound("Happy",1,1.2f);
    }
}
