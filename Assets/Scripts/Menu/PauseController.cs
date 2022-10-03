using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void PauseButton()
    {
        Time.timeScale = 0.0f;
        _pauseMenu.SetActive(true);
        _pauseButton.SetActive(false);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
        _pauseButton.SetActive(true);
    }
}
