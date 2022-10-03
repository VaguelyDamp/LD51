using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundelGetter : MonoBehaviour
{
    public int offset = 0;

    void Start() {
        DeckManager deck = DeckManager.instance;

        GetComponent<Image>().sprite = deck.stationRoundels[deck.stationIndex + offset];
        GetComponent<Image>().preserveAspect = true;
    }
}
