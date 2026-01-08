using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UniqueID))]

public class SaveData
{
    public List<string> collectedItems;
    public SerializableDictionary<string, ItemPickupSaveData> activeItems;

    public SerializableDictionary<string, InventorySaveData> chestDictionary;

    public InventorySaveData playerInventory;
    public SaveData()
    {
        collectedItems = new List<string>();
        activeItems = new SerializableDictionary<string, ItemPickupSaveData>();
        chestDictionary = new SerializableDictionary<string, InventorySaveData>();
        playerInventory = new InventorySaveData();
    }
}