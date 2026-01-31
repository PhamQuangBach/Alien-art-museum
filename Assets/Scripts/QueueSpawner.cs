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
        pattern = beatQueuer.pattern;
        //ParsePattern();
        SpawnQueue(CalculatePositions());
    }

    //parse pattern to a list of gameobjects
    private GameObject GetGoFromPattern(int index = 0)
    {
        char c = pattern[index];
        if (char.IsLower(c))
        {   
            return characters[0];
        }
        else if (char.IsUpper(c))
        {
            return characters[1];
        }
        return null;
    }

    //calculate positions
    private float[] CalculatePositions()
    {
        int amountToSpawn = pattern.Length;
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
        for (int i = 0; i < pattern.Length; i++)
        {
            Vector3 spawnPosition = new Vector3(positions[i], transform.position.y, 0);
            queue.Add(Instantiate(GetGoFromPattern(i), spawnPosition, Quaternion.identity));
            //character.GetComponent<Human>().emotion = (Human.emotionMap[char.ToLower(pattern[i])]);
        }
    }

}
