using UnityEngine;

public class EnemyKillPlayer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            Debug.Log("GAME OVER");
        }
    }
}