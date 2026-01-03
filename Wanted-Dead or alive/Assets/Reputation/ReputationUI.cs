using UnityEngine;
using UnityEngine.UI;

public class ReputationUI : MonoBehaviour
{
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
        float reputation = reputationManager.GetReputation();
        reputationBar.fillAmount = reputation / 100f;
        reputationText.text = $"Reputation: {reputation:F0}%";
    }
}