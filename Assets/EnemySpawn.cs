using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject prefab;
        [Min(0.01f)] public float weight = 1f;
    }

    public enum EnemyGroup
    {
        Barrel,
        Alien
    }

    [Header("References")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Transform player;

    [Header("Barrels (spawn from top)")]
    [SerializeField] private SpawnEntry[] barrelPrefabs;
    [SerializeField] private float barrelSpawnInterval = 3.5f;
    [SerializeField] private int maxBarrelsAlive = 4;
    [SerializeField] private float topSpawnOffset = 0.8f;
    [SerializeField] private Vector3 barrelFixedScale = new Vector3(0.35f, 0.35f, 1f);

    [Header("Aliens (spawn on side edges)")]
    [SerializeField] private SpawnEntry[] alienPrefabs;
    [SerializeField] private float alienSpawnInterval = 4.5f;
    [SerializeField] private int maxAliensAlive = 3;
    [SerializeField] private float alienEdgeInset = 0.6f;

    [Header("Difficulty")]
    [SerializeField] private float minBarrelInterval = 1.4f;
    [SerializeField] private float minAlienInterval = 1.8f;
    [SerializeField] private float rampDuration = 90f;
    [SerializeField] private int extraBarrelsOverTime = 3;
    [SerializeField] private int extraAliensOverTime = 2;

    [Header("Spawn Rules")]
    [SerializeField] private float spawnPadding = 0.6f;
    [SerializeField] private float minDistanceFromPlayer = 2.2f;
    [SerializeField] private int maxSpawnPointAttempts = 20;

    private bool isActive;
    private float startTime;

    private int aliveBarrels;
    private int aliveAliens;

    private Coroutine barrelLoop;
    private Coroutine alienLoop;

    private void Start()
    {
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (isActive) return;

        if (arena == null)
        {
            Debug.LogError("EnemySpawner: arena is not assigned.");
            return;
        }

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        isActive = true;
        startTime = Time.time;

        barrelLoop = StartCoroutine(BarrelSpawnLoop());
        alienLoop = StartCoroutine(AlienSpawnLoop());
    }

    public void StopSpawning()
    {
        isActive = false;

        if (barrelLoop != null)
        {
            StopCoroutine(barrelLoop);
            barrelLoop = null;
        }

        if (alienLoop != null)
        {
            StopCoroutine(alienLoop);
            alienLoop = null;
        }
    }

    private IEnumerator BarrelSpawnLoop()
    {
        while (isActive)
        {
            int currentMax = maxBarrelsAlive + Mathf.RoundToInt(GetProgress01() * extraBarrelsOverTime);

            if (aliveBarrels < currentMax)
                SpawnBarrel();

            yield return new WaitForSeconds(GetCurrentBarrelInterval());
        }
    }

    private IEnumerator AlienSpawnLoop()
    {
        while (isActive)
        {
            int currentMax = maxAliensAlive + Mathf.RoundToInt(GetProgress01() * extraAliensOverTime);

            if (aliveAliens < currentMax)
                SpawnAlien();

            yield return new WaitForSeconds(GetCurrentAlienInterval());
        }
    }

    private void SpawnBarrel()
    {
        GameObject prefab = PickWeightedPrefab(barrelPrefabs);
        if (prefab == null) return;

        Bounds b = arena.bounds;

        for (int i = 0; i < maxSpawnPointAttempts; i++)
        {
            float x = Random.Range(b.min.x + spawnPadding, b.max.x - spawnPadding);
            float y = b.max.y + topSpawnOffset;

            Vector3 spawnPos = new Vector3(x, y, 0f);

            if (player != null && Vector2.Distance(spawnPos, player.position) < minDistanceFromPlayer)
                continue;

            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
            obj.transform.localScale = barrelFixedScale;

            aliveBarrels++;
            RegisterSpawnedEnemy(obj, EnemyGroup.Barrel);
            return;
        }
    }

    private void SpawnAlien()
    {
        GameObject prefab = PickWeightedPrefab(alienPrefabs);
        if (prefab == null) return;

        if (!TryGetAlienSideSpawnPosition(out Vector3 spawnPos))
            return;

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

        aliveAliens++;
        RegisterSpawnedEnemy(obj, EnemyGroup.Alien);
    }

    private bool TryGetAlienSideSpawnPosition(out Vector3 result)
    {
        Bounds b = arena.bounds;

        for (int i = 0; i < maxSpawnPointAttempts; i++)
        {
            bool left = Random.value < 0.5f;

            float x = left ? b.min.x + alienEdgeInset : b.max.x - alienEdgeInset;
            float y = Random.Range(b.min.y + spawnPadding, b.max.y - spawnPadding);

            Vector3 candidate = new Vector3(x, y, 0f);

            if (player != null && Vector2.Distance(candidate, player.position) < minDistanceFromPlayer)
                continue;

            result = candidate;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private GameObject PickWeightedPrefab(SpawnEntry[] entries)
    {
        if (entries == null || entries.Length == 0) return null;

        float totalWeight = 0f;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] != null && entries[i].prefab != null)
                totalWeight += Mathf.Max(0f, entries[i].weight);
        }

        if (totalWeight <= 0f) return null;

        float roll = Random.value * totalWeight;
        float sum = 0f;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] == null || entries[i].prefab == null) continue;

            sum += Mathf.Max(0f, entries[i].weight);
            if (roll <= sum)
                return entries[i].prefab;
        }

        return null;
    }

    private void RegisterSpawnedEnemy(GameObject obj, EnemyGroup group)
    {
        SpawnedEnemy marker = obj.GetComponent<SpawnedEnemy>();
        if (marker == null)
            marker = obj.AddComponent<SpawnedEnemy>();

        marker.Init(this, group);
    }

    public void NotifyEnemyDestroyed(EnemyGroup group)
    {
        if (group == EnemyGroup.Barrel)
            aliveBarrels = Mathf.Max(0, aliveBarrels - 1);
        else
            aliveAliens = Mathf.Max(0, aliveAliens - 1);
    }

    private float GetCurrentBarrelInterval()
    {
        return Mathf.Lerp(barrelSpawnInterval, minBarrelInterval, GetProgress01());
    }

    private float GetCurrentAlienInterval()
    {
        return Mathf.Lerp(alienSpawnInterval, minAlienInterval, GetProgress01());
    }

    private float GetProgress01()
    {
        if (rampDuration <= 0f) return 1f;
        return Mathf.Clamp01((Time.time - startTime) / rampDuration);
    }
}