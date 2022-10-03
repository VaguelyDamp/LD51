using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour
{
    private GameObject pauseMenu;
    public bool isPaused;
    void Start()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("Canvas").transform.Find("PauseMenu").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                DoPause();

            }
            else
            {
                DoUnPause();
            }
        }
    }

    public void DoPause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        isPaused = true;
    }
    public void DoUnPause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
