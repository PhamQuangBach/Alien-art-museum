using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class beat : MonoBehaviour
{
    [SerializeField]
    private float deltatime = 0f; // Time accumulator
    [SerializeField] float beatinterval = 0.5f; // Interval between beats in seconds

    [SerializeField]
    float hitWindow = 0.2f;

    public AudioClip beatSound;
    private AudioManager audioManager;
    private Queue<int> beatQueue = new Queue<int>();
    private int lastBeat = -1;
    int beatCounter = 0;
    [SerializeField] public Human[] humans;
    public string[] beatSounds = new string[] { "Happy", "Sad", "Surprised", "Angry" };


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
        if (beatQueue.Count > 0)
        {
            int beatIndex = beatQueue.Dequeue();
            if (beatIndex < 4 && beatIndex >= 0)
            {
                audioManager.PlaySound(beatSounds[beatIndex], 1, (beatCounter * 0.03f) + 1f);

                if (humans.Length > beatCounter)
                {
                    humans[beatCounter].OnSpeak();
                }
            }
            lastBeat = beatIndex;
        }
    }

    public bool HitBeat(int beatIndex)
    {
        if (deltatime <= hitWindow / 2f)
        {
            if (beatIndex == lastBeat - 4)
            {
                return true;
            }
        }
        else if (deltatime >= (beatinterval - hitWindow / 2f))
        {
            if (beatQueue.Count > 0 && beatIndex == beatQueue.Peek() - 4)
            {
                return true;
            }
        }
        return false;
    }
}
