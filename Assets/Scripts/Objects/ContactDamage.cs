using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var receiver = collision.gameObject.GetComponent<PlayerDamageReceiver>();
            if (receiver != null)
            {
                receiver.TakeDamage(damage);
            }

            // Week 1:  barrel destroys itself on contact with player, later we can add some visual feedback and maybe sound
            Destroy(gameObject);
        }
    }
}
