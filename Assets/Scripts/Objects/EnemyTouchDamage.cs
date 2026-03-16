using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private float lastHitTime;
    private float hitCooldown = 0.8f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryDamage(other);
    }

    void TryDamage(Collider2D other)
    {
        if (Time.time - lastHitTime < hitCooldown) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player == null)
            player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
            lastHitTime = Time.time;
        }
    }
}