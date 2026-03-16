using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class PlayerLivesUI : MonoBehaviour
{
    private TMP_Text livesText;

    private void Awake()
    {
        livesText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateLives;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateLives;
    }

    private void UpdateLives(int current, int max)
    {
        livesText.text = $"HP: {current}/{max}";
    }
}