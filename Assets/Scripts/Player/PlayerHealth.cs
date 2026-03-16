using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealth : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private int maxHP = 3;

    [Header("Invulnerability")]
    [SerializeField] private float invulnSeconds = 0.8f;
    [SerializeField] private Color hurtColor = Color.red;

    private int currentHP;
    private bool invulnerable;
    private bool dead;

    private SpriteRenderer sr;
    private Color originalColor;
    private PlayerController controller;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;
    public bool IsDead => dead;

    public static event Action<int, int> OnHealthChanged;
    public static event Action OnPlayerDied;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
        originalColor = sr.color;
        currentHP = maxHP;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(int damage)
    {
        if (dead) return;
        if (invulnerable) return;

        currentHP -= damage;
        currentHP = Mathf.Max(0, currentHP);

        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvulnerabilityRoutine());
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        invulnerable = true;
        sr.color = hurtColor;

        yield return new WaitForSeconds(invulnSeconds);

        sr.color = originalColor;
        invulnerable = false;
    }

    private void Die()
    {
        dead = true;

        if (controller != null)
            controller.StopMovement();

        OnPlayerDied?.Invoke();
    }
}