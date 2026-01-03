using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent (typeof(UniqueID))]
public class ItemPickup : MonoBehaviour
{
    public float PickupRadius = 1f;
    public InventoryItemData ItemData;

    [SerializeField] private float _rotationSpeed = 20f;

    private SphereCollider myCollider;

    [SerializeField] private ItemPickupSaveData itemSaveData;
    private string id;

    private void Awake()
    {
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickupSaveData(ItemData,transform.position,transform.rotation);

        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickupRadius;
    }
    private void Start()
    {
        id = GetComponent<UniqueID>().ID;
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }
    private void LoadGame(SaveData data)
    {
        if(data.collectedItems.Contains(id)) Destroy(this.gameObject);
    }
    private void OnDestroy()
    {
        if(SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.OnLoadGame -= LoadGame;
    }
    private void OnTriggerEnter(Collider other)
    {
    //    if (Keyboard.current.fKey.isPressed)
        {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventory) return;

            if (inventory.AddToInventory(ItemData, 1))
            {
                SaveGameManager.data.collectedItems.Add(id);
                Destroy(this.gameObject);
                Debug.Log("item picked up");

                TutorialManager tutorial = FindObjectOfType<TutorialManager>();
                if (tutorial != null)
                    tutorial.MarkStepComplete("pickupItem");
            }
        }
    }
    
}

[System.Serializable]
public struct ItemPickupSaveData
{
    public InventoryItemData ItemData;
    public Vector3 Position;
    public Quaternion Rotation;

    public ItemPickupSaveData(InventoryItemData _data, Vector3 _position, Quaternion _rotation)
    {
        ItemData = _data;
        Position = _position;
        Rotation = _rotation;
    }
}