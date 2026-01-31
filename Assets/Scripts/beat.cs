using System.Collections.Generic;
using UnityEngine;

public class beat : MonoBehaviour
{
    [SerializeField]
    private float deltatime = 0f; // Time accumulator
    [SerializeField] float beatinterval = 0.5f; // Interval between beats in seconds

    public AudioClip beatSound;
    private AudioManager audioManager;

    [SerializeField] int[] beatSequence;

    private Queue<int> beatQueue = new Queue<int>();
    int beatCounter = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        deltatime += Time.deltaTime;
        if (deltatime >= beatinterval)
        {
            deltatime -= beatinterval;
            OnBeat();
        }
    }

    public void QueueBeat(int beat)
    {
        beatQueue.Enqueue(beat);
    }

    public void OnBeat()
    {
        if (beatSequence.Length <= beatCounter)
        {
            beatCounter = 0;
        }

        if (beatSequence[beatCounter] == 1)
        {
            audioManager.PlaySound("Happy");
        }

        beatCounter++;
    }

    public bool IsOnBeat()
    {
        // A hit is considered "on beat" if it occurs within 0.1 seconds of the beat
        print(deltatime);
        return deltatime <= 0.1f || deltatime >= (beatinterval - 0.1f);
    }
}
