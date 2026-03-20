using UnityEngine;

public class VFXDebugListener : MonoBehaviour
{
#if UNITY_EDITOR

    private void OnEnable()
    {
        FallingObject.OnEnemyHit += OnEnemyHit;
        FallingObject.OnEnemyDeath += OnEnemyDeath;
        FallingObject.OnBigEnemyPushedOut += OnBigEnemyPushedOut;

        GameManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        FallingObject.OnEnemyHit -= OnEnemyHit;
        FallingObject.OnEnemyDeath -= OnEnemyDeath;
        FallingObject.OnBigEnemyPushedOut -= OnBigEnemyPushedOut;

        GameManager.OnGameOver -= OnGameOver;
    }

    private void OnEnemyHit(FallingObject obj)
    {
        Debug.Log($"[VFX] Enemy HIT: {obj.name}");
    }

    private void OnEnemyDeath(FallingObject obj)
    {
        Debug.Log($"[VFX] Enemy DEATH: {obj.name}");
    }

    private void OnBigEnemyPushedOut(FallingObject obj)
    {
        Debug.Log($"[VFX] Big enemy OUT: {obj.name}");
    }

    private void OnGameOver()
    {
        Debug.Log("[VFX] GAME OVER triggered");
    }

#endif
}