using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 1;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        FallingObject falling = other.GetComponent<FallingObject>();
        if (falling == null) return;

        falling.TakeDamage(damage);

        Vector2 dir = (other.transform.position - transform.position).normalized;
        if (dir.sqrMagnitude < 0.001f) dir = Vector2.right;

        falling.Knockback(dir * knockbackForce);
    }

    private void OnDrawGizmosSelected()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(col.bounds.center, col.bounds.size);
    }
}