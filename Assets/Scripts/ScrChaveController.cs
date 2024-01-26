using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrChaveController : MonoBehaviour
{
    private CircleCollider2D boxCollider;
    public int enemycounter;

    private void Start()
    {
        enemycounter = 0;
        boxCollider = GetComponent<CircleCollider2D>();
        boxCollider.enabled = false;
    }
    private void Update()
    {
        if(enemycounter == 3)
        {
            AtivarColisao(true);
        }
    }

    public void AtivarColisao(bool ativar)
    {
        boxCollider.enabled = ativar;
    }
    
    public void IncreaseCounter()
    {
        enemycounter++;
    }
}
