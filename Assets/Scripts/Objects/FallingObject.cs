using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int hp = 1;
    [SerializeField] private int scoreValue = 10;

    [Header("Movement")]
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float despawnY = -10f;

    private void Update()
    {
        // Predictable constant downward movement
        transform.position += Vector3.down * (fallSpeed * Time.deltaTime);

        if (transform.position.y < despawnY)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
            Die();
    }

    protected virtual void Die()
    {
        ScoreManager.AddScore(scoreValue);
        Destroy(gameObject);
    }
}
