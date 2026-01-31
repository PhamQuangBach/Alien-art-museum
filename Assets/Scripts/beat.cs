using UnityEngine;

public class beat : MonoBehaviour
{
    private float deltatime = 0f; // Time accumulator
    [SerializeField] float beatinterval = 0.5f; // Interval between beats in seconds

    public AudioClip beatSound;
    private AudioManager audioManager; 

    [SerializeField] int[] beatSequence;
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
            audioManager.PlaySound("Happy");
        }

        beatCounter++;
    }

    
}
