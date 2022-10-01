using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterValueChangerTest : MonoBehaviour
{
    public float minVal = 0;
    public float maxVal = 10;
    public float speed = 2;

    public ValueChannel testChannel = ValueChannel.Health;

    float curVal;

    void Start()
    {
        curVal = minVal;
        Meter.Get(testChannel).minVal = minVal;
        Meter.Get(testChannel).maxVal = maxVal;
        Meter.Get(testChannel).Value = curVal;
    }

    // Update is called once per frame
    void Update()
    {
        curVal = Helpers.Remap(Mathf.Cos(Time.time * speed), -1, 1, minVal, maxVal);
        Meter.Get(testChannel).Value = curVal;
    }
}
