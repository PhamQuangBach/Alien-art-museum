using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeatQueuer : MonoBehaviour
{
    [SerializeField]public string pattern = "wwwWdddDsssSaaaA";

    private Dictionary<char, int> mapping = new Dictionary<char, int>
    {
        {'w', 0},
        {'d', 1},
        {'s', 2},
        {'a', 3},
        {'W', 4},
        {'D', 5},
        {'S', 6},
        {'A', 7},
    };

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
            int p = -1;
            if (mapping.ContainsKey(pattern[i]))
            {
                p = mapping[pattern[i]];
            }
            beatScript.QueueBeat(p);
        }
    }
}
