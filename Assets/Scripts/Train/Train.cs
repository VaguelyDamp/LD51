using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float MaxSpeed = 50;

    public float CurrentSpeed;

    private void Start() {
        CurrentSpeed = this.MaxSpeed;
    }
}
