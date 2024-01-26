using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D.IK;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScrEnemy : MonoBehaviour
{
    public bool altMovement = false;
    private TilemapCollider2D tilemapCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public GameObject exclamation;

    private float speed = 0;
    public float minSpeed = 1.00f;
    public float maxSpeed = 1.60f;
    public float angulo = 0.20f;
    public float escapePerecentage = 0.25f;
    public Vector2 screenLimite = new Vector2(71, 1f);
    private Vector2 position;
    public int cageX = 0;

    float randomValue = 0;//chance de ele escapar;
    private bool isJumping = false;
    public SpriteRenderer enemySpriteRenderer;
    public Collider2D enemyCollider;
    public AnimationCurve jumpCurve;
    public Animator animacao;
    public Rigidbody2D rb;
    public ParticleSystem landingParticleSystem;
    public ParticleSystem escape;
    public AudioSource SfxJumping;
    public AudioSource SfxLanding;
    public AudioSource SfxSmoke;
    public AudioSource SfxBush;
    public AudioSource SfxAttention;

    private bool switchGrass = false;
    
    private bool attention = false;
    bool escapou = false;
    private bool preso = false;
    Vector2 movement;
    bool goingRight = true; // Movimento para a direita
    bool goingUp = true; // Movimento para cima


    // Start is called before the first frame update
    void Start()
    {
        position = GetRigidbody2DPosition(rb);
        movement.x = 0;
        movement.y = 0;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        int seed = System.Environment.TickCount;
        Random.InitState(seed);
        tilemapCollider = GameObject.FindGameObjectWithTag("Obstaculo").GetComponent<TilemapCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Physics2D.IgnoreCollision(enemyCollider, tilemapCollider, isJumping);
    }

    void Morrer()
    {
        transform.position = new Vector2(screenLimite.x, transform.position.y);
    }
    void Move()
    {
        if (altMovement)
        {
            AlternativeMovement();
        }
        if (!escapou && !altMovement)
        {
            animacao.SetFloat("Horizontal", movement.x);
            animacao.SetFloat("Vertical", movement.y);
            animacao.SetFloat("speed", movement.sqrMagnitude);
            if (speed != 0)
            {
                if (goingRight)
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                    movement.x = 1;
                }
                else
                {
                    movement.x = -1;
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }

                // Movimento no eixo Y
                if (goingUp)
                {
                    movement.y = 1;
                    transform.Translate(Vector3.up * (speed - angulo) * Time.deltaTime);
                }
                else
                {
                    movement.y = -1;
                    transform.Translate(Vector3.down * (speed - angulo) * Time.deltaTime);
                }

                // Verificar limites no eixo Y
                if (transform.position.y >= screenLimite.y)
                {
                    randomValue = Random.value;
                    if (randomValue <= escapePerecentage)
                    {
                        escapou = true;
                        speed = 0;
                        escape.Play();
                        enemyCollider.enabled = false;
                        animator.enabled = false;
                        spriteRenderer.enabled = false;
                        SfxBush.Play();
                        StartCoroutine(EnemyEscape());

                    }
                    goingUp = false;
                    movement.y = -1;
                }
                else if (transform.position.y <= -screenLimite.y)
                {
                    randomValue = Random.value;
                    if (randomValue <= escapePerecentage)
                    {
                        escapou = true;
                        speed = 0;
                        escape.Play();
                        enemyCollider.enabled = false;
                        animator.enabled = false;
                        spriteRenderer.enabled = false;
                        SfxBush.Play();
                        StartCoroutine(EnemyEscape());

                    }
                    goingUp = true;
                    movement.y = 1;
                }

                // Verificar limite no eixo X
                if (transform.position.x >= screenLimite.x)
                {
                    goingRight = false;
                }
                else if (transform.position.x <= -screenLimite.x)
                {
                    goingRight = true;
                }
            }
        }
        
    }

    public void AlternativeMovement()
    {
        if (!escapou)
        {
            // Obtém a posição atual do Rigidbody2D
            

            animacao.SetFloat("Horizontal", movement.x);
            animacao.SetFloat("Vertical", movement.y);
            animacao.SetFloat("speed", movement.sqrMagnitude);
            if (speed != 0)
            {
                if (goingRight)
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                    movement.x = 1;
                }
                else
                {
                    movement.x = -1;
                    transform.Translate(Vector3.left * speed * Time.deltaTime);
                }

                // Movimento no eixo Y
                if (goingUp)
                {
                    movement.y = 1;
                    transform.Translate(Vector3.up * (speed - angulo) * Time.deltaTime);
                }
                else
                {
                    movement.y = -1;
                    transform.Translate(Vector3.down * (speed - angulo) * Time.deltaTime);
                }

                // Verificar limites no eixo Y
                if (transform.position.y >= screenLimite.y)
                {
                    randomValue = Random.value;
                    if (randomValue <= escapePerecentage)
                    {
                        BounceOFF();
                    }
                    else
                    {
                        BounceON();
                    }
                    goingUp = false;
                    movement.y = -1;
                }
                else if (transform.position.y <= -screenLimite.y)
                {
                    randomValue = Random.value;
                    if (randomValue <= escapePerecentage)
                    {
                        BounceOFF();
                    }
                    else
                    {
                        BounceON();
                    }
                    goingUp = true;
                    movement.y = 1;
                }

                // Verificar limite no eixo X
                if (transform.position.x >= (position.x + cageX)) // if (transform.position.x >= screenLimite.x)
                {
                    goingRight = false;
                }
                else if (transform.position.x <= (position.x - cageX)) // else if (transform.position.x <= -screenLimite.x)
                {
                    goingRight = true;
                }
            }
        }
    }

    private void BounceOFF()
    {
        switchGrass = true;
        StartCoroutine(GrassRoutine());
        //escape.Play();
        enemyCollider.enabled = false;
        animator.enabled = false;
        spriteRenderer.enabled = false;
        SfxBush.Play();
    }
    private void BounceON()
    {
        if (switchGrass)
        {
            StartCoroutine(GrassRoutine());
            //escape.Play();
            SfxBush.Play();
        }
        switchGrass = false;
        enemyCollider.enabled = true;
        animator.enabled = true;
        spriteRenderer.enabled = true;
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
        //enemyCollider.enabled = false;
        rb.AddForce(rb.velocity.normalized * jumpPushScale * 10, ForceMode2D.Impulse);

        float jumpStartTime = Time.time;
        float jumpDuration = 1f;

        SfxJumping.Play();

        //playerShadowRenderer.transform.localScale = playerShadowRenderer.transform.localScale * 0.55f;

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
            enemySpriteRenderer.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;


            enemySpriteRenderer.transform.localScale = enemySpriteRenderer.transform.localScale * 2.00f;


            //Diminui o tamanho da sombra para dar sensação de altura
            //playerShadowRenderer.transform.localPosition = new Vector3(-1f, -1.5f, 0.0f) * .5f * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;

            //quanto o pulo está completo
            if (jumpCompletedPercentage == 1.0f)
            {
                break;
            }

            yield return null;
        }
        //checar se onde está pulando é ok

        var obstaculo = (Physics2D.OverlapCircle(transform.position, .2f));
        if (!preso && obstaculo != null && obstaculo.gameObject.tag == "Obstaculo")
        {

            isJumping = false;
            //faz com que o personagem de outro pulo para atravessar o obstaculo
            Jump(0.2f, 0.6f);

        }
        else
        {
            //quando o perçonagem parar no chão controlar o tamanho
            enemySpriteRenderer.transform.localScale = new Vector3(2, 2, 2);
            //reseta a posição da sombra e o tamanho
            //playerShadowRenderer.transform.localPosition = Vector3.zero;//(0, -0.211f,0);
            //playerShadowRenderer.transform.localScale = playerSpriteRenderer.transform.localScale / 2;

            //mudar estado
            animacao.SetBool("pulando", false);
            //enemyCollider.enabled = true;
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


    void OnTriggerEnder2D(Collider2D collider2d)
    {
        if (collider2d.CompareTag("Jump"))
        {
            //pega as informações do pulo do scrip de configuração
            ScrJumpConfig jumpData = collider2d.GetComponent<ScrJumpConfig>();
            Jump(jumpData.jumpHeightScale, jumpData.jumpPushScale);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstaculo"))
        {
            Jump(.2f, 0.0f);
        }
        if (collision.gameObject.CompareTag("Buraco"))
        {
            Jump(.2f, 0.0f);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            preso = true;
            isJumping = false;
            animacao.SetBool("pulando", false);
            animacao.SetBool("escapou", true);
            Debug.Log("Capturou");
            ScrScore.instance.PrepareScore(8);
            ScrScore.instance.AddPoint(collision.transform.position, 8);
            CapturadoEfx();
        }

    }
    IEnumerator EnemyEscape()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player") == true)
        {
            speed = Random.Range(minSpeed, maxSpeed);
            if (!attention)
            {

                attention = true;
                SfxAttention.Play();
                exclamation.SetActive(true);
                StartCoroutine(CloseExclamation());
            }
        }
       
    }
    private void CapturadoEfx()
    {
        Vector3 newScale = new Vector3(4f, 4f, 4f); // Por exemplo, duplicar a escala atual
        // Aplica a nova escala ao personagem
        transform.localScale = newScale;
        
        speed = 0;
        enemyCollider.enabled = false;
        SfxSmoke.Play();
        StartCoroutine(EnemyEscape());
    }
    IEnumerator CloseExclamation()
    {
        yield return new WaitForSeconds(1.5f);
        exclamation.SetActive(false);
    }
    private IEnumerator GrassRoutine()
    {
        escape.Play();
        yield return new WaitForSeconds(0.2f);
        escape.Stop();
    }

    public Vector2 GetRigidbody2DPosition(Rigidbody2D rigidbody)
    {
        return rigidbody.position;
    }
}