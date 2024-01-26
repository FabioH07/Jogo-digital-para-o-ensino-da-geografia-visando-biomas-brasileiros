using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrDialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;
    public int pontos_no_dialogo;
    private bool rewarded=false;
    public bool trivia = false;
    public bool ultimaEstrela = false;
    //for trivia
    ///public Answer answers;
    public string[] answers;

    public void StartDialogue(Vector3 collectedScorePosition)
    {
        //for trivia
        if (!ultimaEstrela)
        {
            FindObjectOfType<ScrDialogueManager>().OpenDialogue(messages, actors, answers, trivia, rewarded);
        }
        else
        {
            FindObjectOfType<ScrDialogueMComLoad>().OpenDialogue(messages, actors, answers, trivia, rewarded);
        }
        
        if (!rewarded)  
        {
            
            ScrScore.instance.PrepareScore(pontos_no_dialogo);
            ScrScore.instance.AddPoint(collectedScorePosition, pontos_no_dialogo);
            rewarded = true;
        }
    }   
}

[System.Serializable]
public class Message
{
    public int actorId;
    [TextArea(5, 15)]
    public string message;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;

}