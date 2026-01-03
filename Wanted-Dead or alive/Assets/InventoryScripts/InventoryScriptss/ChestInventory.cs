using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    public bool RequiresCursorLock => true;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        var chestSaveData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);
        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    protected override void LoadInventory(SaveData data)
    {
        // Zkontroluj, jestli tento GameObject není znièen
        if (this == null || gameObject == null) return;

        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.primaryInventorySystem = chestData.InvSystem;
            this.transform.position = chestData.Position;
            this.transform.rotation = chestData.Rotation;
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem, 0);
        interactSuccesful = true;
    }

    public void EndInteraction() { }
}