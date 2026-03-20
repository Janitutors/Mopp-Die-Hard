using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 inputVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    public void OnMove(InputValue value)
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying)
            return;

        inputVector = value.Get<Vector2>();

        if (inputVector.sqrMagnitude > 1f)
            inputVector = inputVector.normalized;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying)
        {
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = inputVector * moveSpeed;
    }

    public void StopMovement()
    {
        inputVector = Vector2.zero;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }
    }
}