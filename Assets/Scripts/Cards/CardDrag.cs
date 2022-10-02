using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour
{
    private Vector2 basePos;

    public static StaffCard selectedCard;

    private void Start()
    {
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
    }

    private void OnDragStart(BaseEventData eventData) {
        GetComponent<CardChooser>().enabled = false;
        Debug.LogFormat("Now draggin {0}", gameObject.name);
        selectedCard = GetComponent<StaffCard>();
    }

    private void OnDragEnd(BaseEventData eventData) {
        GetComponent<CardChooser>().enabled = true;
        Debug.LogFormat("Stopped draggin {0}", gameObject.name);
        selectedCard = null;
    }

    private void OnDrag(BaseEventData eventData) {
        PointerEventData pointerEvent = (PointerEventData)eventData;
        //Debug.LogFormat("Drag {0}", pointerEvent.position);

        Canvas canvas = FindObjectOfType<Canvas>();
        Vector3 canvasPos = pointerEvent.position;

        //GetComponent<RectTransform>().anchoredPosition = basePos + pointerEvent.position;
        transform.position = pointerEvent.position;
    }
}
