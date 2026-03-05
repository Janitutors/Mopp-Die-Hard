using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        var hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
        }
    }

    // if the player stays in the trigger, they should take damage repeatedly,
    // but only once per invulnerability period (i-frames)
    void OnTriggerStay2D(Collider2D other)
    {
        var hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(damage); // will only actually do damage if player isn't currently invulnerable,
                                   // so it's safe to call every frame
        }
    }
}