using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, GameOver }

    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; } = GameState.Playing;

    [Header("References")]
    [SerializeField] private MonoBehaviour[] disableOnGameOver; // esim PlayerMovement, MopAttack
    [SerializeField] private MonoBehaviour spawner;            // toteutetaan Day 3
    [SerializeField] private GameObject gameOverUI;            // UI-paneeli

    private void Awake()
    {
        // Estää duplikaatit jos scene reloadaa
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public void StartRun()
    {
        State = GameState.Playing;

        if (spawner != null)
            spawner.enabled = true;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        foreach (var behaviour in disableOnGameOver)
        {
            if (behaviour != null)
                behaviour.enabled = true;
        }
    }

    public void GameOver()
    {
        if (State == GameState.GameOver)
            return;

        State = GameState.GameOver;

        if (spawner != null)
            spawner.enabled = false;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        foreach (var behaviour in disableOnGameOver)
        {
            if (behaviour != null)
                behaviour.enabled = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}