using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float attackShakeAmount = 0.05f;
    [SerializeField] private float attackShakeDuration = 0.06f;

    [SerializeField] private float bigEnemyShakeAmount = 0.10f;
    [SerializeField] private float bigEnemyShakeDuration = 0.10f;

    private Vector3 originalPos;
    private float shakeTimer;
    private float shakeAmount;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    private void OnEnable()
    {
        PlayerAttack.OnPlayerAttack += HandlePlayerAttack;
        FallingObject.OnBigEnemyPushedOut += HandleBigEnemy;
    }

    private void OnDisable()
    {
        PlayerAttack.OnPlayerAttack -= HandlePlayerAttack;
        FallingObject.OnBigEnemyPushedOut -= HandleBigEnemy;
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            Vector2 offset = Random.insideUnitCircle * shakeAmount;
            transform.localPosition = originalPos + new Vector3(offset.x, offset.y, 0f);
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    private void HandlePlayerAttack()
    {
        StartShake(attackShakeAmount, attackShakeDuration);
    }

    private void HandleBigEnemy(FallingObject obj)
    {
        StartShake(bigEnemyShakeAmount, bigEnemyShakeDuration);
    }

    private void StartShake(float amount, float duration)
    {
        shakeAmount = amount;
        shakeTimer = duration;
    }
}