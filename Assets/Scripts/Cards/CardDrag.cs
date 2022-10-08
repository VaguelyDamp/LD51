using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour
{
    private Vector2 basePos;

    public static StaffCard selectedCard;

    public AudioClip dragBoop;

    private AudioSource sauce;

    public bool draggable = true;
    private bool isDragging = false;

    private List<GameObject> toFlash = new List<GameObject>();

    private void Start()
    {
        sauce = GetComponent<AudioSource>();

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry onDragEntry = new EventTrigger.Entry();
        onDragEntry.eventID = EventTriggerType.Drag;
        onDragEntry.callback.AddListener(OnDrag);
        trigger.triggers.Add(onDragEntry);

        EventTrigger.Entry onDragStartEntry = new EventTrigger.Entry();
        onDragStartEntry.eventID = EventTriggerType.BeginDrag;
        onDragStartEntry.callback.AddListener(OnDragStart);
        trigger.triggers.Add(onDragStartEntry);

        EventTrigger.Entry onDragEndEntry = new EventTrigger.Entry();
        onDragEndEntry.eventID = EventTriggerType.EndDrag;
        onDragEndEntry.callback.AddListener(OnDragEnd);
        trigger.triggers.Add(onDragEndEntry);

        basePos = GetComponent<RectTransform>().anchoredPosition;

        StaffCard sc = gameObject.GetComponent<StaffCard>();
        if (sc)
        {
            if (sc.staffType == StaffCard.StaffType.DampBoi)
            {
                draggable = false;
                gameObject.GetComponent<UnityEngine.UI.Image>().color = GetComponent<CardChooser>().rejectionColor;
            } 
        } 
    }

    private void OnDragStart(BaseEventData eventData) {
        if (draggable)
        {
            GetComponent<CardChooser>().enabled = false;
            Debug.LogFormat("Now draggin {0}", gameObject.name);
            selectedCard = GetComponent<StaffCard>();
            isDragging = true;

            sauce.PlayOneShot(dragBoop);

            foreach(Transform uiCar in GameObject.FindGameObjectWithTag("Canvas").transform.Find("UI_Train"))
            {
                foreach(StaffSpot sp in uiCar.GetComponentsInChildren<StaffSpot>())
                {
                    if(sp.staffType == selectedCard.staffType || selectedCard.staffType == StaffCard.StaffType.DampBoi) toFlash.Add(sp.gameObject);
                }
            }
            
            StartCoroutine(FlashFrame());
        }
        else{
            GetComponent<CardChooser>().StartRejectionShake();
        }     
    }

    private void OnDragEnd(BaseEventData eventData) {
        if (draggable)
        {
            GetComponent<CardChooser>().enabled = true;
            Debug.LogFormat("Stopped draggin {0}", gameObject.name);
            selectedCard = null;
            isDragging = false;

            foreach(GameObject flasher in toFlash)
            {
                StaffSpot sp = flasher.GetComponent<StaffSpot>();
                flasher.transform.Find("Frame").GetComponent<UnityEngine.UI.Image>().color = sp.origColor;
            }
        }
    }

    private void OnDrag(BaseEventData eventData) {
        PointerEventData pointerEvent = (PointerEventData)eventData;
        //Debug.LogFormat("Drag {0}", pointerEvent.position);

        if (draggable)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            Vector3 canvasPos = pointerEvent.position;

            //GetComponent<RectTransform>().anchoredPosition = basePos + pointerEvent.position;
            transform.position = pointerEvent.position;

            
        }  
    }

    private IEnumerator FlashFrame()
    {
        float flashTime = 0;
        while(isDragging)
        {
            flashTime += Time.deltaTime;
            foreach(GameObject flasher in toFlash)
            {
                StaffSpot sp = flasher.GetComponent<StaffSpot>();
                flasher.transform.Find("Frame").GetComponent<UnityEngine.UI.Image>().color = Color.Lerp(sp.origColor, sp.flashColor, 0.5f*(Mathf.Sin(flashTime*10)+1));
            }
            yield return null;
        }
    }
}
