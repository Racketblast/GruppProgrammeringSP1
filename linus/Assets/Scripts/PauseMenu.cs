using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool pause = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;// S�tter ig�ng spelv�rldens tid
        pause = false;
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;// St�nger av spelv�rldens tid
        pause = true;// S�tter ig�ng paus scenen
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;// Om man inte har den h�r kommer spelet vara fruset i main menu
        SceneManager.LoadScene(0);// Skickar en till f�rsta scenen
    }

    public void ReloadGame()
    {
        pause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);// Reloadar scenen
    }
}
