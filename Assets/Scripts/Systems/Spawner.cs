using System.Collections;
using System.Collections.Generic;
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

    [Header("Lane Spawn")]
    [SerializeField] private int laneCount = 5;
    [SerializeField] private float laneEdgePadding = 0.4f;

    [Header("Difficulty / Pace")]
    [Tooltip("Initial spawn interval between waves")]
    [SerializeField] private float startInterval = 1.8f;

    [Tooltip("Minimum interval (game will not get faster than this)")]
    [SerializeField] private float minInterval = 0.35f;

    [Tooltip("Time in seconds to reach approximately the minimum interval")]
    [SerializeField] private float rampDuration = 90f;

    [Header("More enemies over time")]
    [Tooltip("Maximum additional enemies per wave (besides the main one)")]
    [SerializeField] private int maxExtraPerWave = 2;

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

    [Tooltip("Extra mass multiplier for big enemies")]
    [SerializeField] private float bigMassMultiplier = 1.6f;

    [Header("Enemy Speed Ramp")]
    [SerializeField] private float maxFallSpeedMultiplier = 1.3f;

    [Header("Wave Timing")]
    [SerializeField] private Vector2 extraSpawnDelayRange = new Vector2(0.1f, 0.35f);

    [Header("Score Values")]
    [SerializeField] private int bigEnemyScore = 25;

    private Coroutine loop;
    private bool isActive;
    private FallingObject current;
    private float startTime;

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

        if (laneCount < 1)
        {
            Debug.LogError("Spawner: laneCount must be at least 1!");
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
            if (current == null)
            {
                current = SpawnWave();
            }

            yield return new WaitUntil(() => current == null || !isActive);

            if (!isActive)
                yield break;

            yield return new WaitForSeconds(GetCurrentInterval());
        }
    }

    private FallingObject SpawnWave()
    {
        float t01 = GetProgress01();
        var usedLanes = new List<int>();

        FallingObject main = SpawnOne(usedLanes);

        float doubleChance = Mathf.Lerp(doubleSpawnBaseChance, doubleSpawnMaxChance, t01);
        if (Random.value < doubleChance)
            StartCoroutine(SpawnWithDelay(usedLanes));

        int extraCount = 0;
        float p = Mathf.Clamp01(t01 * extraGrowth);

        for (int i = 0; i < maxExtraPerWave; i++)
        {
            if (Random.value < p)
                extraCount++;
        }

        for (int i = 0; i < extraCount; i++)
        {
            StartCoroutine(SpawnWithDelay(usedLanes));
        }

        return main;
    }

    private IEnumerator SpawnWithDelay(List<int> usedLanes)
    {
        float delay = Random.Range(extraSpawnDelayRange.x, extraSpawnDelayRange.y);
        yield return new WaitForSeconds(delay);

        if (!isActive)
            yield break;

        SpawnOne(usedLanes);
    }

    private FallingObject SpawnOne(List<int> usedLanes)
    {
        Bounds b = arena.bounds;

        float minX = b.min.x + spawnPadding + laneEdgePadding;
        float maxX = b.max.x - spawnPadding - laneEdgePadding;

        float totalWidth = maxX - minX;
        float laneWidth = totalWidth / laneCount;

        int chosenLane = GetFreeLane(usedLanes);
        usedLanes.Add(chosenLane);

        float laneCenterX = minX + (laneWidth * chosenLane) + (laneWidth * 0.5f);
        Vector3 pos = new Vector3(laneCenterX, spawnY, 0f);

        FallingObject obj = Instantiate(fallingPrefab, pos, Quaternion.identity);

        float t01 = GetProgress01();
        obj.MultiplyFallSpeed(Mathf.Lerp(1f, maxFallSpeedMultiplier, t01));

        TryMakeBig(obj);

        return obj;
    }

    private int GetFreeLane(List<int> usedLanes)
    {
        var candidates = new List<int>();

        for (int i = 0; i < laneCount; i++)
        {
            bool blocked = false;

            foreach (int used in usedLanes)
            {
                if (Mathf.Abs(i - used) <= 1)
                {
                    blocked = true;
                    break;
                }
            }

            if (!blocked)
                candidates.Add(i);
        }

        if (candidates.Count == 0)
            return Random.Range(0, laneCount);

        return candidates[Random.Range(0, candidates.Count)];
    }

    private void TryMakeBig(FallingObject obj)
    {
        if (obj == null) return;

        float t01 = GetProgress01();
        float chance = Mathf.Lerp(bigEnemyChance * 0.6f, bigEnemyChance, t01);

        if (Random.value > chance) return;

        float s = Random.Range(bigScaleRange.x, bigScaleRange.y);
        obj.transform.localScale = obj.transform.localScale * s;

        obj.SetHP(2);
        obj.SetScoreValue(bigEnemyScore);
        obj.SetBigEnemy(true);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
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