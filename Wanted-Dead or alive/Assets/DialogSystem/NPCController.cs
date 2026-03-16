using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public struct StoryDialog
{
    public StoryProgress requiredProgress;
    public DialogNode dialogNode;
}

public class NPCController : MonoBehaviour, IInteractable
{
    [Header("Data")]
    public string npcName = "Obchodník";
    public CityName currentCity;

    [Header("Pøíbìh a Duel")]
    [Tooltip("Je tento NPC souèástí hlavního pøíbìhového duelu?")]
    public bool isMainStoryDuelist = false;

    [Header("Dialogy")]
    public DialogNode defaultDialog;
    public List<StoryDialog> storyDialogs;

    [Header("Obchod")]
    public List<InventoryItemData> shopInventory;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public bool RequiresCursorLock => true;

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        interactSuccesful = true;
        DialogNode dialogToShow = defaultDialog;

        if (MainStoryManager.Instance != null && storyDialogs.Count > 0)
        {
            StoryProgress currentProgress = MainStoryManager.Instance.currentState;

            foreach (var sd in storyDialogs)
            {
                if (sd.requiredProgress == currentProgress)
                {
                    dialogToShow = sd.dialogNode;
                    break;
                }
            }
        }

        if (dialogToShow != null)
        {
            DialogManager.Instance.StartDialog(this, dialogToShow);
        }
    }

    public void EndInteraction()
    {
    }

    public void OpenShop()
    {
        if (shopInventory != null && shopInventory.Count > 0)
        {
            ShopManager.Instance.OpenShop(shopInventory);
        }
    }
}