using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
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

    bool paused = false;

    [SerializeField]
    GameObject tileableHallway;

    public AudioClip beatSound;
    private AudioManager audioManager;
    private Queue<int> beatQueue = new Queue<int>();
    public GameObject nextAlien;
    private int lastBeat = -1;

    private int beatTransitionState = 0;

    public int patternIndex;

    [SerializeField] private int score = 0;
    private bool failedPattern = false;


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
        if (!paused && beatQueue.Count > 0)
        {
            beatTransitionState = 0;
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
        else if (!paused)
        {
            beatTransitionState += 1;
            if (beatTransitionState == 1)
            {
                Cheer();
            }
            else if (beatTransitionState == 2)
            {
                StartCoroutine(TransitionAnimation(beatinterval, 40));
            }
            else if (beatTransitionState == 3)
            {
                FindFirstObjectByType<BeatQueuer>().QueueRandomPattern(); // also spawns new people
                StartCoroutine(PeopleMoveInAnimation(beatinterval, 20, GetComponent<QueueSpawner>().queue));
                patternIndex = 0;
                failedPattern = false;
            }
        }
    }

    public void Cheer()
    {
        Instantiate(tileableHallway, Camera.main.transform.position + new Vector3(40, 0, 28), Quaternion.identity);

        if (!failedPattern)
        {
            score += 1;
            if (score == 5)
            {
                FindFirstObjectByType<BeatQueuer>().LoadPattern(1);
            }
            else if (score == 10)
            {
                FindFirstObjectByType<BeatQueuer>().LoadPattern(2);
            }
            else if (score == 15)
            {
                FindFirstObjectByType<BeatQueuer>().LoadPattern(3);
            }
        }
    }

    IEnumerator TransitionAnimation(float length, float dist)
    {
        float T = 0;
        while (T < length)
        {
            Camera.main.transform.position += new Vector3(dist * Time.deltaTime / length, 0, 0);
            T += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator PeopleMoveInAnimation(float length, float dist, List<GameObject> people)
    {
        float T = 0;
        while (T < length)
        {
            foreach (GameObject person in people)
            {
                person.transform.position += new Vector3(dist * Time.deltaTime / length, 0, 0);
            }
            T += Time.deltaTime;
            yield return null;
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

        failedPattern = true;
    }
}
