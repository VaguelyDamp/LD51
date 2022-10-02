using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Train : MonoBehaviour
{
    public float carWidth = 120;
    private DeckManager deck;

    public GameObject uiCarPrefab;

    public void SpawnTrain() {
        deck = FindObjectOfType<DeckManager>();

        int numCars = deck.GetCarCardCountInHand() + 1;
        Vector3 spawnPos = new Vector3((numCars / -2.0f) * carWidth, 0, 0);

        if(deck) {
            foreach(GameObject card in deck.GetHand()){
                CarCard cc = card.GetComponent<CarCard>();
                if(cc) {
                    GameObject uiCar = Instantiate(uiCarPrefab, transform);
                    uiCar.transform.localPosition = spawnPos;
                    spawnPos += new Vector3(carWidth, 0, 0);

                    uiCar.GetComponent<UI_Car>().realCard = card;
                }
            }
        }
    }
}
