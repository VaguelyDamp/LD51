using UnityEngine;

public class CarWobble : MonoBehaviour
{
	private Train train;

    public float wobbleMagnitude = 5.0f;
    public float wobbleRate = 0.2f;

    public float bounceMagnitude = 0.3f;
    public float bounceRate = 0.1f;

    private float wobbleOffset;
    private float bounceOffset;

    private Vector3 trackPos;
    private Quaternion baseRot;

    public Transform toWobble;

    public bool iBeWobblin = true;

    private void Start() {
        train = FindObjectOfType<Train>();
        wobbleOffset = Random.Range(0, 2 * Mathf.PI);
        bounceOffset = Random.Range(0, 2 * Mathf.PI);
        trackPos = transform.position + Vector3.down;
        baseRot = transform.rotation;

        if(!train) {
            Debug.LogError("No Train!");
            Destroy(this);
        }

        if(!toWobble) {
            toWobble = transform.Find("Car");
        }
    }

    public void resetRot(Quaternion rot)
    {
        baseRot = rot;
    }

    private void Update() {
        if (iBeWobblin)
        {
            float wobbleVal = (train.CurrentSpeed / train.MaxSpeed) * wobbleMagnitude * Mathf.Sin((Time.time + wobbleOffset) * (wobbleRate * train.CurrentSpeed));
            float bounceVal = (train.CurrentSpeed / train.MaxSpeed) * bounceMagnitude * Mathf.Sin((Time.time + bounceOffset) * (bounceRate * train.CurrentSpeed));
            toWobble.rotation = baseRot*Quaternion.AngleAxis(wobbleVal, Vector3.forward);
            toWobble.position = trackPos + Vector3.up + new Vector3(0, bounceMagnitude + bounceVal);
        }       
    }
}