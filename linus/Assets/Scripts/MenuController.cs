using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowCredits() 
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}
