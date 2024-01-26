using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ScrDialogueMComLoad : MonoBehaviour
{
    [SerializeField] private string Scene;
    //public ScrLoadQuiz loadQuiz;
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;
    public AudioSource SfxPop;

    Message[] correntMenssages;
    Actor[] currentActors;


    private bool reward = false;

    int activeMessage = 0;
    public static bool isActiveLoad = false;
    //Answer
    public void OpenDialogue(Message[] messages, Actor[] actors, string[] answers, bool trivia, bool rewarded)
    {
        reward = rewarded;
        correntMenssages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActiveLoad = true;


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

            isActiveLoad = false;
            Debug.Log("Conversation ended!");
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            StartCoroutine(Timer_to_false());
        }

        if (activeMessage < correntMenssages.Length)
        {

            DisplayMessage();
        }
        else
        {
            isActiveLoad = false;
            Debug.Log("Conversation ended!");
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            LoadScene();
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
        if ((Application.isMobilePlatform ? InputHandle.buttonDown[1] : Input.GetButtonDown("Jump")) && isActiveLoad)
        {
            SfxPop.Play();

            NextMessage();
        }
    }
    IEnumerator Timer_to_false()
    {
        yield return new WaitForSeconds(0.2f);
        isActiveLoad = false;
        

    }
    public void LoadScene()
    {
        ScrScore.instance.Save_score();
        SceneManager.LoadScene(Scene);
    }
    //for trivia
}
