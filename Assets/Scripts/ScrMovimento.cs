using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D.IK;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class ScrMovimento : MonoBehaviour
{
    private TilemapCollider2D tilemapCollider;
    public Vector2 screenLimite;
    public float moveSpeed = 2f;
    public float moveSpeedAtual = 2f;
    public static bool isJumping = false;
    public Rigidbody2D rb;
    public Collider2D playerCollider;

    public Animator animacao;

    //[SerializeField] Transform target; //para os buracos
    //Vector3 targetPosition;

    public SpriteRenderer playerSpriteRenderer;
    public SpriteRenderer playerShadowRenderer;

    public AnimationCurve jumpCurve;
    public ParticleSystem landingParticleSystem;
    public ParticleSystem barro;

    public AudioSource SfxJumping;
    public AudioSource SfxLanding;

    public bool dialogoPronto;
    public bool dialogoComecar = false;

    private bool jumpLock = false;

    Vector2 movement;

    private void Start()
    {
        moveSpeed = moveSpeedAtual;
        tilemapCollider = GameObject.FindGameObjectWithTag("Obstaculo").GetComponent<TilemapCollider2D>();
        isJumping = false;
    }
    // Update is called once per frame
    void Update()
    {

        //entrada
        if (ScrDialogueManager.isActive == false && ScrTriggerDialogo.dialogoComecar == false && ScrDialogueMComLoad.isActiveLoad == false)
        {
            
            Vector2 move = Application.isMobilePlatform ? InputHandle.joystickAxis : new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement = move;

            if ((Application.isMobilePlatform ? InputHandle.buttonDown[1] : Input.GetButtonDown("Jump")) && !jumpLock) Jump(.2f, 0.0f);
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }

        animacao.SetFloat("Horizontal", movement.x);
        animacao.SetFloat("Vertical", movement.y);
        animacao.SetFloat("speed", movement.sqrMagnitude);

        if (transform.position.x > screenLimite.x) transform.position = new Vector3(screenLimite.x, transform.position.y);
        if (transform.position.x < -screenLimite.x) transform.position = new Vector3(-screenLimite.x, transform.position.y);
        if (transform.position.y > screenLimite.y) transform.position = new Vector3(transform.position.x, screenLimite.y);
        if (transform.position.y < -screenLimite.y) transform.position = new Vector3(transform.position.x, -screenLimite.y);
    }

    private void FixedUpdate()
    {
        //movimento
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Physics2D.IgnoreCollision(playerCollider, tilemapCollider, isJumping);

    }
    public void Jump(float jumpHeightScale, float jumpPushScale)
    {
        if (!isJumping)
        {
            StartCoroutine(JumpCo(jumpHeightScale, jumpPushScale));
        }
    }
    private IEnumerator JumpCo(float jumpHeightScale, float jumpPushScale)
    {
        isJumping = true;
        /*
        var cotoco = Physics2D.OverlapCircle(new Vector3(movement.x, movement.y, 0).normalized * 0.5f + transform.position - Vector3.up * 0.25f, .1f);
        //Debug.Log(cotoco.gameObject.tag);
        //playerCollider.enabled = false; //desabilitar colisão
        if (cotoco != null)
        {
            if (cotoco.gameObject.tag == "Obstaculo")
            {
                //playerCollider.enabled = false; //desabilitar colisão
            }
        }
        */
        rb.AddForce(movement.normalized * jumpPushScale * 10, ForceMode2D.Impulse);

        float jumpStartTime = Time.time;
        float jumpDuration = 1f;

        SfxJumping.Play();

        playerShadowRenderer.transform.localScale = playerShadowRenderer.transform.localScale * 0.55f;

        while (isJumping)
        {
            //Porcentagem 0 - 1.0 de quando está no processo do pulo
            float jumpCompletedPercentage = (Time.time - jumpStartTime) / jumpDuration;
            jumpCompletedPercentage = Mathf.Clamp01(jumpCompletedPercentage);
            animacao.SetBool("pulando", true);
            animacao.SetFloat("Horizontal", movement.x);
            animacao.SetFloat("Vertical", movement.y);
            animacao.SetFloat("speed", movement.sqrMagnitude);


            //Aumenta a escala da imagem para um valor x, com base na original
            playerSpriteRenderer.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;


            playerSpriteRenderer.transform.localScale = playerSpriteRenderer.transform.localScale * 2.00f;


            //Diminui o tamanho da sombra para dar sensação de altura
            playerShadowRenderer.transform.localPosition = new Vector3(-1f, -1.5f, 0.0f) * .5f * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;

            //quanto o pulo está completo
            if (jumpCompletedPercentage == 1.0f)
            {
                break;
            }

            yield return null;
        }
        //checar se onde está pulando é ok

        var obstaculo = (Physics2D.OverlapCircle(transform.position - Vector3.up * 0.2f, .2f)); //============================

        if (obstaculo != null && obstaculo.gameObject.CompareTag("Obstaculo"))
        {

            isJumping = false;
            //faz com que o personagem de outro pulo para atravessar o obstaculo
            Jump(0.2f, 0.6f);

        }
        else
        {
            //quando o perçonagem parar no chão controlar o tamanho
            playerSpriteRenderer.transform.localScale = new Vector3(2, 2, 2);
            //reseta a posição da sombra e o tamanho
            playerShadowRenderer.transform.localPosition = Vector3.zero;//(0, -0.211f,0);
            playerShadowRenderer.transform.localScale = playerSpriteRenderer.transform.localScale / 2;

            //mudar estado
            animacao.SetBool("pulando", false);

            playerCollider.enabled = true;

            if (jumpHeightScale > 0.2f)
            {
                landingParticleSystem.Play();
                SfxLanding.Play();
            }
            landingParticleSystem.Play();


            SfxLanding.Play();
            isJumping = false;
        }
    }
    //detecta a ativação do pulo
    void OnTriggerEnder2D(Collider2D collider2d)
    {
        if (collider2d.CompareTag("Jump"))
        {
            //pega as informações do pulo do scrip de configuração
            ScrJumpConfig jumpData = collider2d.GetComponent<ScrJumpConfig>();
            Jump(jumpData.jumpHeightScale, jumpData.jumpPushScale);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lentidao") == true)
        {
            jumpLock = true;
            moveSpeed -= 0.80f;
            barro.Play();
        }
        if (!isJumping && collision.gameObject.CompareTag("Buraco") == true)
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y);
            ScrScore.instance.RemovePoint(true);
        }
        if (collision.gameObject.CompareTag("Cacto") == true)
        {
            transform.position = new Vector3(transform.position.x - 1.5f, transform.position.y);
            ScrScore.instance.RemovePoint(true);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isJumping && collision.gameObject.CompareTag("Buraco") == true)
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y);
            ScrScore.instance.RemovePoint(true);
        }
        if (collision.gameObject.CompareTag("Cacto") == true)
        {
            transform.position = new Vector3(transform.position.x - 1.5f, transform.position.y);
            ScrScore.instance.RemovePoint(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lentidao") == true)
        {
            moveSpeed = moveSpeedAtual;
            barro.Stop();
            jumpLock = false;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(movement.x, movement.y, 0).normalized * 0.5f + transform.position - Vector3.up * 0.25f, .1f);
    }
}
