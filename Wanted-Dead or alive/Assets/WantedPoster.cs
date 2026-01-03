using UnityEngine;
using UnityEngine.Events;

public class WantedPoster : MonoBehaviour, IInteractable
{
    public WantedContract contract;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public bool RequiresCursorLock => false;

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        interactSuccesful = false;

        if (contract == null) return;

        bool accepted = WantedQuestTracker.Instance.TryAcceptContract(contract);

        if (accepted)
        {
            Debug.Log($"Kontrakt pøijat: {contract.contractName}");

            interactSuccesful = true;

            // plakát zmizí
            Destroy(gameObject);
        }

        OnInteractionComplete?.Invoke(this);
    }


    public void EndInteraction()
    {
        // plakát nic neukonèuje, ale interface to vyžaduje
    }
}
