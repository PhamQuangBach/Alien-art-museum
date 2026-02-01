using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using static Human;

public class beat : MonoBehaviour
{
    private float deltatime = 0f; // Time accumulator
    [SerializeField] float beatinterval = 0.5f; // Interval between beats in seconds

    [SerializeField]
    float hitWindow = 0.3f;
    [SerializeField]
    float hitWindowOffset = 0.05f;

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
    public bool patternFailed = false;

    [SerializeField] private int score = 0;
    [SerializeField] private int succesfulHitsNeeded = 0;


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
        if (beat >= 4)
        {
            succesfulHitsNeeded += 1;
        }
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
                if (queue.Count > patternIndex && !patternFailed)
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
                StartCoroutine(TransitionAnimation(beatinterval, 40.96f));
            }
            else if (beatTransitionState == 3)
            {
                succesfulHitsNeeded = 0;
                FindFirstObjectByType<BeatQueuer>().QueueRandomPattern(); // also spawns new people
                StartCoroutine(PeopleMoveInAnimation(beatinterval * 0.6f, 20, GetComponent<QueueSpawner>().queue));
                patternIndex = 0;
            }
        }
    }

    public void Cheer()
    {
        Instantiate(tileableHallway, Camera.main.transform.position + new Vector3(40.96f, 0, 28), Quaternion.identity);

        if (succesfulHitsNeeded == 0)
        {
            score += 1;
            if (score == 5)
            {
                FindFirstObjectByType<BeatQueuer>().LoadPattern(1);
            }
            else if (score == 9)
            {
                FindFirstObjectByType<BeatQueuer>().LoadPattern(2);
            }
            else if (score == 12)
            {
                FindFirstObjectByType<BeatQueuer>().LoadPattern(3);
            }
            else if (score == 15)
            {
                FindFirstObjectByType<BeatQueuer>().LoadPattern(3);
            }
            else if (score == 18)
            {
                GameObject gameState = GameObject.FindGameObjectWithTag("GameController");
                gameState.GetComponent<OnlineMode>().loadMenu();
            }
        }
    }

    IEnumerator TransitionAnimation(float length, float dist)
    {
        float T = 0;
        float x = Camera.main.transform.position.x;
        while (T < length)
        {
            Camera.main.transform.position += new Vector3(dist * Time.deltaTime / length, 0, 0);
            T += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = new Vector3(x + dist, 0, -10);
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
        foreach (GameObject person in people)
        {
            person.GetComponent<Human>().StartLookAtAnimation();
        }
    }


    public bool HitBeat(int beatIndex)
    {
        if (lastBeat == 10 || (beatQueue.Count > 0 && beatQueue.Peek() == 10))
        {
            return true;
        }

        if (deltatime <= hitWindow / 2f + hitWindowOffset)
        {
            if (beatIndex == lastBeat - 4)
            {
                return true;
            }
        }
        else if (deltatime >= (beatinterval - hitWindow / 2f + hitWindowOffset))
        {
            if (beatQueue.Count > 0)
            {
                if (beatIndex == beatQueue.Peek() - 4)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void OnGoodHit(int input)
    {
        Debug.Log("Good Hit!");
        if (!nextAlien.IsDestroyed())
        {
            nextAlien.GetComponent<Human>().OnSpeak(input);
        }

        if (succesfulHitsNeeded != 0) succesfulHitsNeeded -= 1;
    }

    public void OnBadHit(int input)
    {
        Debug.Log("Bad Hit!");
        if (!nextAlien.IsDestroyed())
        {
            nextAlien.GetComponent<Human>().OnSpeak(input);
        }

        succesfulHitsNeeded = -1;

        foreach (GameObject character in GetComponent<QueueSpawner>().queue)
        {
            character.GetComponent<Human>().OnSpeak((int)AlienAnimController.alienAnimations.Fail);
        }
        if (!nextAlien.IsDestroyed())
        {
            nextAlien.GetComponent<Human>().OnSpeak((int)AlienAnimController.alienAnimations.Fail);
        }
    }
}
