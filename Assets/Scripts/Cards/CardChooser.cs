using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChooser : MonoBehaviour
{
    public int cardIndex = 0;

    private CardDealer dealer;

    private bool selected = false;

    public bool Selected {
        get { return selected; }
        set { selected = value; }
    }

    private Vector3 unselectPos;
    private Vector3 selectPos;

    public Vector3 selectedMove = new Vector3(0, 200, 0);

    public float moveSpeed = 1000;

    public float rejectionEffectTime = 0.5f;
    public Color rejectionColor = Color.red;
    public float rejectionShakeMagnitude = 30;

    public UnityEngine.UI.Image[] toFade;

    private bool animationActive;

    private IEnumerator rejectionCoroutine;

    public AudioClip selectBoop;
    public AudioClip rejectBoop;

    private AudioSource sauce;

    private void Start() {
        dealer = FindObjectOfType<CardDealer>();

        if (!dealer) {
            Debug.LogError("NO DEALER!!");
        }

        unselectPos = transform.position;
        selectPos = unselectPos + selectedMove;

        transform.position = unselectPos + new Vector3(0, -500 + (cardIndex * -50), 0);

        animationActive = false;

        sauce = GetComponent<AudioSource>();
    }


    public void SelectCard() {
        bool wasSelected = selected;
        selected = dealer.RegisterCardSelection(cardIndex);
        if(wasSelected == selected) {
            if (animationActive) {
                StopCoroutine(rejectionCoroutine);
            }
            rejectionCoroutine = RejectionShake();
            StartCoroutine(rejectionCoroutine);
            sauce.PlayOneShot(rejectBoop);
        }
        else if(selected) {
            sauce.PlayOneShot(selectBoop);
        }
    }

    private void Update() {
        if(!animationActive) {
            Vector3 targetPosition = selected ? selectPos : unselectPos;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator RejectionShake() {
        float timer = rejectionEffectTime;
        Vector3 basePos = transform.position;
        animationActive = true;

        while(timer > 0) {
            float t = 1 - (timer / rejectionEffectTime);

            foreach(UnityEngine.UI.Image img in toFade) {
                img.color = Color.Lerp(rejectionColor, Color.white, t);
            }

            transform.position = basePos 
                + new Vector3((1 - t) * rejectionShakeMagnitude * Mathf.Sin(t * 4 * Mathf.PI), 0, 0);

            timer -= Time.deltaTime;
            yield return null;
        }

        foreach(UnityEngine.UI.Image img in toFade) {
            img.color = Color.white;
        }

        animationActive = false;
    }
}
