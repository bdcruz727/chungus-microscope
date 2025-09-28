using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject endPoint;
    public GameObject note1Prefab;
    public GameObject note2Prefab;
    private List<int[]> hitObjects = new List<int[]>();
    public int currentTimeMs;
    public int songTimeMs;
    public int songStart;
    int spawnLead = 200, despawnLag = 200;
    bool songPlaying = false;

    public int currHit, currStart;

    private int spawnIdx = 0;     // next note to spawn
    private int despawnIdx = 0;   // next note to despawn
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTimeMs = Mathf.RoundToInt(Time.time * 1000f);

        spawnPoint = GameObject.FindGameObjectWithTag("spawn");
        endPoint = GameObject.FindGameObjectWithTag("end");

        loadSong("AngelWithAShotGun");
    }

    void loadSong(string songName)
    {
        LoadHitObjects(songName);
        startSong();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeMs = Mathf.RoundToInt(Time.time * 1000f);
        songTimeMs = currentTimeMs - songStart;

        if (!songPlaying)
        {
            return;
        }
        while (spawnIdx < hitObjects.Count && hitObjects[spawnIdx][1] <= songTimeMs + spawnLead)
        {
            var (type, timeMs) = (hitObjects[spawnIdx][0], hitObjects[spawnIdx][1]);

            // pick prefab by type (you decide mapping)
            spawnNote(type, timeMs);
            spawnIdx++;
        }
    }

    void spawnNote(int type, int time)
    {
        GameObject currNote;
        if (type == 1)
        {
            currNote = Instantiate(note1Prefab, spawnPoint.transform);
        }
        else
        {
            currNote = Instantiate(note2Prefab, spawnPoint.transform);
        }

        //Debug.Log(currNote);
        //currNote.GetComponent<NoteControler>().startTime = songStart;
        //Debug.Log(currNote.GetComponent<NoteControler>().startTime);
        //currNote.GetComponent<NoteControler>().hitTime = time;
        currStart = songStart;
        Debug.Log(time);
        currHit = time;
    }

    void startSong()
    {
        songStart = Mathf.RoundToInt(Time.time * 1000f);
        Debug.Log("songStart: " + songStart);
        songPlaying = true;
    }

    
    private void LoadHitObjects(string fileName)
    {
        Debug.Log(fileName);
        TextAsset osuFile = Resources.Load<TextAsset>("Maps/" + fileName);
        if (osuFile == null)
        {
            Debug.LogError($"Could not load Resources/Maps/{fileName}.osu. " +
                        $"Ensure the file is at Assets/Resources/Maps and omit the extension in Resources.Load.");
            // Optional: list what's actually there
            foreach (var t in Resources.LoadAll<TextAsset>("Maps"))
                Debug.Log($"Found map: {t.name}");
            return;
        }
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
                    int type = int.Parse(parts[4]);
                    if (type == 1 || type == 3)
                    {
                        newNote[0] = 2;
                    }
                    else
                    {
                        newNote[0] = 1;
                    }
                    newNote[1] = time;
                }

                hitObjects.Add(newNote);
            }
        }
    }
}
