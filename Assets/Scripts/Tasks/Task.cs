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

    private PromptSpot actualSpot;
    private GameObject promptPrefab;
    private GameObject prompt;

    public Color successColor = Color.green;
    public Color failColor = Color.red;

    
    private float interval() 
    {
        return Random.Range(minInterval, maxInterval);
    }

    private void ResetTask()
    {
        timeTillStart = interval();
        taskUp = false;
        Destroy(prompt);
    }

    private void FailTask()
    {
        Debug.Log("Task Failed");
        taskUp = false;
        StartCoroutine(FailTaskAnim());
        //Failure Stuff
    }
    private IEnumerator FailTaskAnim()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            SpriteRenderer keySR = prompt.transform.Find((i+1).ToString()).gameObject.GetComponent<SpriteRenderer>();
            keySR.color = failColor;
        }
        yield return new WaitForSeconds(0.6f);
        EndTask();
    }

    private void SucceedTask()
    {
        Debug.Log("Task Succeeded");
        taskUp = false;
        StartCoroutine(SucceedTaskAnim());
    }
    private IEnumerator SucceedTaskAnim()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            SpriteRenderer keySR = prompt.transform.Find((i+1).ToString()).gameObject.GetComponent<SpriteRenderer>();
            keySR.color = successColor;
        }
        yield return new WaitForSeconds(0.6f);
        EndTask();
    }

    private void StartTask() 
    {
        Debug.Log("Task Started");
        //Display Task
        
        PromptSpot[] spots = transform.Find("TrainPlane").gameObject.GetComponentsInChildren<PromptSpot>();
        actualSpot = null;
        foreach (PromptSpot spot in spots)
        {
            if (!spot.taken)
            {
                actualSpot = spot;
                spot.taken = true;
                break;
            }
        }
        if (actualSpot is null)
        {
            //Couldn't find a spot logic goes here
            Debug.Log("Can't find a spot");
            return;
        }

        prompt = Instantiate(promptPrefab, actualSpot.transform);
        for (int i = 0; i < keys.Length; i++)
        {
            //Assigne the correct sprites to the prefab
            string path = "Sprites/Keyboard_White_"+keys[i].ToString();
            Sprite sprite = Resources.Load<Sprite>(path);
            Transform t = prompt.transform.Find((i+1).ToString());
            t.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

        }

        timeTillStart = interval();
        timeTillFail = failTime;
        taskUp = true;
        keyIndex = 0;
    }

    private void EndTask()
    {
        Debug.Log("Task Completed!");
        taskUp = false;
        actualSpot.taken = false;
        actualSpot = null;
        ResetTask();
    }

    private void KeySuccess()
    {
        Debug.Log("Correct Key");
        //Do positive feedback here
        prompt.transform.Find((keyIndex+1).ToString()).gameObject.GetComponent<SpriteRenderer>().color = successColor;
        keyIndex++;
        if (keyIndex >= keys.Length) SucceedTask();
    }

    private void KeyFail()
    {
        Debug.Log("Incorrect Key");
        //Do negative feedback here
        StartCoroutine(KeyFailAnim(keyIndex));

    }
    private IEnumerator KeyFailAnim(int k)
    {
        GameObject key = prompt.transform.Find((k+1).ToString()).gameObject;
        SpriteRenderer keySR = key.GetComponent<SpriteRenderer>();
        keySR.color = failColor;
        Vector3 origPos = key.transform.position;
        float amp = 0.02f;
        float speed = 75;
        for (float eTime = 0; eTime < .3f; eTime += Time.deltaTime)
        {
            Vector3 kPos = key.transform.position;
            kPos.z = origPos.z + amp*Mathf.Sin(eTime*speed);
            kPos.y = origPos.y + amp*Mathf.Cos(eTime*speed);
            key.transform.position = kPos;
            yield return null;
        }
        key.transform.position = origPos;
        keySR.color = Color.white;
    }

    void Start()
    {
        ResetTask();
        timeTillFail = failTime;

        if (keys.Length > 5) Debug.LogError("Too many keys!");
        promptPrefab = Resources.Load<GameObject>("Prefabs/TaskPrompts/"+keys.Length.ToString()+"Key"); //Find correct size prefab
    }

    // Update is called once per frame
    void Update()
    {
        timeTillStart -= Time.deltaTime;
        if (timeTillStart <= 0) StartTask();

        if (taskUp)
        {
            timeTillFail -= Time.deltaTime;
            if (timeTillFail <= 0) FailTask();

            if (Input.GetKeyDown(keys[keyIndex])) KeySuccess();
            else if (Input.anyKeyDown) KeyFail();
        }     
    }
}
