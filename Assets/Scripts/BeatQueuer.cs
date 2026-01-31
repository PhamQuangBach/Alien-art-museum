using UnityEngine;

public class BeatQueuer : MonoBehaviour
{
    public string pattern = "1115222633370004";

    private beat beatScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beatScript = GameObject.Find("beat").GetComponent<beat>();
        QueuePattern(pattern);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QueuePattern(string pattern)
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            beatScript.QueueBeat((int)char.GetNumericValue(pattern[i]));
        }
    }
}
