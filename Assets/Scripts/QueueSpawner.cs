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
        SpawnQueue(CalculatePositions());
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
    private float[] CalculatePositions()
    {
        int amountToSpawn = queue.Count;
        float totalWidth = 15;
        float spacing = totalWidth / (amountToSpawn + 1);

        float[] positions = new float[amountToSpawn];

        for (int i = 0; i < amountToSpawn; i++)
        {
            positions[i] = -spacing * (i + 1f) + (totalWidth / 2f);
        }
        return positions;
    }
    //spawn accordingly
    private void SpawnQueue(float[] positions)
    {
        for (int i = 0; i < queue.Count; i++)
        {
            Vector3 spawnPosition = new Vector3(positions[i], transform.position.y, 0);
            Instantiate(queue[i], spawnPosition, Quaternion.identity);
        }
    }
}
