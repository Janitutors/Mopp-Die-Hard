using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AttackHitbox hitbox;

    private void Awake()
    {
        if (hitbox == null)
            hitbox = GetComponentInChildren<AttackHitbox>(true);
    }

    public void DoAttack()
    {
        if (hitbox == null) return;

        hitbox.gameObject.SetActive(true);
        Invoke(nameof(DisableHitbox), 0.1f);
    }

    private void DisableHitbox()
    {
        if (hitbox != null)
            hitbox.gameObject.SetActive(false);
    }
}
