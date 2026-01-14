using UnityEngine;
using UnityEngine.Events;

public class NPCController : MonoBehaviour, IInteractable
{
    [Header("Data")]
    public string npcName = "Obchodník";
    public DialogNode startingDialog; // Sem pøetáhneš ten ScriptableObject

    // ZDE JSEM SMAZAL [Header("Události")], PROTOŽE TO ZPÙSOBOVALO CHYBU
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    // Implementace tvého interface: Požadujeme kurzor myši
    public bool RequiresCursorLock => true;

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        interactSuccesful = true;

        // 1. Najdeme DialogManager (UI) a pošleme mu data
        // Používáme FindObjectOfType pro jistotu, nebo lépe Singleton DialogManager.Instance
        DialogManager.Instance.StartDialog(this, startingDialog);

        Debug.Log($"Mluvíš s {npcName}");
    }

    public void EndInteraction()
    {
        Debug.Log("Konec dialogu");
    }

    // Metoda pro otevøení obchodu
    public void OpenShop()
    {
        Debug.Log("Otevírám obchod...");
    }
}