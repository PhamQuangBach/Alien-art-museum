using UnityEngine;
using System.Collections.Generic;
using System.Runtime;
using System.Linq;

public class QueueSpawner : MonoBehaviour
{
    //private BeatQueuer beatQueuer;
    //private string pattern;
    [SerializeField] public List<GameObject> queue;
    [SerializeField] private GameObject[] characters;

    private void Start()
    {
        //beatQueuer = GetComponent<BeatQueuer>();
        //pattern = beatQueuer.pattern;
        //ParsePattern();
        //SpawnQueue(CalculatePositions());
    }

    //parse pattern to a list of gameobjects
    private GameObject GetGoFromPattern(string pattern, int index = 0)
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
    private float[] CalculatePositions(int length, float offset)
    {
        int amountToSpawn = length;
        float totalWidth = 15;
        float spacing = totalWidth / (amountToSpawn + 1);

        float[] positions = new float[amountToSpawn];

        for (int i = 0; i < amountToSpawn; i++)
        {
            positions[i] = -spacing * (i + 1f) + (totalWidth / 2f) + offset;
        }
        return positions;
    }

    //spawn accordingly
    public void SpawnQueue(string pattern)
    {
        float[] positions = CalculatePositions(pattern.Length, Camera.main.transform.position.x - 20);
        for (int i = 0; i < pattern.Length; i++)
        {
            Vector3 spawnPosition = new Vector3(positions[i], transform.position.y, 0);
            queue.Add(Instantiate(GetGoFromPattern(pattern, i), spawnPosition, Quaternion.identity));

            //set emotion for the spawned character
            Human human = queue[queue.Count - 1].GetComponent<Human>();
            human.emotion = Human.charEmotionMap[char.ToLower(pattern[i])];
            if (char.IsUpper(pattern[i]))
            {
                human.isUndercoverAlien = true;
            }
        }
    }

    public void DespawnQueue()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            Destroy(queue[i]);
        }
        queue.Clear();
    }
}
