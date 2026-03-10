using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, GameOver }

    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; } = GameState.Playing;

    [Header("References")]
    [SerializeField] private MonoBehaviour[] disableOnGameOver; // e.g. PlayerController, PlayerAttack, PlayerInput
    [SerializeField] private Spawner spawner;            // done Day 3
    [SerializeField] private GameObject gameOverUI;            // UI-panel

    private void Awake()
    {
        // blocks duplications if scene reload
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    private void Start()
    {
        StartRun();   // auto start for development
    }

    private void Update()
    {
        if (State == GameState.GameOver && Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }


    

    public void StartRun()
    {
        State = GameState.Playing;
        
        ScoreManager.ResetScore();

        if (spawner != null)
            spawner.StartSpawning();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        foreach (var behaviour in disableOnGameOver)
         if (behaviour != null) behaviour.enabled = true;
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
          if (behaviour != null) behaviour.enabled = false;
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}