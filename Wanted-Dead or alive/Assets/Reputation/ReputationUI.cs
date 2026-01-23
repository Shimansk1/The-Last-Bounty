using UnityEngine;
using UnityEngine.UI;

public class ReputationUI : MonoBehaviour
{
    [Header("Nastavení")]
    public CityName cityToDisplay; 

    [Header("UI Elementy")]
    [SerializeField] private Image reputationBar;
    [SerializeField] private Text reputationText;

    private ReputationManager reputationManager;

    void Start()
    {
        reputationManager = ReputationManager.Instance;
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (reputationManager == null) return;

        float reputation = reputationManager.GetReputation(cityToDisplay);

        if (reputationBar != null)
            reputationBar.fillAmount = reputation / 100f;

        if (reputationText != null)
            reputationText.text = $"{cityToDisplay}: {reputation:F0}%";
    }
}