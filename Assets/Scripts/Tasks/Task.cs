using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Task : MonoBehaviour
{
    public string taskName = "Task Name";
    public StaffCard.StaffType staffType;
    public KeyCode[] keys;
    public bool isOrdered = true;

    public int associatedTrainCar = 0;

    public float minInterval = 10;
    public float maxInterval = 10;
    public float offset = 0;
    public float failTime = 4;

    public float otherSelectedTimeMult = 0.75f;

    private float timeTillStart = 0;
    public float timeTillFail;
    public bool taskUp = false;
    private int keyIndex = 0;

    private PromptSpot actualSpot;
    private GameObject promptPrefab;
    private GameObject prompt;
    private FillMeter timerMeter;
    private bool carDead = false;

    public bool selected = false;
    private bool otherSelected = false;

    public Color successColor = Color.green;
    public Color failColor = Color.red;

    private AudioSource audioS;
    public AudioClip taskPopUp;
    public AudioClip taskFailNoise;
    public AudioClip[] goodKeys;
    public AudioClip[] badKeys;
    public AudioClip taskSucceed;

    public bool taskEnabled = true;

    
    private float interval() 
    {
        return Random.Range(minInterval, maxInterval);
    }

    public void KillCar()
    {
        carDead = true;
        StopAllCoroutines();
        Destroy(prompt);
    }

    private void ResetTask()
    {
        timeTillStart = interval();
        taskUp = false;
        StopAllCoroutines();
        Destroy(prompt);
    }

    private void FailTask()
    {
        Debug.Log("Task Failed");
        audioS.PlayOneShot(taskFailNoise);
        taskUp = false;
        StartCoroutine(FailTaskAnim());
        //Failure Stuff
        gameObject.GetComponent<Car>().TaskFailed();
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
        audioS.PlayOneShot(taskSucceed);
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

    public void StartTask() 
    {
        //Debug.Log("Task Started");
        //Display Task

        if (prompt != null) 
        {
            Debug.LogWarning("Trying to create an already existing prompt");
            timeTillStart = 0.2f;
            return;
        }
        
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

        audioS.PlayOneShot(taskPopUp);

        prompt = Instantiate(promptPrefab, actualSpot.transform);
        Transform promptCanvas = prompt.transform.Find("Canvas");
        promptCanvas.Find("Name").GetComponent<TextMeshPro>().text = taskName;
        if(staffType != StaffCard.StaffType.None) promptCanvas.Find("StaffSprite").GetComponent<UnityEngine.UI.Image>().sprite = DeckManager.instance.GetSpriteForStaff(staffType, false);
        else promptCanvas.Find("StaffSprite").gameObject.SetActive(false);
        
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

        timerMeter = promptCanvas.Find("Timer").GetComponent<FillMeter>();
        timerMeter.maxVal = failTime;

        SetKeyPromptsHidden(!selected);
    }

    private void SetKeyPromptsHidden(bool hide){
        if(taskUp) {
            foreach(SpriteRenderer sr in prompt.GetComponentsInChildren<SpriteRenderer>()) {
                sr.enabled = !hide;
            }
        }
    }

    private void EndTask()
    {
        //Debug.Log("Task Completed!");
        taskUp = false;
        actualSpot.taken = false;
        actualSpot = null;
        ResetTask();

        //if(selected) FindObjectOfType<Train>().SelectCar(-1);
    }

    private void KeySuccess()
    {
        //Debug.Log("Correct Key");
        //Do positive feedback here
        audioS.pitch = Helpers.Remap(keyIndex, 0, keys.Length - 1, 0.2f, 1.8f);
        audioS.PlayOneShot(goodKeys[Random.Range(0, goodKeys.Length - 1)], 0.5f);
        prompt.transform.Find((keyIndex+1).ToString()).gameObject.GetComponent<SpriteRenderer>().color = successColor;
        keyIndex++;
        if (keyIndex >= keys.Length) SucceedTask();
    }
    private IEnumerator playKeySound()
    {
        audioS.pitch = Helpers.Remap(keyIndex, 0, keys.Length - 1, 0.8f, 1.2f);
        audioS.PlayOneShot(goodKeys[Random.Range(0, goodKeys.Length - 1)], 0.5f);
        while(audioS.isPlaying) yield return null;
        audioS.pitch = 1;
    }

    public void KeyFail()
    {
        //Debug.Log("Incorrect Key");
        //Do negative feedback here
        if (taskUp)
        {
            audioS.PlayOneShot(badKeys[Random.Range(0, badKeys.Length - 1)]);
            StartCoroutine(KeyFailAnim(keyIndex));
        }
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
        audioS = gameObject.GetComponent<AudioSource>();
        ResetTask();
        timeTillStart = Random.Range(minInterval * 0.5f, maxInterval);
        timeTillFail = failTime;

        if (keys.Length > 5) Debug.LogError("Too many keys!");
        promptPrefab = Resources.Load<GameObject>("Prefabs/TaskPrompts/"+keys.Length.ToString()+"Key"); //Find correct size prefab
    
        FindObjectOfType<Train>().CarSelectionChanged.AddListener(OnTrainSelectionChanged);
        
        if (gameObject.GetComponent<Car>().GetAssignedStaffTypes().Contains(staffType)) taskEnabled = false;
        else if (gameObject.GetComponent<Car>().GetAssignedStaffTypes().Contains(StaffCard.StaffType.DampBoi))
        {
            if (gameObject.GetComponent<Car>().card.transform.Find("StaffSpot:"+staffType).childCount > 0)
            {
                Debug.Log("Found Damp Boi for task "+staffType);
                taskEnabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeTillStart -= Time.deltaTime;
        if (timeTillStart <= 0 && !carDead && !taskUp && taskEnabled && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "trainplace") StartTask();

        if (taskUp)
        {
            float modTimeStep = Time.deltaTime * (otherSelected ? otherSelectedTimeMult : 1.0f);
            timeTillFail -= modTimeStep;
            timerMeter.Value = timeTillFail;
            if (timeTillFail <= 0) FailTask();

            if (selected) {
                //if (Input.GetKeyDown(keys[keyIndex])) KeySuccess();
                //else if (Input.anyKeyDown) KeyFail();
            }
        }     
    }

    public bool TryKey()
    {
        if (taskUp)
        {
            if (Input.GetKeyDown(keys[keyIndex]))
            {
                KeySuccess();
                return true;
            }
            else return false;
        }
        else return false;
    }

    private void OnTrainSelectionChanged(int newSelection) {
        selected = (newSelection == associatedTrainCar);
        otherSelected = !selected && (newSelection != -1);

        SetKeyPromptsHidden(!selected);
        if(selected) {
            Debug.LogFormat("Task {0} on car {1} is now active", taskName, associatedTrainCar);
        }
    }
}
