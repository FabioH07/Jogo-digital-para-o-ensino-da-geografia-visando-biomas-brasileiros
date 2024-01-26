using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject creditsPanel;
    [SerializeField] private string Scene;
    

    public void PlayGame()
    {
        SceneManager.LoadScene(Scene);
    }

    public void QuitGame()
    {
        Debug.Log("Quiting game...");
        Application.Quit();

    }

    public void TutorialScene()
    {
        SceneManager.LoadScene("TUTORIALUI");
    }

    public void Voltar()
    {
        creditsPanel.SetActive(false);  
        SceneManager.LoadScene("Menu");
    }

    public void ProfessorArea()
    {

    }

    public void CreditsScreen()
    {
        creditsPanel.SetActive(true);
    }
    


}
