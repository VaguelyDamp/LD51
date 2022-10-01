using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public KeyCode[] keys;
    public bool isOrdered = true;

    public float minInterval = 10;
    public float maxInterval = 10;
    public float failTime = 2;

    private float timeTillStart = 0;
    private float timeTillFail;
    private bool taskUp = false;
    private int keyIndex = 0;

    
    private float interval() 
    {
        return Random.Range(minInterval, maxInterval);
    }

    private void resetTask()
    {
        timeTillStart = interval();
        taskUp = false;
    }

    private void failTask()
    {
        Debug.Log("Task Failed");
        resetTask();
        //Failure Stuff
    }

    private void startTask() 
    {
        Debug.Log("Task Started");
        //Display Task
        timeTillStart = interval();
        timeTillFail = failTime;
        taskUp = true;
        keyIndex = 0;
    }

    private void endTask()
    {
        Debug.Log("Task Completed!");
        resetTask();
    }

    private void keySuccess()
    {
        Debug.Log("Correct Key");
        //Do positive feedback here
        keyIndex++;
        if (keyIndex >= keys.Length) endTask();
    }

    private void keyFail()
    {
        Debug.Log("Incorrect Key");
        //Do negative feedback here
    }

    void Start()
    {
        resetTask();
        timeTillFail = failTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeTillStart -= Time.deltaTime;
        if (timeTillStart <= 0) startTask();

        if (taskUp)
        {
            timeTillFail -= Time.deltaTime;
            if (timeTillFail <= 0) failTask();

            if (Input.GetKeyDown(keys[keyIndex])) keySuccess();
            else if (Input.anyKeyDown) keyFail();
        }     
    }
}
