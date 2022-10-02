using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    int stars = 1;
    int hearts = 3;


    [SerializeField]
    private AnimationCurve fallCurve;
    private float fallTime = 2.5f;

    public void TaskFailed()
    {
        hearts -= 1;
        if (hearts <= 0) KillCar();
    }

    private void KillCar()
    {
        stars = 0;
        hearts = 0;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
