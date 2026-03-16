using UnityEngine;

public class SpawnedEnemy : MonoBehaviour
{
    private EnemySpawner spawner;
    private EnemySpawner.EnemyGroup group;
    private bool initialized;

    public void Init(EnemySpawner owner, EnemySpawner.EnemyGroup enemyGroup)
    {
        spawner = owner;
        group = enemyGroup;
        initialized = true;
    }

    private void OnDestroy()
    {
        if (!initialized) return;
        if (spawner == null) return;

        spawner.NotifyEnemyDestroyed(group);
    }
}