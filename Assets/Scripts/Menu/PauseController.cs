using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{


    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Menu()
    {
        SceneManager.LoadScene(1);
    }

}
