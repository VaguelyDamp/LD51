using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntFlipper : MonoBehaviour
{
    public GameObject[] exteriors;
    public GameObject[] interiors;
    public bool isInterior = false;
    void Start()
    {
        Flip(isInterior);
    }

    public void Flip(bool toInterior)
    {
        foreach (GameObject interior in interiors)
        {
            interior.SetActive(toInterior);
        }
        foreach (GameObject exterior in exteriors)
        {
            exterior.SetActive(!toInterior);
        }
    }

}
