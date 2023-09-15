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
        Time.timeScale = 1.0f;
        pause = false;
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        pause = true;
    }

    public void MainMenu()
    {
        pause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void ReloadGame()
    {
        pause = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
