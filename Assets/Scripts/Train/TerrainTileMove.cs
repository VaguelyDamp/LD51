using UnityEngine;

public class TerrainTileMove : MonoBehaviour
{
	private Train train;

    public float snapBackAt = -200;

    public Transform[] tiles;
    public float tileSize = 100;

    private int currentHead;

    

    private void Start() {
        train = FindObjectOfType<Train>();

        if(!train) {
            Debug.LogError("No Train!");
            Destroy(this);
        }

        currentHead = 0;
    }

    private int GetBefore(int index) {
        if(index > 0) return index - 1;
        else return tiles.Length - 1;
    }

    private int GetAfter(int index) {
        if(index < tiles.Length - 1) return index + 1;
        else return 0;
    }

    private void Update() {
        float moveAmount = train.CurrentSpeed * Time.deltaTime;
        Vector3 moveVec = new Vector3(0, 0, -moveAmount);
        
        foreach(Transform tile in tiles) {
            tile.position += moveVec;
        }

        if(tiles[currentHead].localPosition.z < snapBackAt) {
            tiles[currentHead].localPosition = tiles[GetBefore(currentHead)].localPosition + new Vector3(0, 0, tileSize);
            currentHead = GetAfter(currentHead);
        }
    }
}
