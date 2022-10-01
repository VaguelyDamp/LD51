using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float MaxSpeed = 50;

    public float CurrentSpeed;

    // Value of -1 means none
    public int SelectedCar = -1;

    public List<GameObject> cars;

    public GameObject CarPrefab;
    public Vector3 carSpacing = new Vector3(0, 0, -6);

    public Cinemachine.CinemachineTargetGroup camTargetGroup;

    private void Start() {
        CurrentSpeed = this.MaxSpeed;
    }

    public void SelectCar(int index) {
        if(SelectedCar == index) index = -1;
        if(index >= cars.Count) index = -1;

        if(SelectedCar != -1) cars[SelectedCar].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
        if(index != -1) cars[index].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 100;
        SelectedCar = index;
    }

    public void AddCar() {
        GameObject created = GameObject.Instantiate(CarPrefab, Vector3.zero, Quaternion.identity, transform);
        created.transform.localPosition = carSpacing * cars.Count;
        camTargetGroup.AddMember(created.transform, 1, 3);
        cars.Add(created);
    }

    private void DoKeyboardCarSelection() {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectCar(9);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectCar(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectCar(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SelectCar(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SelectCar(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SelectCar(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SelectCar(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SelectCar(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) {
            SelectCar(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0)) {
            SelectCar(0);
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            SelectCar(-1);
        }
    }

    private void Update() {
        DoKeyboardCarSelection();

        if(Input.GetKeyDown(KeyCode.UpArrow) && cars.Count < 10) {
            AddCar();
        }
    }
}
