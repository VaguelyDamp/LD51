using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public int stars = 1;
    public int hearts = 3;

    public bool carDead = false;


    [SerializeField]
    private AnimationCurve fallCurve;
    private float fallTime = 2.5f;

    private ChunkMeter heartMeter;
    private ChunkMeter starMeter;
    private CarWobble carWobble;
    private Transform car;

    private Task[] tasks;

    public GameObject fire;

    private bool shakinBakin = false;

    private bool selected = false;
    public int associatedTrainCar = 0;

    private static KeyCode[] nonKeys = new KeyCode[] {KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, 
    KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Space, KeyCode.Return, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.Mouse0};
    public CarCard card;

    public void TaskFailed()
    {
        hearts -= 1;
        Debug.Log("Car: "+name+" has "+hearts+" hearts");
        if (hearts == 1)FindObjectOfType<Train>().PlayAlertWhistle();
        heartMeter.Value = hearts;
        StartCoroutine(DamageShake());
        if (hearts <= 0) KillCar();
    }
    protected IEnumerator DamageShake()
    {
        //Turn car red
        //carWobble.iBeWobblin = false;
        shakinBakin = true;
        car = transform.Find("Car");
        Vector3 origPos = car.transform.position;
        float amp = 0.2f;
        float speed = 50;
        for (float eTime = 0; eTime < .3f; eTime += Time.deltaTime)
        {
            Vector3 cPos = car.transform.position;
            cPos.z = origPos.z + amp*Mathf.Sin(eTime*speed);
            //cPos.y = origPos.y + amp*Mathf.Cos(eTime*speed);
            car.transform.position = cPos;
            yield return null;
        }
        //carWobble.iBeWobblin = true;
        shakinBakin = false;
    }

    protected void KillCar()
    {
        stars = 0;
        hearts = 0;
        carDead = true;
        FindObjectOfType<Train>().KillSmallCar();
        foreach (Task task in gameObject.GetComponents<Task>()) task.KillCar();
        StartCoroutine(CarFall());

        DeckManager.instance.RemoveCardFromHand(card.gameObject);
    }
    protected IEnumerator CarFall()
    {
        while(shakinBakin) yield return null;
        Quaternion startPos = car.rotation;
        Quaternion endPos = startPos*Quaternion.AngleAxis(90, Vector3.forward);
        carWobble.iBeWobblin = false;
        float animTime = 0;
        while (animTime < fallTime)
        {
            animTime += Time.deltaTime;
            float percent = Mathf.Clamp01(animTime / fallTime);
            car.rotation = Quaternion.LerpUnclamped(startPos, endPos, fallCurve.Evaluate(percent));
            yield return null;
        }
        if (gameObject.tag == "Engine")
        {
            FindObjectOfType<Train>().GameOver();
        }
        //Do fire here
        GameObject iFire = Instantiate(fire, car.Find("FireSpawn"));
        iFire.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<CarWobble>().resetRot(endPos);
        gameObject.GetComponent<CarWobble>().iBeWobblin = true;
    }

    public List<StaffCard.StaffType> GetAssignedStaffTypes() {
        if(!card) return new List<StaffCard.StaffType>();
        return card.GetStaffTypes();
    }

    public float GetMostUrgentTaskTime()
    {
        float time = 1000;
        foreach (Task task in tasks)
        {
            if (task.taskUp && task.timeTillFail < time) time = task.timeTillFail;
        }
        return time;
    }

    private void Awake()
    {
        carWobble = gameObject.GetComponent<CarWobble>();
        car = transform.Find("Car");

        heartMeter = transform.Find("Car").Find("CarCanvas").Find("HeartMeter").GetComponent<ChunkMeter>();
        heartMeter.maxVal = hearts;
        heartMeter.numChunks = hearts;
        heartMeter.Value = hearts;

        if (gameObject.tag == "Car") {
            starMeter = transform.Find("Car").Find("CarCanvas").Find("StarMeter").GetComponent<ChunkMeter>();
            starMeter.maxVal = stars;
            starMeter.numChunks = stars;
            starMeter.Value = stars;
        }

        if (fire == null) Debug.LogError("No fire prefab!");

    }

    void Start()
    {
        tasks = gameObject.GetComponents<Task>();
        FindObjectOfType<Train>().CarSelectionChanged.AddListener(OnTrainSelectionChanged);
        if(card) Debug.LogFormat("Associated Card: {0} - has {1} staff", card.gameObject.name, card.GetAttachedStaff().Length);
    
        heartMeter.Value = hearts;

        if (gameObject.tag == "Car")
        {
            starMeter.Value = stars;
        }

        Debug.LogFormat("hearts {0} - stars {1}", hearts, stars);
    }

    public void RefreshCounters() {
        heartMeter.maxVal = hearts;
        heartMeter.numChunks = hearts;
        heartMeter.Value = hearts;

        if (gameObject.tag == "Car") {
            starMeter.maxVal = stars;
            starMeter.numChunks = stars;
            starMeter.Value = stars;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && selected && isValidKey())
        {
            bool goodkey = false;
            foreach (Task task in tasks)
            {
                if (task.TryKey())
                {
                    goodkey = true;
                    break;
                }
            }
            if (!goodkey)
            {
                foreach (Task task in tasks) task.KeyFail();
            }
        }
    }

    private bool isValidKey()
    {
        foreach ( KeyCode key in nonKeys)
        {
            if (Input.GetKeyDown(key)) return false;
        }
        return true;
    }

    private void OnTrainSelectionChanged(int newSelection) {
        selected = (newSelection == associatedTrainCar);
    }
}
