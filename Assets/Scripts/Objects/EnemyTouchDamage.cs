using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    public int damage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        var hp = collision.gameObject.GetComponent<PlayerHealth>();
        if (hp == null)
            hp = collision.gameObject.GetComponentInParent<PlayerHealth>();

        if (hp != null)
            hp.TakeDamage(damage);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        var hp = collision.gameObject.GetComponent<PlayerHealth>();
        if (hp == null)
            hp = collision.gameObject.GetComponentInParent<PlayerHealth>();

        if (hp != null)
            hp.TakeDamage(damage);
    }
}