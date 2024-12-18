using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player)
        {
            player.TakeDamage(damage);

            Destroy(this.gameObject);    
        }

        if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

}
