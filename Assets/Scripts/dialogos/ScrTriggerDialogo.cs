using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class ScrTriggerDialogo : MonoBehaviour
{
    public string[] dialogo;
    public string[] dialogo1;
    public int dialogoId;
    public int dialogoId1;


    public GameObject dialogoPainel;
    public TextMeshProUGUI dialogoTexto;
    public TextMeshProUGUI dialogoTexto1;
    public Image dialogoImagem;
    public Sprite dialogoSprite;

    public static bool dialogoPronto;
    public static bool dialogoComecar;
    bool lido = false;

    private void Awake()
    {
        PlayerPrefs.SetInt("pontos", 0);
    }
    void Start()
    {
        dialogoPainel.SetActive(false);
        PlayerPrefs.SetInt("pontos", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogoPronto && !lido)
        {
            if (!dialogoComecar)
            {
                FindAnyObjectByType<ScrMovimento>().moveSpeed = 0f;
                DialogoComecar();
            }
            else if ((dialogoTexto.text == dialogo[dialogoId]) && dialogoTexto1.text == dialogo1[dialogoId1])
            {
                if (Input.GetButtonDown("Fire3") && dialogoPronto)
                {
                    lido = true;
                    DialogoProximo();
                }
            }
        }
    }

    void DialogoProximo()
    {
        dialogoId++;
        if (dialogoId < dialogo.Length)
        {
            StartCoroutine(DialogoMostrar());
        }
        else
        {
            dialogoPainel.SetActive(false);
            dialogoComecar = false;
            dialogoId = 0;
            FindAnyObjectByType<ScrMovimento>().moveSpeed = FindAnyObjectByType<ScrMovimento>().moveSpeedAtual;
        }
        dialogoId1++;
        if (dialogoId1 < dialogo1.Length)
        {
            StartCoroutine(DialogoMostrar());
        }
        else
        {
            dialogoPainel.SetActive(false);
            dialogoComecar = false;
            //ScrDialogos.dialogoComecar = false;
            dialogoId1 = 0;
            FindAnyObjectByType<ScrMovimento>().moveSpeed = FindAnyObjectByType<ScrMovimento>().moveSpeedAtual;
        }
    }

    void DialogoComecar()
    {
        
        dialogoImagem.sprite = dialogoSprite;
        dialogoComecar = true;
        //ScrDialogos.dialogoComecar = true;
        dialogoId = 0;
        dialogoId1 = 0;
        dialogoPainel.SetActive(true);
        
        StartCoroutine(DialogoMostrar());
    }

    IEnumerator DialogoMostrar()
    {
        dialogoTexto.text = "";
        dialogoTexto1.text = "";
        foreach (char letter in dialogo[dialogoId])
        {
            dialogoTexto.text += letter;

            yield return new WaitForSeconds(0.025f);
        }
        foreach (char letter in dialogo1[dialogoId])
        {

            dialogoTexto1.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            dialogoPronto = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogoPronto = false;
        }
    }
}
