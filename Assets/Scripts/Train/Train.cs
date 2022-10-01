using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float MaxSpeed = 50;

    public float CurrentSpeed;

    // Value of -1 means none
    public int SelectedCar = -1;

    public GameObject[] cars;

    private void Start() {
        CurrentSpeed = this.MaxSpeed;
    }

    public void SelectCar(int index) {
        if(SelectedCar != index) {
            if(SelectedCar != -1) cars[SelectedCar].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
            if(index != -1) cars[index].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 100;
            SelectedCar = index;
        }
    }

    private void DoKeyboardCarSelection() {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectCar(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectCar(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectCar(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SelectCar(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SelectCar(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SelectCar(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SelectCar(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SelectCar(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            SelectCar(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0)) {
            SelectCar(9);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            SelectCar(-1);
        }
    }

    private void Update() {
        DoKeyboardCarSelection();
    }
}
