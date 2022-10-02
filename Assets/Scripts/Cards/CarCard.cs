using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCard : MonoBehaviour
{
    public GameObject CarPrefab;

    public int carHealth;
    public int value;

    public StaffCard.StaffType[] staffSlots;

    public Sprite uiSprite;

    private void Awake() {
        foreach(StaffCard.StaffType slot in staffSlots) {
            GameObject slotObj = new GameObject(string.Format("StaffSpot:{0}", slot.ToString()));
            slotObj.transform.parent = transform;
        }
    }

    public int GetSlotCountOfType(StaffCard.StaffType staffType) {
        int count = 0;
        foreach(StaffCard.StaffType slot in staffSlots) {
            if(slot == staffType) {
                count++;
            }
        }
        return count;
    }
}
