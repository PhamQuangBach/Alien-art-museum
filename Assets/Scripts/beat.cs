using UnityEngine;

public class beat : MonoBehaviour
{
    private float deltatime = 0f; // Time accumulator
    [SerializeField] float beatinterval = 0.5f; // Interval between beats in seconds

    public AudioClip beatSound;
    private AudioManager audioManager; 

    [SerializeField] int[] beatSequence;
    int beatCounter = 0;
    [SerializeField] public Human[] humans;

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
            deltatime = 0f;
            OnBeat();
        }
    }

    public void OnBeat()
    {
        Debug.Log("Beat!");

        if (beatSequence.Length <= beatCounter) 
        {
            beatCounter = 0;
        }

        if (beatSequence[beatCounter] == 1)
        {
            audioManager.PlaySound("Happy", 1, (beatCounter * 0.03f) + 1f);

            if (humans.Length > beatCounter)
            {
                humans[beatCounter].OnSpeak();
            }
        }

        beatCounter++;
    }

    public bool IsOnBeat()
    {
        // A hit is considered "on beat" if it occurs within 0.1 seconds of the beat
        return deltatime <= 0.1f || deltatime >= (beatinterval - 0.1f);
    }
}
