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
        Time.timeScale = 1.0f;// Sätter igång spelvärldens tid
        pause = false;
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;// Stänger av spelvärldens tid
        pause = true;// Sätter igång paus scenen
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;// Om man inte har den här kommer spelet vara fruset i main menu
        SceneManager.LoadScene(0);// Skickar en till första scenen
    }

    public void ReloadGame()
    {
        pause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);// Reloadar scenen
    }
}
