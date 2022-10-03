using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffSpot : MonoBehaviour
{
    public Sprite janitorSpriteFull;
    public Sprite janitorSpriteEmpty;
    public Sprite engineerSpriteFull;
    public Sprite engineerSpriteEmpty;
    public Sprite conductorSpriteFull;
    public Sprite conductorSpriteEmpty;
    public Sprite cookSpriteFull;
    public Sprite cookSpriteEmpty;

    public StaffCard.StaffType staffType;

    public GameObject slotGameObject;

    public bool filled = false;

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

    private void Start() {
        RefreshSprite();
    }

    public void RefreshSprite() {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "trainplace" && !filled) {
            gameObject.SetActive(false);
        }
        else {
            transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite 
                = GetSpriteForStaff(staffType, filled);

            transform.Find("Frame").GetComponent<UnityEngine.UI.Image>().enabled = !filled;
        }
        
    }

    public void OnDrop(UnityEngine.EventSystems.BaseEventData eventData) {
        StaffCard sc = CardDrag.selectedCard;

        if(sc && !filled && sc.staffType == staffType) {
            Debug.LogFormat("Dropped Staff Card {0}", sc.gameObject.name);
            DeckManager.instance.AddToHand(sc.gameObject).transform.parent = slotGameObject.transform;
            FindObjectOfType<CardDealer>().MarkStaffCardAssigned(sc.gameObject);

            filled = true;
            RefreshSprite();
        }
    }
}
