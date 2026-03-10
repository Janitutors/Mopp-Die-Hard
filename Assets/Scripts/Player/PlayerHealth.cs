using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
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

        // TODO: change player color/alpha or something to show invulnerability,
        // maybe spawn some particles or something when hit
        // Debug.Log($"Player HP: {currentHP}");

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
        // TODO: death animation, sound effect, particles, etc
        Debug.Log("Player died");

        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
        
    }
}