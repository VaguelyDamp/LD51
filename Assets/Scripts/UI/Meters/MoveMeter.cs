using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMeter : Meter
{
    public GameObject needle;

    public Transform lowPoint;
    public Transform highPoint;

    protected override void UpdateValue()
    {
        needle.transform.localPosition = Vector3.Lerp(lowPoint.localPosition, highPoint.localPosition, normalizedVal);
        needle.transform.rotation = Quaternion.Lerp(lowPoint.rotation, highPoint.rotation, normalizedVal);
        needle.transform.localScale = Vector3.Lerp(lowPoint.localScale, highPoint.localScale, normalizedVal);
    }
}
