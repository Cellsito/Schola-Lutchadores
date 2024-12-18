using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    public float playerSpeed = 0.8f;
    public float currentSpeed = 1f;

    public Vector2 playerDirection;

    private bool isWalking;
    private Animator playerAnimator;

    // Player olhando 
    private bool playerFacingRight = true;

    // V�rivel contadora
    private int punchCount;

    // Tempo de ataque
    private float timeCross = 1f;

    private bool comboControl;

    private bool isDead;

    // Propriedades para UI
    public int maxHealth = 10;
    public int currentHealth;
    public Sprite playerImage;

    void Start()
    {
        // Obtem e inicializa as propriedades do RigidBody2D
        playerRigidBody = GetComponent<Rigidbody2D>();

        // Obtem e inicializa as propriedades do Animator
        playerAnimator = GetComponent<Animator>();
        currentSpeed = playerSpeed;

        // Iniciar a vida do Player
        currentHealth = maxHealth;
    }

    void Update()
    {
        PlayerMove();
        UpdateAnimator();


        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.X))
        {
         

            if (punchCount < 2)
            {
                PlayerJab();
                punchCount++;

                if (!comboControl)
                {
                    // Inicia o temporizador
                    StartCoroutine(CrossController());

                }
            }

            else if (punchCount >= 2)
            {
                PlayerCross();
                punchCount = 0;
            }

            // Parando o temporizador
            StopCoroutine(CrossController());
            
        }

    }


    //Fixed Update geralmente � utilizada para implementa��o de f�sica no jogo
    //Por ter uma execu��o padronizada em diferentes dispositivos
    private void FixedUpdate()
    {
        // Verificar se o Player est� em movimento

        if (playerDirection.x != 0 || playerDirection.y != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        playerRigidBody.MovePosition(playerRigidBody.position + currentSpeed * Time.fixedDeltaTime * playerDirection);
    }

    void PlayerMove()
    {
        // Pega a entrada do jogador, e cria um Vector2 para usar no playerDirection
        playerDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Se o player vai para a ESQUERDA e est� olhando para a DIREITA
        if (playerDirection.x < 0 && playerFacingRight)
        {
            Flip();
        }

        // Se o player vai para a DIREITA e est� olhando para a ESQUERDA
        else if (playerDirection.x > 0 && !playerFacingRight)
        {
            Flip();
        }
    }

    void UpdateAnimator()
    {
        // Definir o valor da par�metro do animator, igual � propriedade isWalking
        playerAnimator.SetBool("isWalking", isWalking);
    }

    void Flip()
    {
        // Vai girar o sprite do player em 180� no eixo Y

        // Inverter o valor da vari�vel playerFacingRight
        playerFacingRight = !playerFacingRight;

        // Girar o sprite do player em 180� no eixo Y
        // X, Y, Z
        transform.Rotate(0, 180, 0);
    }

    void PlayerJab()
    {
        //Acessa a anima��o de Jab
        //Ativa o gatilho de ataque Jab
        playerAnimator.SetTrigger("isJab");
    }

    void PlayerCross()
    {
        //Acessa a anima��o de Cross
        //Ativa o gatilho de ataque Cross
        playerAnimator.SetTrigger("isCross");
    }

    IEnumerator CrossController()
    {
        comboControl = true;

        yield return new WaitForSeconds(timeCross);
        punchCount = 0;

        comboControl = false;
    }

    void ZeroSpeed()
    { 
        currentSpeed = 0;
    }

    void ResetSpeed() 
    {
        currentSpeed = playerSpeed;
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            playerAnimator.SetTrigger("HitDamage");
            FindAnyObjectByType<UIManager>().UpdatePlayerHealth(currentHealth);
        }
    }
}
