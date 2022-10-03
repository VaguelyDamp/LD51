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

    public float secsToDest;
    private bool ded = false;
    public float initialSecondsToDestination = 60.0f;

    public AudioSource bigDieRadio;
    public AudioSource smallDieRadio;
    public AudioSource alertRadio;

    public bool disableLogic = false;

    private bool hasWon;

    public bool spawnUITrain = false;

    private void Start() {
        CurrentSpeed = this.MaxSpeed;
        hasWon = false;

        secsToDest = initialSecondsToDestination;

        if(disableLogic) { 
            foreach(CarWobble w in GetComponentsInChildren<CarWobble>()) {
                w.enabled = false;
            }
            foreach(Task t in GetComponentsInChildren<Task>()) {
                t.enabled = false;
            }
        }
        else {
            Meter.Get(ValueChannel.Progress).minVal = 0;
            Meter.Get(ValueChannel.Progress).maxVal = initialSecondsToDestination;
        }

        if(DeckManager.instance) {
            if(spawnUITrain) FindObjectOfType<UI_Train>().SpawnTrain();

            foreach(GameObject card in DeckManager.instance.GetHand()){
                CarCard carCard = card.GetComponent<CarCard>();
                if (carCard) {
                    GameObject car = AddCar(carCard.CarPrefab);
                    Car carcar = car.GetComponent<Car>();
                    carcar.card = carCard;
                    carcar.hearts = carCard.carHealth;
                    carcar.stars = carCard.value;

                    carcar.RefreshCounters();
                }
            }
        }
    }

    public void SelectCar(int index) {
        if(SelectedCar == index) index = -1;
        if(index >= cars.Count) index = -1;

        if(SelectedCar != -1)
        {
            cars[SelectedCar].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 0;
            cars[SelectedCar].GetComponent<IntFlipper>().Flip(false);
        } 
        if(index != -1)
        {
            cars[index].GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().Priority = 100;
            cars[index].GetComponent<IntFlipper>().Flip(true);
        }        
        

        SelectedCar = index;

        CarSelectionChanged.Invoke(SelectedCar);
    }

    public GameObject AddCar(GameObject carPrefab) {
        GameObject created = GameObject.Instantiate(carPrefab, Vector3.zero, Quaternion.identity, transform);
        created.transform.localPosition = carSpacing * cars.Count;
        created.transform.localRotation = Quaternion.identity;
        
        if(disableLogic) {
            created.GetComponent<CarWobble>().enabled = false;
            foreach(Task t in created.GetComponents<Task>()) {
                t.enabled = false;
            }
        }
        else {
            camTargetGroup.AddMember(created.transform, 1, 3);
            foreach(Task t in created.GetComponents<Task>()) {
                t.associatedTrainCar = cars.Count;
            }
            created.GetComponent<Car>().associatedTrainCar = cars.Count;
        }

        created.transform.Find("NumberText").GetComponent<TextMeshPro>().text = (10 - cars.Count).ToString();
        
        cars.Add(created);
        return created;
    }

    public void KillSmallCar ()
    {
        smallDieRadio.Play();
    }

    public void PlayAlertWhistle ()
    {
        alertRadio.Play();
    }

    public void GameOver()
    {
        bigDieRadio.Play();
        GameObject audio = GameObject.FindWithTag("Audio");
        audio.GetComponent<Audio>().Die();
        StartCoroutine(DoGameOver());
        ded = true;
    }
    private IEnumerator DoGameOver()
    {
        float cameraNoiseGain = 1;
        Cinemachine.CinemachineVirtualCamera trainCam = GameObject.Find("CM Whole Train").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        for (float speed = CurrentSpeed; speed > 0; speed = speed - 30*Time.deltaTime) 
        {
            CurrentSpeed = speed;
            cameraNoiseGain += .1f*Time.deltaTime;
            trainCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = cameraNoiseGain;
            yield return null;
        }
         trainCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        //Transition to game over screen
    }

    private void Update() {
        if (!ded && !disableLogic){
            DoKeyboardCarSelection();
            DestinationCountdown();
        } 
    }

    private void DestinationCountdown() {
        secsToDest -= Time.deltaTime;

        if(secsToDest <= 0) {
            if (!hasWon) {
                DeckManager.instance.stationIndex++;
                if(DeckManager.instance.stationIndex >= DeckManager.instance.stationRoundels.Length - 1) {
                    FindObjectOfType<SceneTransition>().StartSceneTransition("Winsville");
                }
                else {
                    FindObjectOfType<SceneTransition>().StartSceneTransition();
                }
                hasWon = true;
            }
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
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.DownArrow)) {
            SelectCar(-1);
        }

        if (SelectedCar != -1 && SelectedCar > 0 && Input.GetKeyDown(KeyCode.RightArrow)) {
            SelectCar(SelectedCar - 1);
        }
        else if (SelectedCar != -1 && SelectedCar < 9 && Input.GetKeyDown(KeyCode.LeftArrow)) {
            SelectCar(SelectedCar + 1);
        }
    }

    
}
