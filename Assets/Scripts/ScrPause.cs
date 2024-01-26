using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScrPause : MonoBehaviour
{

    public GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {

        if (Application.isMobilePlatform ? InputHandle.buttonDown[0] : Input.GetButtonDown("Cancel"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Congela o tempo do jogo
        isPaused = true;
        // Exibir UI de pausa, se necessário
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Restaura o tempo do jogo
        pausePanel.SetActive(false);
        isPaused = false;
        // Ocultar UI de pausa, se necessário
    }
    public void ReturnMenu()
    {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene("Menu");
    }
    public void Continue()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        isPaused = false;
    }
    /*
     * 
     
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            StartCoroutine(prePause());
        }
    }

    IEnumerator prePause()
    {
        pausePanel.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Continue()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void returnMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
    */

}
