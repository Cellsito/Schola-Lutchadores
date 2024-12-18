using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    public Slider playerHealthBar;
    public Image playerImage;

    public GameObject enemyUI;
    public Slider enemyHealtBar;
    public Image enemyImage;

    // Objeto para armazenar os dados do Player
    private PlayerController player;

    // Timers e controles do enemyUI
    [SerializeField] private float enemyUITime = 4f;
    private float enemyTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Obtem os dados do Player
        player = FindAnyObjectByType<PlayerController>();

        // Definir o valor m�ximo da barra de vida igual ao m�ximo da vida do Player
        playerHealthBar.maxValue = player.maxHealth;

        // Inicia a HealthBar cheia
        playerHealthBar.value = playerHealthBar.maxValue;

        // Definir imagem do player
        playerImage.sprite = player.playerImage;
    }

    // Update is called once per frame
    void Update()
    {
        // Inicia o contador para controlar o tempo de exibi��o da enemyUI
        enemyTimer += Time.deltaTime;

        // Se o tempo limite for atingindo, oculta a UI e reseta o timer
        if (enemyTimer >= enemyUITime)
        {
            enemyUI.SetActive(false);
            enemyTimer = 0;
        }
    }

    public void UpdatePlayerHealth(int amount)
    {
        playerHealthBar.value = amount;
    }

    public void UpdateEnemyUI(int maxHealth, int currentHealth, Sprite image)
    {
        // Atualiza os dados do inimigo de acordo com o inimigo atacado
        enemyHealtBar.maxValue = maxHealth;
        enemyHealtBar.value = currentHealth;
        enemyImage.sprite = image;

        // Zera o timer para come�ar a contar 4 segindos
        enemyTimer = 0;

        // Habilita a enemyUI, deixando-a vis�vel
        enemyUI.SetActive(true);
    }
}
