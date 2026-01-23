using UnityEngine;

public class WantedQuestTracker : MonoBehaviour
{
    public static WantedQuestTracker Instance;

    public WantedContract ActiveMainContract { get; private set; }
    public WantedContract ActiveSideContract { get; private set; }

    [Header("Nastavení Odmìn (Reputace)")]
    public float reputationRewardMain = 3f; 
    public float reputationRewardSide = 1f; 

    private PlayerInventoryHolder playerInventory;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventoryHolder>();
    }

    public bool TryAcceptContract(WantedContract contract)
    {
        if (contract.contractType == ContractType.Main)
        {
            if (ActiveMainContract != null) return false;

            ActiveMainContract = contract;
            BountySpawner.Instance.SpawnBountyTarget(contract);
            return true;
        }

        if (contract.contractType == ContractType.Side)
        {
            if (ActiveSideContract != null) return false;

            ActiveSideContract = contract;
            BountySpawner.Instance.SpawnBountyTarget(contract);
            return true;
        }

        return false;
    }

    public void NotifyEnemyDeath(WantedContract contract)
    {
        if (contract == ActiveMainContract)
        {
            CompleteContract(contract);
        }
        else if (contract == ActiveSideContract)
        {
            CompleteContract(contract);
        }
    }

    private void CompleteContract(WantedContract contract)
    {
        // 1. Peníze
        if (playerInventory != null)
        {
            playerInventory.AddMoney(contract.reward);
        }

        // 2. Reputace
        if (ReputationManager.Instance != null)
        {
            float repAmount = (contract.contractType == ContractType.Main) ? reputationRewardMain : reputationRewardSide;

            ReputationManager.Instance.AddReputation(contract.targetCity, repAmount);
            Debug.Log($"Pøidána reputace {repAmount}% pro mìsto {contract.targetCity}");
        }

        // 3. Vyèištìní
        if (ActiveMainContract == contract) ActiveMainContract = null;
        if (ActiveSideContract == contract) ActiveSideContract = null;

        MapManager mapManager = FindObjectOfType<MapManager>();
        if (mapManager != null) mapManager.UpdateQuestUI();
    }
}