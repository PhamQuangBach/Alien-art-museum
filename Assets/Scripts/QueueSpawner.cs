using UnityEngine;
using System.Collections.Generic;
using System.Runtime;

public class QueueSpawner : MonoBehaviour
{
    private BeatQueuer beatQueuer;
    private string pattern;
    [SerializeField] public List<GameObject> queue;
    [SerializeField] private GameObject[] characters;

    private void Start() {
        beatQueuer = GetComponent<BeatQueuer>();
        FetchPattern();
        ParsePattern();
    }

    //fetch pattern 
    private string FetchPattern()
    {
        return pattern = beatQueuer.pattern;
    }

    //parse pattern
    private void ParsePattern()
    {
        for (int i = 0; i < pattern.Length; i++)
        {
            char c = pattern[i];
            if (char.IsLower(c))
            {   
                queue.Add(characters[0]);
            }
            else if (char.IsUpper(c))
            {
                queue.Add(characters[1]);
            }
        }
    }

    //calculate positions

    //spawn accordingly
}
