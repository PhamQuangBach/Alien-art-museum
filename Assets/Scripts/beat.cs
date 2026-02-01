using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Human;

public class beat : MonoBehaviour
{
    private float deltatime = 0f; // Time accumulator
    [SerializeField] float beatinterval = 0.5f; // Interval between beats in seconds

    [SerializeField]
    float hitWindow = 0.2f;

    [SerializeField]
    float mainMusicDelay = 0;

    public AudioClip beatSound;
    private AudioManager audioManager;
    private Queue<int> beatQueue = new Queue<int>();
    public GameObject nextAlien;
    private int lastBeat = -1;
    //public string[] beatSounds = new string[] { "Happy", "Sad", "Surprised", "Angry" };

    public int patternIndex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        patternIndex = 0;
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


        if (mainMusicDelay >= 0 && mainMusicDelay - Time.deltaTime < 0)
        {
            audioManager.StartMainMusic();
        }
        mainMusicDelay -= Time.deltaTime;
    }

    public void QueueBeat(int beat)
    {
        beatQueue.Enqueue(beat);
    }

    public void OnBeat()
    {
        if (beatQueue.Count > 0)
        {
            foreach (GameObject character in GetComponent<QueueSpawner>().queue)
            {
                character.GetComponent<Human>().OnBeat();
            }

            List<GameObject> queue = GetComponent<QueueSpawner>().queue;
            int beatIndex = beatQueue.Dequeue();
            if (beatIndex < 4 && beatIndex >= 0)
            {
                
                //audioManager.PlaySound(beatSounds[beatIndex], 1, (beatIndex * 0.03f) + 1f);

                if (queue.Count > patternIndex)
                {
                    queue[patternIndex].GetComponent<Human>().OnSpeak();
                }
            }
            
            //fetch next alien (player controlled character)
            if (queue.Count > patternIndex)
            {
                nextAlien = queue[patternIndex];
                for (int i = 0; i < queue.Count - patternIndex + 1; i++)
                {
                    int index = i + patternIndex;
                    if (index >= queue.Count) break;
                    if (queue[index].GetComponent<Human>().isUndercoverAlien)
                    {
                        nextAlien = queue[index];
                        break;
                    }
                }
            }

            lastBeat = beatIndex;
            patternIndex++;
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

    public void OnGoodHit(int input)
    {
        Debug.Log("Good Hit!");
        nextAlien.GetComponent<Human>().OnSpeak(input);
    }

    public void OnBadHit(int input)
    {
        Debug.Log("Bad Hit!");
        nextAlien.GetComponent<Human>().OnSpeak(input);
    }
}
