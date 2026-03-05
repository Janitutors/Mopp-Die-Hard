using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private GameObject hitbox;
    [SerializeField] private float hitboxActiveTime = 0.1f;

    private Coroutine attackRoutine;

    private void Start()
    {
        if (hitbox != null)
            hitbox.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            DoAttack();
    }

    public void DoAttack()
    {
        if (hitbox == null) return;

        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(AttackWindow());
    }

    private IEnumerator AttackWindow()
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        hitbox.SetActive(false);
        attackRoutine = null;
    }
}