using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic; // Potøeba pro List

public class NPCController : MonoBehaviour, IInteractable
{
    [Header("Data")]
    public string npcName = "Obchodník";
    public DialogNode startingDialog;

    [Header("Obchod")]
    public List<InventoryItemData> shopInventory; // SEM pøetáhneš itemy v Inspectoru

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public bool RequiresCursorLock => true;

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        interactSuccesful = true;
        DialogManager.Instance.StartDialog(this, startingDialog);
    }

    public void EndInteraction()
    {
        Debug.Log("Konec interakce");
    }

    // TOTO SE ZAVOLÁ Z DIALOG MANAGERU (když klikneš na odpovìï s triggersShop = true)
    public void OpenShop()
    {
        Debug.Log("Otevírám obchod...");
        if (shopInventory != null && shopInventory.Count > 0)
        {
            ShopManager.Instance.OpenShop(shopInventory);
        }
        else
        {
            Debug.LogWarning("Tohle NPC nic neprodává!");
        }
    }
}