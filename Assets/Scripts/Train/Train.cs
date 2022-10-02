using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using TMPro;

public class Train : MonoBehaviour
{
    public UnityEvent<int> CarSelectionChanged;

    public float MaxSpeed = 50;

    public float CurrentSpeed;

    // Value of -1 means none
    public int SelectedCar = -1;

    public List<GameObject> cars;

    public Vector3 carSpacing = new Vector3(0, 0, -6);

    public Cinemachine.CinemachineTargetGroup camTargetGroup;

    private DeckManager deck;

    public float secsToDest;
    public float initialSecondsToDestination = 60.0f;

    private void Start() {
        CurrentSpeed = this.MaxSpeed;

        deck = FindObjectOfType<DeckManager>();

        secsToDest = initialSecondsToDestination;

        Meter.Get(ValueChannel.Progress).minVal = 0;
        Meter.Get(ValueChannel.Progress).maxVal = initialSecondsToDestination;

        if(deck) {
            foreach(GameObject card in deck.GetHand()){
                CarCard carCard = card.GetComponent<CarCard>();
                if (carCard) {
                    AddCar(carCard.CarPrefab);
                }
            }
        }
    }

    public void SelectCar(int index) {
        if(SelectedCar == index) index = -1;
        if(index >= cars.Count) index = -1;

        if(SelectedCar != -1) cars[SelectedCar].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
        if(index != -1) cars[index].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 100;
        SelectedCar = index;

        CarSelectionChanged.Invoke(SelectedCar);
    }

    public void AddCar(GameObject carPrefab) {
        GameObject created = GameObject.Instantiate(carPrefab, Vector3.zero, Quaternion.identity, transform);
        created.transform.localPosition = carSpacing * cars.Count;
        camTargetGroup.AddMember(created.transform, 1, 3);
        created.transform.Find("NumberText").GetComponent<TextMeshPro>().text = (10 - cars.Count).ToString();

        foreach(Task t in created.GetComponents<Task>()) {
            t.associatedTrainCar = cars.Count;
        }
        
        cars.Add(created);
    }

    public void GameOver()
    {
        StartCoroutine(DoGameOver());
    }
    private IEnumerator DoGameOver()
    {
        for (float speed = CurrentSpeed; speed > 0; speed = speed - 30*Time.deltaTime) 
        {
            CurrentSpeed = speed;
            yield return null;
        }
        //Transition to game over screen
    }

    private void Update() {
        DoKeyboardCarSelection();
        DestinationCountdown();
    }

    private void DestinationCountdown() {
        secsToDest -= Time.deltaTime;

        if(secsToDest <= 0) {
            FindObjectOfType<SceneTransition>().StartSceneTransition();
        }
        else {
            if(Meter.Get(ValueChannel.Progress) != null) {
                Meter.Get(ValueChannel.Progress).Value = initialSecondsToDestination - secsToDest;
            }
        }
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
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
            SelectCar(-1);
        }
    }

    
}
