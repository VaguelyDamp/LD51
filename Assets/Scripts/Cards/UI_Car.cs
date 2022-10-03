using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Car : MonoBehaviour {
    public GameObject realCard;

    public Sprite carSprite;

    private List<GameObject> uiCardSlots;
    private List<GameObject> realCardStaffSlots;

    private string staffSpotPredicate = "StaffSpot:";

    private void Start() {
        transform.Find("CarSprite").GetComponent<UnityEngine.UI.Image>().sprite = carSprite;

        uiCardSlots = new List<GameObject>();
        foreach(Transform child in transform) {
            if(child.name.Contains("StaffSpot")) {
                uiCardSlots.Add(child.gameObject);
            }
        }

        realCardStaffSlots = new List<GameObject>();
        foreach(Transform child in realCard.transform) {
            if(child.name.StartsWith(staffSpotPredicate)) {
                realCardStaffSlots.Add(child.gameObject);
            }
        }

        int slots = Mathf.Min(uiCardSlots.Count, realCardStaffSlots.Count);
        for(int i = 0; i < slots; ++i) {
            StaffSpot uiSpot = uiCardSlots[i].GetComponent<StaffSpot>();
            uiSpot.slotGameObject = realCardStaffSlots[i];
            string spotTypeStr = realCardStaffSlots[i].name.Substring(staffSpotPredicate.Length);

            if(spotTypeStr == "Janitor") {
                uiSpot.staffType = StaffCard.StaffType.Janitor;
            }
            else if(spotTypeStr == "Engineer") {
                uiSpot.staffType = StaffCard.StaffType.Engineer;
            }
            else if(spotTypeStr == "Conductor") {
                uiSpot.staffType = StaffCard.StaffType.Conductor;
            }
            else if(spotTypeStr == "Cook") {
                uiSpot.staffType = StaffCard.StaffType.Cook;
            }
            else {
                Debug.LogWarningFormat("Found staff slot type of {0}, which doesn't match any known type", spotTypeStr);
            }

            uiSpot.RefreshSprite();
        }

        for(int i = slots; i < uiCardSlots.Count; ++i) {
            uiCardSlots[i].SetActive(false);
        }
    }

    void Update()
    {
        
    }
}
