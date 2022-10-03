using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public GameObject scoreTicket;

    private DeckManager dm;
    // Start is called before the first frame update
    void Start()
    {
        dm = FindObjectOfType<DeckManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartScoring()
    {
        StartCoroutine(DoScoring());
    }
    private IEnumerator DoScoring()
    {
        //Fly in ticket
        Vector3 endTicketPos = transform.Find("EndTicketPos").position;
        Vector3 startTicketPos = scoreTicket.transform.position;
        float animTime = 0;
        float animLength = 2;

        TMPro.TextMeshProUGUI prevStarNum = scoreTicket.transform.Find("PrevStars").Find("Num").GetComponent<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI newStarNum = scoreTicket.transform.Find("NewStars").Find("Num").GetComponent<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI totalStarNum = scoreTicket.transform.Find("TotalStars").Find("Num").GetComponent<TMPro.TextMeshProUGUI>();
        prevStarNum.color = new Color(prevStarNum.color.r, prevStarNum.color.g, prevStarNum.color.b, 0);
        newStarNum.color = new Color(newStarNum.color.r, newStarNum.color.g, newStarNum.color.b, 0);
        totalStarNum.color = new Color(totalStarNum.color.r, totalStarNum.color.g, totalStarNum.color.b, 0);

        while (animTime < animLength)
        {
            animTime += Time.deltaTime;
            scoreTicket.transform.position = Vector3.Lerp(startTicketPos, endTicketPos, animTime/animLength);
            yield return null;
        }

        //Fade in old star num 
        prevStarNum.text = dm.score.ToString();
        StartCoroutine(FadeInText(prevStarNum, 1));
        yield return new WaitForSeconds(0.5f);

        //Fade in new star num  
        int newScore = 0;
        foreach (CarCard card in dm.GetComponentsInChildren<CarCard>())
        {
            newScore += card.value;
        }
        newStarNum.text = newScore.ToString();
        StartCoroutine(FadeInText(newStarNum, 1));
        yield return new WaitForSeconds(0.5f);

        //Fade in total star num       
        int totalScore = dm.score + newScore;
        totalStarNum.text = totalScore.ToString();
        dm.score = totalScore;
        StartCoroutine(FadeInText(totalStarNum, 1));
        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<CardDealer>().actualStart();
    }
    private IEnumerator FadeInText(TMPro.TextMeshProUGUI text, float time)
    {
        float animTime = 0;
        float animLength = time;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (animTime < animLength)
        {
            animTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, animTime);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
    }
}
