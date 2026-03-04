using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private FallingObject fallingPrefab;

    [Header("Timing")]
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Area")]
    [SerializeField] private float spawnXMin = -5f;
    [SerializeField] private float spawnXMax = 5f;
    [SerializeField] private float spawnY = 6f;

    private Coroutine loop;
    private bool isActive;

    private FallingObject current;

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

        if (isActive) return;

        isActive = true;
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
                current = SpawnOne();
            }

            yield return new WaitUntil(() => current == null);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private FallingObject SpawnOne()
    {
        float x = Random.Range(spawnXMin, spawnXMax);
        Vector3 pos = new Vector3(x, spawnY, 0f);
        return Instantiate(fallingPrefab, pos, Quaternion.identity);
    }
}