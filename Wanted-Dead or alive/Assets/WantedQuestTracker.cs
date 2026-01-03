using UnityEngine;

public class WantedQuestTracker : MonoBehaviour
{
    public static WantedQuestTracker Instance;

    public WantedContract ActiveMainContract { get; private set; }
    public WantedContract ActiveSideContract { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool TryAcceptContract(WantedContract contract)
    {
        if (contract.contractType == ContractType.Main)
        {
            if (ActiveMainContract != null) return false;

            ActiveMainContract = contract;
            return true;
        }

        if (contract.contractType == ContractType.Side)
        {
            if (ActiveSideContract != null) return false;

            ActiveSideContract = contract;
            return true;
        }

        return false;
    }

    public void CompleteContract(WantedContract contract)
    {
        if (ActiveMainContract == contract)
            ActiveMainContract = null;

        if (ActiveSideContract == contract)
            ActiveSideContract = null;
    }
}
