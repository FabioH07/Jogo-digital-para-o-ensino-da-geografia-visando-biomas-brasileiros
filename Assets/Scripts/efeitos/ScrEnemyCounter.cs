using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrEnemyCounter : MonoBehaviour
{
    public ScrChaveController controller;
    private bool a = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true && !a)
        {
            a = true;
            controller.IncreaseCounter();
        }
    }
}
