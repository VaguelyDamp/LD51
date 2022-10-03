using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winning : MonoBehaviour
{
    private GameObject ticket;
    private Vector3 endTicketPos;

    void Start()
    {
        ticket = transform.Find("Ticket").gameObject;
        endTicketPos = transform.Find("EndTicketPos").position;
        ticket.transform.Find("Explanation").GetComponent<TMPro.TextMeshProUGUI>().text = "You were able to feed the engine coal every 10 seconds and delivered some stuff along the way.\n\nFinal Score: "+FindObjectOfType<DeckManager>().score;
        StartCoroutine(MoveTicket());
    }

    private IEnumerator MoveTicket()
    {
        Vector3 startPos = ticket.transform.position;
        float curTime = 0;
        float animTime = 4;
        while (curTime < animTime)
        {
            ticket.transform.position = Vector3.Lerp(startPos, endTicketPos, curTime/animTime);
            curTime += Time.deltaTime;
            yield return null;
        }
        transform.Find("MainMenu").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}