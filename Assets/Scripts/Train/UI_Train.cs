using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_Train : MonoBehaviour
{
    public float carWidth = 120;
    private DeckManager deck;
    private List<GameObject> ui_cars = new List<GameObject>();

    public GameObject uiCarPrefab;
    public GameObject uiEnginePrefab;

    public void SpawnTrain() {
        deck = FindObjectOfType<DeckManager>();

        int numCars = deck.GetCarCardCountInHand() + 1;
        Vector3 spawnPos = new Vector3((numCars / -2.0f) * carWidth, 0, 0);    
        
        if (SceneManager.GetActiveScene().name == "trainplace")
        {
            GameObject ui_engine = Instantiate(uiEnginePrefab, transform);
            UI_Flasher flash = ui_engine.AddComponent<UI_Flasher>();
            flash.actualCarNum = 0;
            ui_cars.Add(ui_engine);
        } 
        

        if(deck) {
            foreach(GameObject card in deck.GetHand()){
                CarCard cc = card.GetComponent<CarCard>();
                if(cc) {
                    GameObject uiCar = Instantiate(uiCarPrefab, transform);
                    uiCar.transform.localPosition = spawnPos;
                    spawnPos += new Vector3(carWidth, 0, 0);

                    uiCar.GetComponent<UI_Car>().realCard = card;
                    
                    ui_cars.Add(uiCar);
                    if (SceneManager.GetActiveScene().name == "trainplace")
                    {
                        UI_Flasher flash = uiCar.AddComponent<UI_Flasher>();
                        flash.actualCarNum = ui_cars.Count - 1;
                    }
                }
            }
        }
    }
}
