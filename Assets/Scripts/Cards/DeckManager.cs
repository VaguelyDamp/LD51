using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
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

            deck = new List<GameObject>();
            deck.AddRange(baseDeck);

            hand = new List<GameObject>();
            discard = new List<GameObject>();

            score = 0;

            DontDestroyOnLoad(gameObject);
        }
    }

    public GameObject Draw() {
        int index = Random.Range(0, deck.Count);
        GameObject chosen = deck[index];
        deck.RemoveAt(index);
        return chosen;
    }

    public GameObject[] Draw(int count) {
        int toDraw = Mathf.Min(count, deck.Count);
        GameObject[] deal = new GameObject[toDraw];
        for(int i = 0; i < toDraw; ++i) {
            deal[i] = Draw();
        }
        return deal;
    }

    public void AddToDeck(GameObject card) {
        deck.Add(card);
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
                count += cc.GetSlotCountOfType(staffType);
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
