using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    int stars = 1;
    int hearts = 3;

    public bool carDead = false;


    [SerializeField]
    private AnimationCurve fallCurve;
    private float fallTime = 2.5f;

    private ChunkMeter heartMeter;
    private CarWobble carWobble;
    private Transform car;

    private bool shakinBakin = false;

    public void TaskFailed()
    {
        hearts -= 1;
        Debug.Log("Car: "+name+" has "+hearts+" hearts");
        heartMeter.Value = hearts;
        StartCoroutine(DamageShake());
        if (hearts <= 0) KillCar();
    }
    private IEnumerator DamageShake()
    {
        //Turn car red
        //carWobble.iBeWobblin = false;
        shakinBakin = true;
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

    private void KillCar()
    {
        stars = 0;
        hearts = 0;
        carDead = true;
        foreach (Task task in gameObject.GetComponents<Task>()) task.KillCar();
        StartCoroutine(CarFall());
    }
    private IEnumerator CarFall()
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
        //Do fire here
        //gameObject.GetComponent<CarWobble>().iBeWobblin = true;
    }

    void Start()
    {
        heartMeter = transform.Find("Car").Find("CarCanvas").Find("HeartMeter").GetComponent<ChunkMeter>();
        heartMeter.maxVal = hearts;
        heartMeter.numChunks = hearts;
        heartMeter.Value = hearts;

        carWobble = gameObject.GetComponent<CarWobble>();
        car = transform.Find("Car");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
