using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScrDialogueManager : MonoBehaviour
{
    
    public Image actorImage;
    public Text actorName;
    public GameObject relerText;
    public Text messageText;
    public RectTransform backgroundBox;
    public AudioSource SfxPop;

    Message[] correntMenssages;
    Actor[] currentActors;

    //for trivia
    public GameObject triviaPanel;
    private string[] currentAnswers;
    [SerializeField]
    private ScrAnswerButton[] answerButtons;
    [SerializeField]
    private int correctAnswerChoice;
    private bool triviaRequest = false;

    private bool reward = false;

    int activeMessage = 0;
    public static bool isActive = false;
    //Answer
    public void OpenDialogue(Message[] messages, Actor[] actors, string[] answers, bool trivia, bool rewarded)
    {
        relerText.SetActive(false);
        reward = rewarded;
        triviaRequest = trivia;
        correntMenssages = messages;
        currentActors = actors;
        currentAnswers = answers;
        if (triviaRequest && reward) triviaRequest = false;
        activeMessage = 0;
        isActive = true;
        if (trivia)
        {
            SetAnswerValues();
        }
        Debug.Log("Started conversation Load messages:" + messages.Length);
        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f);
    }

    void DisplayMessage()
    {
        Message messageToDisplay = correntMenssages[activeMessage];
        messageText.text = messageToDisplay.message;
        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        AnimateTextColor();
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage == correntMenssages.Length - 1 && reward)
        {
            if (triviaRequest) return;
            Debug.Log("Conversation ended!");
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            StartCoroutine(Timer_to_false());
        }
        if (triviaRequest && activeMessage == correntMenssages.Length - 1 && !reward)
        {
            relerText.SetActive(true);
            triviaPanel.SetActive(true);
        }
        if (activeMessage < correntMenssages.Length)
        {
            DisplayMessage();
        }
        else
        {
            if (triviaRequest) return;
            Debug.Log("Conversation ended!");
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            StartCoroutine(Timer_to_false());
        }
    }

    void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Application.isMobilePlatform ? InputHandle.buttonDown[1] : Input.GetButtonDown("Jump")) && isActive) 
        {
            SfxPop.Play();
            relerText.SetActive(false);
            NextMessage();
            if (triviaRequest && activeMessage == correntMenssages.Length - 1 && !reward)
            {
                activeMessage = -1;
            }
        }
    }
    IEnumerator Timer_to_false()
    {
        yield return new WaitForSeconds(0.2f);
        isActive = false;
    }
    //for trivia
    private void SetAnswerValues()
    {
        List<string> answers = RandomizeAnswers(new List<string>(currentAnswers));
        for (int i = 0; i < currentAnswers.Length; i++)
        {
            bool isCorrect = false;
            if (i == correctAnswerChoice)
            {
                isCorrect = true;
            }
            answerButtons[i].SetIsCorrect(isCorrect);
            answerButtons[i].SetAnswerText(answers[i]);
        }
    }
    //for trivia
    private List<string> RandomizeAnswers(List<string> answersOriginal)
    {
        bool correctAnswerChosen = false;
        List<string> newList = new List<string>();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int random = Random.Range(0, answersOriginal.Count);

            if (random == 0 && !correctAnswerChosen)
            {
                correctAnswerChoice = i;
                correctAnswerChosen = true;
            }
            newList.Add(answersOriginal[random]);
            answersOriginal.RemoveAt(random);
        }
        return newList;
    }
    public void ButtonNextDialogue()
    {
        SfxPop.Play();
        relerText.SetActive(false);
        NextMessage();
        if (triviaRequest && activeMessage == correntMenssages.Length - 1 && !reward)
        {
            activeMessage = -1;
            
        }
    }
}
