using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedInventorySlot;

    public Transform _playerTransform;
    public float _dropOffset = 3f;

    private void Awake()
    {
        ItemSprite.preserveAspect = true;

        ItemSprite.color = Color.clear;
        ItemCount.text = "";

        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }
    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.ItemData.Icon;
        ItemCount.text = AssignedInventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
        Debug.Log(ItemCount.text);
    }

    private void Update()
    {
        //Vector3 mousePosition = Input.mousePosition;
        if (AssignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();
            //Debug.Log("milan je sebran");
                if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
                {
                if (AssignedInventorySlot.ItemData.ItemPrefab != null)
                {
                    Vector3 spawnPosition = _playerTransform.position + _playerTransform.forward * _dropOffset;
                    spawnPosition.y -= 1.5f;

                    Instantiate(AssignedInventorySlot.ItemData.ItemPrefab, spawnPosition, Quaternion.identity);
                }
                if(AssignedInventorySlot.StackSize > 1)
                {
                    AssignedInventorySlot.AddToStack(-1);
                    UpdateMouseSlot();
                }
                else
                {
                ClearSlot();
                }
                }
        }
    }
    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
        
    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            Debug.Log("Hit UI Element: " + result.gameObject.name);
            // Pokud je to konkrétní UI prvek, který má blokovat drop
            if (result.gameObject.CompareTag("UI"))
            {
                return true;
            }
        }
        return false;
    }

}
