using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float activeTime = 0.12f;

    [Header("References")]
    [SerializeField] private Collider2D hitboxCollider;

    private bool isAttacking;

    private void Awake()
    {
        if (hitboxCollider == null)
        {
            AttackHitbox hitbox = GetComponentInChildren<AttackHitbox>(true);
            if (hitbox != null)
                hitboxCollider = hitbox.GetComponent<Collider2D>();
        }

        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            DoAttack();
        }
    }

    public void DoAttack()
    {
        if (isAttacking) return;
        if (hitboxCollider == null) return;

        isAttacking = true;
        hitboxCollider.enabled = true;

        Invoke(nameof(DisableHitbox), activeTime);
    }

    private void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;

        isAttacking = false;
    }
}