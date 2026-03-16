using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AlienChase : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private string playerTag = "Player";

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2.2f;
    [SerializeField] private float stopDistance = 0.15f;

    [Header("Optional")]
    [SerializeField] private bool faceMovementDirection = false;

    private Rigidbody2D rb;
    private Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        FindPlayer();
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            FindPlayer();
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 currentPos = rb.position;
        Vector2 targetPos = target.position;
        Vector2 delta = targetPos - currentPos;

        float distance = delta.magnitude;

        if (distance <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = delta.normalized;
        rb.linearVelocity = dir * moveSpeed;

        if (faceMovementDirection && Mathf.Abs(dir.x) > 0.01f)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (dir.x < 0 ? -1f : 1f);
            transform.localScale = scale;
        }
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            target = playerObj.transform;
    }
}