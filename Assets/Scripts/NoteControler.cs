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
        t = hitTime / (Time.time - startTime);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(spawnPoint.transform.position, despawnPoint.transform.position, t);
    }
}
