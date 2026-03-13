using UnityEngine;
using System; 

public class PlayerAttack : MonoBehaviour
{
    public static Action OnPlayerAttack;

    [Header("Timing")]
    [SerializeField] private float activeTime = 0.12f;

    [Header("References")]
    [SerializeField] private Collider2D hitboxCollider;

    private bool isAttacking;

    private void Awake()
    {
        if (hitboxCollider == null)
            hitboxCollider = GetComponentInChildren<Collider2D>(true);

        if (hitboxCollider != null)
            hitboxCollider.enabled = false;
    }

    
    public void OnAttack()
    {
        DoAttack();
    }

    public void DoAttack()
    {
        if (isAttacking) return;
        if (hitboxCollider == null) return;

        isAttacking = true;
        OnPlayerAttack?.Invoke();
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