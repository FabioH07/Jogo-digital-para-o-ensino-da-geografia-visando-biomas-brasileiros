using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrChave : MonoBehaviour
{
    bool key = false;
    public ScrChaveController controller;

    private void Start()
    {
        //controller.AtivarColisao(key);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
        {
            key = true;
            controller.AtivarColisao(key);
        }
    }

}
