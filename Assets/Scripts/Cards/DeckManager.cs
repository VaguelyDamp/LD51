using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject[] allCardPrefabs;
    public GameObject[] baseDeck;

    public List<GameObject> deck;

    public List<GameObject> hand;

    public List<GameObject> discard;

    public string[] stationNames;
    public Sprite[] stationRoundels;

    public int stationIndex = 0;

    public static DeckManager instance;

    public int score;

    public Sprite janitorSpriteFull;
    public Sprite janitorSpriteEmpty;
    public Sprite engineerSpriteFull;
    public Sprite engineerSpriteEmpty;
    public Sprite conductorSpriteFull;
    public Sprite conductorSpriteEmpty;
    public Sprite cookSpriteFull;
    public Sprite cookSpriteEmpty;

    public StaffCard.StaffType staffType;

    public bool UseSet = false;
    public GameObject[] set;


    public Sprite GetSpriteForStaff(StaffCard.StaffType staffType, bool full) {
        switch(staffType) {
            case StaffCard.StaffType.Janitor:
                return full ? janitorSpriteFull : janitorSpriteEmpty;
            case StaffCard.StaffType.Engineer:
                return full ? engineerSpriteFull : engineerSpriteEmpty;
            case StaffCard.StaffType.Conductor:
                return full ? conductorSpriteFull : conductorSpriteEmpty;
            case StaffCard.StaffType.Cook:
                return full ? cookSpriteFull : cookSpriteEmpty;
            default:
                return null;
        }
    }

    private void Awake() {
        if(instance != null) {
            Destroy(gameObject);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);

            ResetDeck();
            LoadDeck();
        }
    }

    private void Start()
    {
       
    }

    public void LoadDeck()
    {
        foreach (GameObject cardPrefab in allCardPrefabs)
        {
            if (PlayerPrefs.HasKey(cardPrefab.name))
            {
                AddToDeck(cardPrefab, PlayerPrefs.GetInt(cardPrefab.name, 0));
            }
        }
    }

    // public void SaveDeck()
    // {
    //     PlayerPrefs.SetInt("DeckSaved", 1);
    //     int cardIndex = 0;
    //     foreach(GameObject card in baseDeck)
    //     {
    //         PlayerPrefs.SetString("Card"+cardIndex, card.name);
    //         cardIndex++;
    //     }
    //     PlayerPrefs.SetInt("DeckCount", cardIndex);
    // }

    public void ResetDeck() {
        deck = new List<GameObject>();
        deck.AddRange(baseDeck);

        hand = new List<GameObject>();
        discard = new List<GameObject>();

        score = 0;
        stationIndex = 0;
    }

    public GameObject Draw() {
        int index = Random.Range(0, deck.Count);
        GameObject chosen = deck[index];
        deck.RemoveAt(index);
        return chosen;
    }

    public GameObject[] Draw(int count) {
        int toDraw = Mathf.Min(count, deck.Count);
        if (UseSet)
        {
           return set;
        }
        else 
        {
            GameObject[] deal = new GameObject[toDraw];
                for(int i = 0; i < toDraw; ++i) {
                    deal[i] = Draw();
            }
            return deal;
        }
    }

    public void AddToDeck(GameObject card, int count = 1) {
        while (count > 0)
        {
            deck.Add(card);
            count--;
        }      
    }

    public void RemoveCardFromHand(GameObject card) {
        CarCard cc = card.GetComponent<CarCard>();
        if(cc) {
            foreach(StaffCard sc in cc.GetAttachedStaff()) {
                hand.Remove(sc.gameObject);
            }
        }
        hand.Remove(card);
        Destroy(card);
    }

    public GameObject AddToHand(GameObject card) {
        Debug.LogFormat("Added card {0} to hand", card.name);
        GameObject instance = Instantiate(card, transform);
        CardChooser cc = instance.GetComponent<CardChooser>();
        if(cc) {
            Destroy(cc);
        }
        CardDrag cd = instance.GetComponent<CardDrag>();
        if(cd) {
            Destroy(cd);
        }
        hand.Add(instance);

        return instance;
    }

    public int GetStaffSlotCountInHandByType(StaffCard.StaffType staffType) {
        int count = 0;

        foreach(GameObject card in hand) {
            CarCard cc = card.GetComponent<CarCard>();
            if(cc) {
                if (staffType == StaffCard.StaffType.DampBoi) count += cc.staffSlots.Length;
                else count += cc.GetSlotCountOfType(staffType);
            }
        }

        return count;
    }

    public int GetStaffCardCountInHandByType(StaffCard.StaffType staffType) {
        int count = 0;

        foreach(GameObject card in hand) {
            StaffCard sc = card.GetComponent<StaffCard>();
            if(sc) {
                if(sc.staffType == staffType) ++count;
                if(sc.staffType == StaffCard.StaffType.DampBoi) if(sc.assignedStaffType == staffType) ++count;
            }
        }
        return count;
    }

    public int GetCarCardCountInHand() {
        int count = 0;

        foreach(GameObject card in hand) {
            CarCard cc = card.GetComponent<CarCard>();
            if(cc) {
                ++count;
            }
        }
        return count;
    }

    public List<GameObject> GetHand() {
        return hand;
    }
}
