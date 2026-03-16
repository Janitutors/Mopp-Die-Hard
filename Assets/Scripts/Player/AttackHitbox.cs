using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        FallingBarrel barrel = other.GetComponent<FallingBarrel>();
        if (barrel != null)
        {
            barrel.TakeDamage(damage);
            return;
        }

        AlienHealth alien = other.GetComponent<AlienHealth>();
        if (alien != null)
        {
            alien.TakeDamage(damage);
        }
    }
}