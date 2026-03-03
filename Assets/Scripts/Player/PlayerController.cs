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

    // Send Messages -moodille varma allekirjoitus
    public void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();

        if (inputVector.sqrMagnitude > 1f)
            inputVector = inputVector.normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = inputVector * moveSpeed;
    }
}