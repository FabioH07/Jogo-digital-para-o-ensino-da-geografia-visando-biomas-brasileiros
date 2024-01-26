using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ScrScore : MonoBehaviour
{
    public float flashDuration = 0.1f;
    public GameObject screenFlashObject;
    public GameObject screenFlashObjectB;
    public GameObject screenFlashObjectSL; //scoreloss
    public GameObject screenFlashObjectSW; //scorewin

    public AudioSource SfxScore;
    public AudioSource SfxLose;
    public static ScrScore instance;
    [SerializeField] TMP_Text scoreText;
    int scores;
    [SerializeField] GameObject animatedScorePrefab;
    [SerializeField] Transform target;

    Queue<GameObject> scoreQueue = new Queue<GameObject>();

    [SerializeField][Range(0.5f, 0.9f)] float minAnimDuration;
    [SerializeField][Range(0.9f, 2f)] float maxAnimDuration;
    [SerializeField] Ease easeType;
    Vector3 targetPosition;

    public void Awake()
    {
        instance = this;
        targetPosition = target.position;
    }

    private void FixedUpdate()
    {
        targetPosition = target.position;
    }

    public void PrepareScore(int max)
    {
        GameObject score;
        for (int i = 0; i < max; i++)
        {
            score = Instantiate(animatedScorePrefab);
            score.transform.parent = transform;
            score.SetActive(false);
            scoreQueue.Enqueue(score);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
   
        scores = PlayerPrefs.GetInt("pontos");
        scoreText.text = scores.ToString();
        

    }
    void Animate(Vector3 collectedScorePosition, int max)
    {
        for (int i = 0; i < max; i++)
        {
            if(scoreQueue.Count > 0) 
            {
                GameObject score = scoreQueue.Dequeue();
                score.SetActive(true);

                score.transform.position = collectedScorePosition;

                
                float duration = Random.Range(minAnimDuration, maxAnimDuration);
                score.transform.DOMove(targetPosition, duration).SetEase(easeType).OnComplete(() =>
                {
                    SfxScore.PlayOneShot(SfxScore.clip, 1);
                    score.SetActive(false);
                    scoreQueue.Enqueue(score);
                    scores += 1;
                    scoreText.text = scores.ToString();
                });

            }
        }
    }

    public void AddPoint(Vector3 collectedScorePosition,int pontos_no_dialogo)
    {
        Animate(collectedScorePosition, pontos_no_dialogo);

    }
    public void FastPoint(int pontos)
    {
        StartCoroutine(FlashRoutine2());
        SfxScore.PlayOneShot(SfxScore.clip, 1);
        scores += 1;
        scoreText.text = scores.ToString();
    }
    public void RemovePoint(bool a)
    {

        FlashScreen(a);
        SfxLose.PlayOneShot(SfxLose.clip, 1);
        scores--;
        scoreText.text = scores.ToString();
    }
    public int Get_score()
    {
        return scores;
    }
    public void Save_score()
    {        
        PlayerPrefs.SetInt("pontos", scores);
    }
    public void FlashScreen(bool a)
    {
        StartCoroutine(FlashRoutine(a));
    }

    private IEnumerator FlashRoutine(bool a)
    {
        if (a) // a significa que não é dialogo
        {
            screenFlashObjectB.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            screenFlashObjectB.SetActive(false);
        }
        else
        {
            screenFlashObjectSL.SetActive(true);
            screenFlashObject.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            screenFlashObject.SetActive(false);
            screenFlashObjectSL.SetActive(false);
        }
    }
    private IEnumerator FlashRoutine2()
    {
        screenFlashObjectSW.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        screenFlashObjectSW.SetActive(false);
    }
}
