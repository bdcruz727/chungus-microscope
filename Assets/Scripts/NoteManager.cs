using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    GameObject spawnPoint;
    GameObject endPoint;
    GameObject note1;
    GameObject note2;
    private Queue<int[]> hitObjects = new Queue<int[]>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("spawn");
        endPoint = GameObject.FindGameObjectWithTag("end");

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnNote(int type, float time)
    {
        if (type == 1)
        {
            Instantiate(note1, spawnPoint.transform);
        }
        else
        {
            Instantiate(note2, spawnPoint.transform);
        }
    }

    
    private void LoadHitObjects(string fileName)
    {
        TextAsset osuFile = Resources.Load<TextAsset>("Maps/" + fileName);
        string fileText = osuFile.text;

        bool inHitObjects = false;
        foreach (var rawLine in fileText.Split('\n'))
        {
            string line = rawLine.Trim();

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                inHitObjects = line.Equals("[HitObjects]", System.StringComparison.OrdinalIgnoreCase);
                continue;
            }

            if (inHitObjects && line.Length > 0 && !line.StartsWith("//"))
            {
                string[] parts = line.Split(',');
                int[] newNote = { 0, 0 };
                if (parts.Length >= 4)
                {
                    int time = int.Parse(parts[2]);
                    int type = int.Parse(parts[3]);

                    newNote[0] = type;
                    newNote[1] = time;
                }
                
                hitObjects.Enqueue(newNote);
            }
        }
    }
}
