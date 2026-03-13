using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        StartScreen,
        Playing,
        GameOver
    }

    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; } = GameState.StartScreen;

    public bool IsPlaying => State == GameState.Playing;
    public bool IsGameOver => State == GameState.GameOver;
    public bool IsOnStartScreen => State == GameState.StartScreen;

    public float SurvivalTime { get; private set; }

    [Header("References")]
    [SerializeField] private MonoBehaviour[] disableOnGameOver;
    [SerializeField] private Spawner spawner;
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject playerRoot;
    [SerializeField] private GameObject scoreUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        ShowStartScreen();
    }

    private void Update()
    {
        if (State == GameState.Playing)
        {
            SurvivalTime += Time.deltaTime;
        }

        if (State == GameState.GameOver && Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    private void ShowStartScreen()
    {
        State = GameState.StartScreen;
        SurvivalTime = 0f;

        ScoreManager.ResetScore();

        if (spawner != null)
            spawner.StopSpawning();

        if (startUI != null)
            startUI.SetActive(true);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
        
        if (playerRoot != null)
            playerRoot.SetActive(false);

        if (scoreUI != null)
            scoreUI.SetActive(false);

        foreach (var behaviour in disableOnGameOver)
        {
            if (behaviour != null)
                behaviour.enabled = false;
        }
    }

    public void PlayGame()
    {
        StartRun();
    }

    public void StartRun()
    {
        State = GameState.Playing;
        SurvivalTime = 0f;

        ScoreManager.ResetScore();

        if (startUI != null)
            startUI.SetActive(false);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (playerRoot != null)
            playerRoot.SetActive(true);

        if (scoreUI != null)
            scoreUI.SetActive(true);
            
        foreach (var behaviour in disableOnGameOver)
        {
            if (behaviour != null)
                behaviour.enabled = true;
        }

        if (spawner != null)
            spawner.StartSpawning();
    }

    public void GameOver()
    {
        if (State == GameState.GameOver)
            return;

        State = GameState.GameOver;

        if (spawner != null)
            spawner.StopSpawning();

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