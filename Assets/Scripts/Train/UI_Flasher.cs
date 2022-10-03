using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Flasher : MonoBehaviour
{
    public int actualCarNum;
    private float indicateTime;
    private float curTime;

    private GameObject actualCar;

    void Start()
    {
        curTime = 0;
        actualCar = FindObjectOfType<Train>().cars[actualCarNum];
    }

    // Update is called once per frame
    void Update()
    {
        
        float urgentTaskTime = actualCar.GetComponent<Car>().GetMostUrgentTaskTime();
        if (urgentTaskTime != 1000)
        {
            indicateTime = urgentTaskTime;
            transform.Find("CarSprite").GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(Color.white, Color.red, Mathf.Sin((1/indicateTime)*curTime));
            curTime += Time.deltaTime;
        }
        else transform.Find("CarSprite").GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }
}
