using UnityEngine;
using UnityEngine.UI;

public class PlayerHeartsUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private Color fullColor = Color.white;
    [SerializeField] private Color emptyColor = new Color(1f, 1f, 1f, 0.2f);

    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHearts;
    }

    private void UpdateHearts(int current, int max)
    {
        if (hearts == null) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            hearts[i].color = i < current ? fullColor : emptyColor;
            hearts[i].enabled = i < max;
        }
    }
}