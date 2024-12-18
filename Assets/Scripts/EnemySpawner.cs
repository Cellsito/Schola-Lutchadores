using Assets.Scripts;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] enemyArray;

    public int numberOfEnemies;
    private int currentEnemies;

    public float spawnTime;

    public string nextSection;


    // Update is called once per frame
    void Update()
    {
        if(currentEnemies >= numberOfEnemies)
        {
            int enemies = FindObjectsByType<EnemyMeleeController>(FindObjectsSortMode.None).Length;

            if (enemies <= 0)
            {
                LevelManager.ChangeSection(nextSection);

                this.gameObject.SetActive(false);
            }
        }
    }

    void SpawnEnemy ()
    {
        // Posi�ao do Spawn do inimigo
        Vector2 spawnPosition;

        // Limites Y
        // -0,35
        // -0,95
        spawnPosition.y = Random.Range(-0.95f, -0.35f);

        // Posi��o X m�ximo (direita) do confiner da camera +1 de distancia
        // Pegar RightBound (limite direito) da Section (Confiner) como base
        float rightSectionBound = LevelManager.currentConfiner.BoundingShape2D.bounds.max.x;

        // Define o X do spawnPosition, igual ao ponto da DIREITA do confiner
        spawnPosition.x = rightSectionBound;

        // Instancia os Inimigos
        // Pega um inimigo aleat�rio da lista de inimigos
        // Spawna na posi��o spawnPosition
        // Quaternion � uma classe utilizada para trabalhar com rota��es
        Instantiate(enemyArray[Random.Range(0, enemyArray.Length)], spawnPosition, Quaternion.identity).SetActive(true);

        currentEnemies++;

        // Se o numero de inimigos atualmente na cena for menor que o numero maximo de inimigos,
        // invoca novamente a fun��o de spawn
        if (currentEnemies < numberOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player)
        {
            this.GetComponent<BoxCollider2D>().enabled = false;

            SpawnEnemy();
        }
    }
}
