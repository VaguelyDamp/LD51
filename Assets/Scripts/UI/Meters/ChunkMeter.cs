using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChunkMeter : Meter
{
    private Image[] chunks;

    public int numChunks = 10;

    public bool doPartial = false;
    public Image.FillMethod partialChunkFillMethod = Image.FillMethod.Horizontal;
    public int partialChunkFillOrigin = 0;

    public Sprite chunkSprite;
    public Vector3 chunkSize;
    public Vector3 chunkSpacing;

    private void Start()
    {
        chunks = PlaceChunks(numChunks).ToArray();
    }

    protected override void UpdateValue()
    {
        if(chunks == null || chunks.Length == 0) 
        {
            Debug.LogWarning("No chunks for chunk meter!");
            return;
        }

        float barFill = (normalizedVal * chunks.Length - 1) + 1;
        int barsFilled = (int) barFill;
        float partialFill = doPartial ? (barFill - barsFilled) : 0.0f;

        for(int i = 0; i < chunks.Length; ++i)
        {
            chunks[i].enabled = i <= barsFilled;

            if(i == barsFilled) 
            {
                chunks[i].fillAmount = partialFill;
            }
            else 
            {
                chunks[i].fillAmount = 1;
            }
        }
    }

    private List<Image> PlaceChunks(int numChunks)
    {
        List<Image> chunks = new List<Image>();
        for(int i = 0; i < numChunks; ++i) 
        {
            chunks.Add(PlaceSingleChunk(i * chunkSpacing).GetComponent<Image>());
        }
        return chunks;
    }

    private GameObject PlaceSingleChunk(Vector3 position)
    {
        GameObject chunk = new GameObject("barChunk");
        chunk.transform.parent = transform;
        chunk.transform.localPosition = position;
        chunk.transform.localScale = new Vector3(
            chunkSize.x / chunkSprite.bounds.size.x,
            chunkSize.y / chunkSprite.bounds.size.x,
            1
        );

        Image image = chunk.AddComponent<Image>();
        image.sprite = chunkSprite;
        image.type = Image.Type.Filled;
        image.fillMethod = partialChunkFillMethod;
        image.fillOrigin = partialChunkFillOrigin;

        return chunk;
    }
}
