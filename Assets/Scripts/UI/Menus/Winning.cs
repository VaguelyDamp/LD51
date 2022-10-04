using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Winning : MonoBehaviour
{
    private GameObject ticket;
    private Vector3 endTicketPos;

    void Start()
    {
        ticket = transform.Find("Ticket").gameObject;

        endTicketPos = ticket.transform.parent.Find("EndTicketPos").position;

        int score = 0;
        int newScore = 0;
        foreach (CarCard card in DeckManager.instance.GetComponentsInChildren<CarCard>())
        {
            newScore += card.value;
        }
        if(DeckManager.instance){
            score = DeckManager.instance.score + newScore;
        }
        ticket.transform.Find("Explanation").GetComponent<TMPro.TextMeshProUGUI>().text = "You were able to feed the engine coal every 10 seconds and delivered some stuff along the way.\n\nFinal Score: "+score;
        StartCoroutine(MoveTicket());
    }

    private IEnumerator MoveTicket()
    {
        ticket.SetActive(true);
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
        foreach (Transform child in DeckManager.instance.transform){
            Destroy(child.gameObject);
        }
    }

    public void GoToMenu ()
    {
        SceneManager.LoadScene(1);
        DeckManager.instance.ResetDeck();
    }
}
