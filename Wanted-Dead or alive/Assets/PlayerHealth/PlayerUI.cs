using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("HP Bar")]
    [SerializeField] private Image HPBar;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image HPIcon;

    [Header("Reference")]
    [SerializeField] private PlayerHealth playerHealth;

    private void Update()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (playerHealth == null) return;

        float percentage = Mathf.Clamp01((float)playerHealth.currentHealth / playerHealth.maxHealth);
        HPBar.fillAmount = percentage;
        HPBar.color = Color.Lerp(Color.red, Color.green, percentage);
        HPText.text = $"{playerHealth.currentHealth} / {playerHealth.maxHealth}";
    }
}
