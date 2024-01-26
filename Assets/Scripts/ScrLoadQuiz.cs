using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScrLoadQuiz : MonoBehaviour
{
    [SerializeField] private string Scene;
    //[SerializeField] int score;
    public void LoadScene()
    {
        SceneManager.LoadScene(Scene);
    }
    public void SaveScore()
    {
        ScrScore.instance.Save_score();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(ScrDialogueManager.isActive == false)
        {
            SaveScore();
            LoadScene();
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ScrDialogueManager.isActive == false)
        {
            SaveScore();
            LoadScene();
        }
    }
}
