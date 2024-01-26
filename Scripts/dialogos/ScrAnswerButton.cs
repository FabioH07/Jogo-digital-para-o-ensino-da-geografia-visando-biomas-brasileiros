using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrAnswerButton : MonoBehaviour
{

    private bool isCorrect;
    [SerializeField]
    private TextMeshProUGUI answerText;
    public GameObject triviaPanel;
    public GameObject dialogueManagerPanel;
    public RectTransform backgroundBox;

    public void SetAnswerText(string newText)
    {
        answerText.text = newText;
    }

    public void SetIsCorrect(bool newBool)
    {
        isCorrect = newBool;
    }

    public void OnClick()
    {
        if (isCorrect)
        {
            Debug.Log("Correct answer!");
            ScrDialogueManager.isActive = false;
            triviaPanel.SetActive(false);
            //dialogueManagerPanel.SetActive(false);
            ScrScore.instance.PrepareScore(1);
            ScrScore.instance.FastPoint(1);
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
        }
        else
        {
            ScrScore.instance.RemovePoint(false);
            Debug.Log("Wrong answer!");
        }
    }
}
