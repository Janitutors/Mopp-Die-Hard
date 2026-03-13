using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingObject : MonoBehaviour
{
    public static Action<FallingObject> OnEnemyHit;
    public static Action<FallingObject> OnEnemyDeath;
    public static Action<FallingObject> OnBigEnemyPushedOut;

    [Header("Stats")]
    [SerializeField] private int hp = 1;
    [SerializeField] private int scoreValue = 10;

    [Header("Movement")]
    [SerializeField] private float fallSpeed = 3f;

    [Header("Despawn Bounds")]
    [SerializeField] private float despawnTopY = 10f;
    [SerializeField] private float despawnLeftX = -12f;
    [SerializeField] private float despawnRightX = 12f;

    [Header("Ground")]
    [SerializeField] private string rooftopName = "Rooftop";

    private Rigidbody2D rb;
    private bool isFalling = true;
    private bool isDead = false;

    // NEW
    private bool isBigEnemy = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -fallSpeed);
        }

        // If enemy gets knocked out of the arena
        if (transform.position.y > despawnTopY ||
            transform.position.x < despawnLeftX ||
            transform.position.x > despawnRightX)
        {
            HandlePushedOut();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == rooftopName)
        {
            isFalling = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // NEW
        OnEnemyHit?.Invoke(this);

        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    public void SetHP(int value)
    {
        hp = value;
    }

    public void SetScoreValue(int value)
    {
        scoreValue = value;
    }

    public void MultiplyFallSpeed(float multiplier)
    {
        fallSpeed *= multiplier;
    }

    // NEW
    public void SetBigEnemy(bool value)
    {
        isBigEnemy = value;
    }

    public void Knockback(Vector2 force)
    {
        if (isDead) return;

        isFalling = false;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    // NEW
    private void HandlePushedOut()
    {
        if (isDead) return;

        if (isBigEnemy)
            OnBigEnemyPushedOut?.Invoke(this);

        Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // NEW
        OnEnemyDeath?.Invoke(this);

        ScoreManager.AddScore(scoreValue);
        Destroy(gameObject);
    }
}