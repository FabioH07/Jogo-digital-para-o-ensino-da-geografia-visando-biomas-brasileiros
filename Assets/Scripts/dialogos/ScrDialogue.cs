using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading;

public class ScrDialogue : MonoBehaviour
{
    public ScrOpenInventory Close;
    public ScrDialogueTrigger trigger;
    private bool a = false;
    private bool lido = false;
    public bool obrigatorio = false;
    private Vector3 place;
    public AudioSource SfxPop;
    private bool interagiu = false;
    public bool temEstrela = false;
    private void Update()
    {
        if (a && !obrigatorio)
        {
            if ((Application.isMobilePlatform ? InputHandle.buttonDown[1] : Input.GetButtonDown("Jump")) && !ScrDialogueManager.isActive && !ScrOpenInventory.isOpen)
            {
                SfxPop.Play();
                StartCoroutine(StartDialogue());
            }
        }
        else if (a && !ScrDialogueManager.isActive && !lido)
        {
            Close.CloseInventory();
            StartCoroutine(StartDialogue());
            lido = true;
        }
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(0.005f);
        trigger.StartDialogue(place);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            a = true;
            
            place = collision.transform.position;
            GameObject objetoColidido = collision.gameObject;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            Interagir();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            a = false;
        }
    }
    public void Interagir()
    {
        if (!interagiu && temEstrela)
        {
            interagiu = true;
            StartCoroutine(WaitForClose());
            
        }
    }
    IEnumerator WaitForClose()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false); // Desabilita o objeto
        Destroy(gameObject);
    }
}


