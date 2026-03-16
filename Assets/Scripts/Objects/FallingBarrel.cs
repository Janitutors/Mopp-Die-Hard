using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class FallingBarrel : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int hp = 1;

    [Header("Fall")]
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float despawnY = -10f;

    [Header("Ground")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Size")]
    [SerializeField] private Vector3 barrelScale = new Vector3(0.35f, 0.35f, 1f);

    private Rigidbody2D rb;
    private bool isFalling = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        transform.localScale = barrelScale;
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            rb.linearVelocity = new Vector2(0f, -fallSpeed);
        }

        if (transform.position.y < despawnY)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, groundLayer))
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

    private bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }
}