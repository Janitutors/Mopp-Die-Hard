using UnityEngine;

public class KeepInBounds2D : MonoBehaviour
{
    [Header("World bounds")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;
    public float maxY = 4.5f;

    Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        Vector2 p = rb.position;

        // clamp
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.y = Mathf.Clamp(p.y, minY, maxY);

        rb.position = p;
    }
}