using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingObject : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int hp = 1;

    [Header("Movement")]
    [SerializeField] private float fallSpeed = 3f;   
    [SerializeField] private float despawnY = -10f;

    [Header("Ground")]
    [SerializeField] private string rooftopName = "Rooftop"; 

    private Rigidbody2D rb;
    private bool isFalling = true;

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

        if (transform.position.y < despawnY)
        {
            Destroy(gameObject);
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
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Knockback(Vector2 force)
    {
        isFalling = false;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}