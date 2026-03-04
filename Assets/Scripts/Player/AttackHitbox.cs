using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private int damage = 1;

    // PlayerAttack calls this when it wants to set the damage
    public void SetDamage(int dmg) => damage = dmg;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // does it hit a FallingObject?
        FallingObject fo = other.GetComponent<FallingObject>();
        if (fo != null)
        {
            fo.TakeDamage(damage);
        }
    }
}
