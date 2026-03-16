using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;

    private bool gameOver;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDied += ShowGameOver;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= ShowGameOver;
    }

    private void Start()
    {
        gameOver = false;
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (!gameOver) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    private void ShowGameOver()
    {
        gameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}