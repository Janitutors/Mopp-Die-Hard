using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private FallingObject fallingPrefab;

    [Header("Arena")]
    [SerializeField] private BoxCollider2D arena;

    [Header("Spawn Area")]
    [SerializeField] private float spawnY = 6f;
    [SerializeField] private float spawnPadding = 0.6f;

    [Header("Difficulty / Pace")]
    [Tooltip("Initial spawn interval between waves")]
    [SerializeField] private float startInterval = 1.5f;

    [Tooltip("Minimum interval (game will not get faster than this)")]
    [SerializeField] private float minInterval = 0.35f;

    [Tooltip("Time in seconds to reach approximately the minimum interval")]
    [SerializeField] private float rampDuration = 90f;

    [Header("More enemies over time")]
    [Tooltip("Maximum additional enemies per wave (besides the main one)")]
    [SerializeField] private int maxExtraPerWave = 2; // up to 3 per wave

    [Tooltip("How quickly the chance for extra enemies grows (0..1)")]
    [SerializeField] private float extraGrowth = 1.0f;

    [Header("Double spawn chance")]
    [Range(0f, 1f)]
    [SerializeField] private float doubleSpawnBaseChance = 0.10f;

    [Range(0f, 1f)]
    [SerializeField] private float doubleSpawnMaxChance = 0.35f;

    [Header("Rare big enemies")]
    [Range(0f, 1f)]
    [SerializeField] private float bigEnemyChance = 0.06f;

    [SerializeField] private Vector2 bigScaleRange = new Vector2(1.4f, 1.9f);

    [Tooltip("Extra mass multiplier for big enemies (optional)")]
    [SerializeField] private float bigMassMultiplier = 1.6f;

    private Coroutine loop;
    private bool isActive;

    // Keeps track of the current main object
    private FallingObject current;

    private float startTime;

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (fallingPrefab == null)
        {
            Debug.LogError("Spawner: fallingPrefab is not set!");
            return;
        }

        if (arena == null)
        {
            Debug.LogError("Spawner: arena is not set!");
            return;
        }

        if (isActive) return;

        isActive = true;
        startTime = Time.time;
        loop = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        isActive = false;

        if (loop != null)
        {
            StopCoroutine(loop);
            loop = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (isActive)
        {
            // Spawn only if there is no current main object
            if (current == null)
            {
                current = SpawnWave();
            }

            // Wait until the main object is destroyed
            yield return new WaitUntil(() => current == null);

            // Dynamic spawn interval
            yield return new WaitForSeconds(GetCurrentInterval());
        }
    }

    /// <summary>
    /// Spawns a wave: one main enemy + possible additional enemies.
    /// Returns the main enemy reference.
    /// </summary>
    private FallingObject SpawnWave()
    {
        float t01 = GetProgress01();

        // Main enemy
        FallingObject main = SpawnOne();

        // Random double spawn
        float doubleChance = Mathf.Lerp(doubleSpawnBaseChance, doubleSpawnMaxChance, t01);
        if (Random.value < doubleChance)
            SpawnOne();

        // More enemies over time
        int extraCount = 0;
        float p = Mathf.Clamp01(t01 * extraGrowth);

        for (int i = 0; i < maxExtraPerWave; i++)
        {
            if (Random.value < p)
                extraCount++;
        }

        for (int i = 0; i < extraCount; i++)
            SpawnOne();

        return main;
    }

    private FallingObject SpawnOne()
    {
        Bounds b = arena.bounds;

        float minX = b.min.x + spawnPadding;
        float maxX = b.max.x - spawnPadding;

        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, spawnY, 0f);

        FallingObject obj = Instantiate(fallingPrefab, pos, Quaternion.identity);

        TryMakeBig(obj);

        return obj;
    }

    private void TryMakeBig(FallingObject obj)
    {
        if (obj == null) return;

        float t01 = GetProgress01();
        float chance = Mathf.Lerp(bigEnemyChance * 0.6f, bigEnemyChance, t01);

        if (Random.value > chance) return;

        float s = Random.Range(bigScaleRange.x, bigScaleRange.y);
        obj.transform.localScale = obj.transform.localScale * s;

        // Optional: make big enemies feel heavier
        var rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.mass *= bigMassMultiplier;
    }

    private float GetCurrentInterval()
    {
        float t01 = GetProgress01();
        return Mathf.Lerp(startInterval, minInterval, t01);
    }

    private float GetProgress01()
    {
        float elapsed = Time.time - startTime;
        if (rampDuration <= 0f) return 1f;
        return Mathf.Clamp01(elapsed / rampDuration);
    }
}