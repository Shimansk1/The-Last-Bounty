using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private MouseLook mouseLook;

    public static UnityAction OnPlayerInventoryChanged;

    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;

    private bool isInventoryOpen = false;

    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
    }

    protected override void LoadInventory(SaveData data)
    {
        if (data.playerInventory.InvSystem != null)
        {
            this.primaryInventorySystem = data.playerInventory.InvSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }

    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame && !isInventoryOpen)
        {
            isInventoryOpen = true;
            OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mouseLook.canMove = false;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame && isInventoryOpen)
        {
            isInventoryOpen = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mouseLook.canMove = true;
        }
    }


    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        return false;
    }

    [Header("Ekonomika")]
    [SerializeField] private int currentGold = 500; // Startovní peníze
    public int CurrentGold => currentGold;

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            Debug.Log($"Utraceno {amount} zlata. Zùstatek: {currentGold}");
            // Tady bys mohl zavolat event pro aktualizaci UI penìz
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
    }
}
