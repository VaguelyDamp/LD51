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

    public void TaskFailed()
    {
        hearts -= 1;
        Debug.Log("Car: "+name+" has "+hearts+" hearts");
        heartMeter.Value = hearts;
        if (hearts <= 0) KillCar();
    }

    private void KillCar()
    {
        stars = 0;
        hearts = 0;
        carDead = true;
        StartCoroutine(CarFall());
    }
    private IEnumerator CarFall()
    {
        Transform car = transform.Find("Car");
        Quaternion startPos = car.rotation;
        Quaternion endPos = startPos*Quaternion.AngleAxis(90, Vector3.forward);
        gameObject.GetComponent<CarWobble>().iBeWobblin = false;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
