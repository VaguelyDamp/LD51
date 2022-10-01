using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    void Awake()
    {
        TextMeshPro myTextMesh = GetComponent<TextMeshPro>();
        myTextMesh.text = "TEXT on a screen!!";

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject menuText = new GameObject();
}
