using UnityEngine;

public class AlienHealth : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private int maxHP = 1;

    [Header("Death")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitFlashTime = 0.08f;

    private int currentHP;
    private SpriteRenderer sr;
    private Color originalColor;
    private float hitTimer;

    private void Awake()
    {
        currentHP = maxHP;
        sr = GetComponent<SpriteRenderer>();

        if (sr != null)
            originalColor = sr.color;
    }

    private void Update()
    {
        if (hitTimer > 0f)
        {
            hitTimer -= Time.deltaTime;

            if (hitTimer <= 0f && sr != null)
                sr.color = originalColor;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHP <= 0) return;

        currentHP -= damage;

        if (sr != null)
        {
            sr.color = hitColor;
            hitTimer = hitFlashTime;
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void SetHP(int value)
    {
        maxHP = Mathf.Max(1, value);
        currentHP = maxHP;
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }
}