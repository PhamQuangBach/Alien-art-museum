using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class BeatQueuer : MonoBehaviour
{
    [SerializeField] public string pattern = "wwwWdddDsssSaaaA";

    private List<string> patterns;

    [SerializeField] private TextAsset[] patternFiles;

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
    private QueueSpawner characterSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        beatScript = GameObject.Find("beat").GetComponent<beat>();
        characterSpawner = GameObject.Find("beat").GetComponent<QueueSpawner>();
        LoadPattern(0);
        //QueuePattern(pattern.Trim());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QueueRandomPattern()
    {
        QueuePattern(patterns[UnityEngine.Random.Range(0, patterns.Count)].Trim());
    }

    public void LoadPattern(int difficulty)
    {
        int diff = Math.Min(difficulty, patternFiles.Length - 1);
        patterns = new List<string>(patternFiles[diff].text.Split("\n"));
        patterns = patterns.Where(x => x.Length > 3).ToList();
    }

    public void QueuePattern(string pattern)
    {
        characterSpawner.DespawnQueue();
        for (int i = 0; i < pattern.Length; i++)
        {
            int p = -1;
            if (mapping.ContainsKey(pattern[i]))
            {
                p = mapping[pattern[i]];
            }
            beatScript.QueueBeat(p);
        }
        characterSpawner.SpawnQueue(pattern);
        beatScript.patternFailed = false;
    }
}
