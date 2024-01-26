using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScrSombra : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    public Animator animacao;

    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer playerShadowRenderer;

    public AnimationCurve jumpCurve;
    ///bool isJumping = false;
    Vector2 movement;

    // Update is called once per frame
    void Update()
    { 
        if (ScrDialogueManager.isActive == false && ScrTriggerDialogo.dialogoComecar == false && ScrDialogueMComLoad.isActiveLoad == false)
        {
            Vector2 move = Application.isMobilePlatform ? InputHandle.joystickAxis : new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement = move;
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }

        if (ScrMovimento.isJumping == true) animacao.SetBool("pulando", true);
        if (ScrMovimento.isJumping == false) animacao.SetBool("pulando", false);
        animacao.SetFloat("Horizontal", movement.x);
        animacao.SetFloat("Vertical", movement.y);
        animacao.SetFloat("speed", movement.sqrMagnitude);
    }
}

