using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int Score { get; private set; }

    public static void ResetScore() => Score = 0;

    public static void AddScore(int amount)
    {
        Score += amount;
        // later UI update
        Debug.Log($"Score: {Score}");
    }
}
