using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float activeTime = 0.12f;
    [SerializeField] private float cooldown = 0.35f;

    [Header("Damage")]
    [SerializeField] private int damage = 1;

    [Header("References")]
    [SerializeField] private Collider2D hitboxCollider; // AttackHitbox collider
    [SerializeField] private AttackHitbox hitboxScript; // AttackHitbox.cs

    private bool onCooldown;

    private void Awake()
    {
        // security null checks
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;

        if (hitboxScript != null)
            hitboxScript.SetDamage(damage);
    }

    private void Update()
    {
        // Input System: Space
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (onCooldown) return;
        if (hitboxCollider == null || hitboxScript == null) return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        onCooldown = true;

        hitboxScript.SetDamage(damage);
        hitboxCollider.enabled = true;

        yield return new WaitForSeconds(activeTime);

        hitboxCollider.enabled = false;

        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}
