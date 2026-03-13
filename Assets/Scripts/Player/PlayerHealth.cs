using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static Action OnPlayerDeath;

    [Header("HP")]
    public int maxHP = 3;
    public int currentHP;

    [Header("I-frames")]
    public float invulnSeconds = 0.5f;

    bool invulnerable;

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        if (invulnerable || amount <= 0) return;

        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;

        if (currentHP == 0)
        {
            Die();
            return;
        }

        StartCoroutine(Invuln());
    }

    IEnumerator Invuln()
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnSeconds);
        invulnerable = false;
    }

    void Die()
    {
        Debug.Log("Player died");

        OnPlayerDeath?.Invoke();   // NEW EVENT

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }
}