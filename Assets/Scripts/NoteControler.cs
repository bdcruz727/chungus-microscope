using UnityEditor;
using UnityEngine;

public class NoteControler : MonoBehaviour
{
    public int hitTime, startTime;
    GameObject despawnPoint, spawnPoint;
    public float t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        despawnPoint = GameObject.FindGameObjectWithTag("end");
        spawnPoint = GameObject.FindGameObjectWithTag("spawn");

        GameObject manager = GameObject.FindGameObjectWithTag("manager");
        hitTime = manager.GetComponent<NoteManager>().currHit;
        startTime = manager.GetComponent<NoteManager>().currStart;

    }

    // Update is called once per frame
    void Update()
    {
        t = (((Time.time * 1000f) - (float)startTime) - ((float)hitTime - 200f))/(float)hitTime;
        gameObject.transform.position = Vector3.Lerp(spawnPoint.transform.position, despawnPoint.transform.position, t);
    }
}
