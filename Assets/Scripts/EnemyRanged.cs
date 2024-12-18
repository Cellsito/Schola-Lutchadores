using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    // Variavel que indica se o inimigo está vivo
    public bool isDead;

    // Variaveis para controlar o lado que o inimigo está virado
    public bool facingRight;
    public bool previousDirectionRight;

    // Variavel para armazenar posição do Player
    private Transform target;

    // Variaveis para movimentação do inimigo
    private float enemySpeed = 0.5f;
    private float currentSpeed;

    private bool isWalking;

    private float horizontalForce;
    private float verticalForce;

    // Variavel que vamos usar para controlar o intervalo de tempo que o inimigo ficará andando vertical
    // Isso vai ajudar à dar uma aleatoriedade ao movimento do inimigo
    private float walkTimer;

    // Variáveis para mecânica de ataque
    private float attackRate = 1f;
    private float nextAttack;

    // Variaveis para mecânica de dano
    public int maxHealth = 4;
    public int currentHealth;
    public Sprite enemyImage;

    public float staggerTime = 0.5f;
    private float damageTimer;
    public bool isTakingDamage;

    public GameObject projectile;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Buscar o Player e armazenar sua posição
        target = FindAnyObjectByType<PlayerController>().transform;

        // Incializar a velocidade do inimigo
        currentSpeed = enemySpeed;

        currentHealth = maxHealth;
    }

    void Update()
    {
        // Verificar se o Player está para a Direita ou para a Esquerda
        // E com isso determinar o lado que o Inimigo ficará virado
        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        // Se facingRight for TRUE, vamos virar o inimigo em 180º no eixo Y,
        // Senão vamos virar o inimigo para a esquerda

        // Se o Player à direita e a direção anterior NÃO era direita (inimigo olhando para esquerda)
        if (facingRight && !previousDirectionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        // Se o Player NÃO está à direita e a direção anterior ERA direita (inimigo olhando para direita)
        if (!facingRight && previousDirectionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionRight = false;
        }

        // Iniciar o timer do caminhar do inimigo
        walkTimer += Time.deltaTime;

        // Gerenciar a animação do inimigo
        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        // Gereciar o tempo de stagger
        if (isTakingDamage && !isDead)
        {
            damageTimer += Time.deltaTime;

            ZeroSpeed();

            if (damageTimer >= staggerTime)
            {
                isTakingDamage = false;
                damageTimer = 0;

                ResetSpeed();
            }
        }

        // Atualiza o animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {

        if (!isDead)
        {
            // MOVIMENTAÇÃO

            // Variavel para armazenar a distancia entre o Inimigo e o Player
            Vector3 targetDistance = target.position - this.transform.position;

            if (walkTimer >= Random.Range(2.5f, 3.5f))
            {
                verticalForce = targetDistance.y / Mathf.Abs(targetDistance.y);
                horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

                walkTimer = 0;
            }

            if (Mathf.Abs(targetDistance.x) < 1f)
            {
                horizontalForce = 0;
            }

            if (Mathf.Abs(targetDistance.y) < 0.05f)
            {
                verticalForce = 0;
            }

            if (!isTakingDamage)
            {
                rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);
            }

            // Ataque Ranged
            if (Mathf.Abs(targetDistance.x) < 1.3f && Mathf.Abs(targetDistance.y) < 0.05 && Time.time > nextAttack)
            {
                animator.SetTrigger("Attack");
                ZeroSpeed();

                nextAttack = Time.time + attackRate;
            }

        }

    }

    void UpdateAnimator()
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            isTakingDamage = true;

            currentHealth -= damage;

            animator.SetTrigger("HitDamage");

            // Atualiza a UI no Inimigo
            FindAnyObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

            if (currentHealth <= 0)
            {
                isDead = true;

                rb.linearVelocity = Vector2.zero;

                animator.SetTrigger("Dead");
            }

        }
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = enemySpeed;
    }

    public void DisableEnemy()
    {
        this.gameObject.SetActive(false);
    }

    public void Shoot()
    {
        Vector2 spawnPosition = new Vector2(this.transform.position.x, this.transform.position.y + 0.2f);

        GameObject shotObject = Instantiate(projectile, spawnPosition, Quaternion.identity);

        shotObject.SetActive(true);

        var shotPhysics = shotObject.GetComponent<Rigidbody2D>();

        if (facingRight)
        {
            shotPhysics.AddForceX(80f);
        }
        else
        {
            shotPhysics.AddForceX(-80f);
        }
    }
}