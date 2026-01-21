using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("HP Bar")]
    [SerializeField] private Image HPBar;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private Image HPIcon;

    [Header("Hunger Bar")]
    [SerializeField] private Image HungerBar;
    [SerializeField] private TextMeshProUGUI HungerText;
    [SerializeField] private Image HungerIcon;

    [Header("Thirst Bar")]
    [SerializeField] private Image ThirstBar;
    [SerializeField] private TextMeshProUGUI ThirstText;
    [SerializeField] private Image ThirstIcon;

    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerNeeds playerNeeds;

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerHealth != null && playerNeeds != null)
        {
            UpdateBar(HPBar, HPText, HPIcon, playerHealth.currentHealth, playerHealth.maxHealth, Color.green, Color.red);
            UpdateBar(HungerBar, HungerText, HungerIcon, playerNeeds.currentHunger, playerNeeds.maxHunger, new Color(0.8f, 0.5f, 0f), new Color(0.5f, 0.25f, 0f));
            UpdateBar(ThirstBar, ThirstText, ThirstIcon, playerNeeds.currentThirst, playerNeeds.maxThirst, Color.blue, Color.cyan);
        }
    }

    private void UpdateBar(Image bar, TextMeshProUGUI text, Image icon, int current, int max, Color fullColor, Color lowColor)
    {
        float percentage = Mathf.Clamp01((float)current / max);
        bar.fillAmount = percentage;
        bar.color = Color.Lerp(lowColor, fullColor, percentage);
        text.text = $"{current} / {max}";
    }
}
