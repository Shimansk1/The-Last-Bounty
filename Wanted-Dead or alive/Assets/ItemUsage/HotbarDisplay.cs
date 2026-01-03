using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class HotbarDisplay : StaticInventoryDisplay
{
    private int _maxIndexSize = 9;
    private int _currentIndex = 0;

    private PlayerNeeds playerNeeds;
    private PlayerControls _playerControls;
    private WeaponHandler weaponHandler;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        Debug.Log("HotbarDisplay Awake: PlayerControls initialized");
    }

    protected override void Start()
    {
        base.Start();
        _currentIndex = 0;
        Debug.Log($"HotbarDisplay Start: Slots array = {(slots != null ? slots.Length : "null")}");
        if (slots == null || slots.Length == 0)
        {
            Debug.LogError("Slots array is null or empty!");
            return;
        }
        _maxIndexSize = slots.Length - 1;
        Debug.Log($"MaxIndexSize set to: {_maxIndexSize}");

        // Kontrola jednotlivých slotù
        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log($"Slot {i}: {(slots[i] != null ? "Valid" : "Null")}");
            if (slots[i] != null)
            {
                Debug.Log($"Slot {i} AssignedInventorySlot: {(slots[i].AssignedInventorySlot != null ? "Valid" : "Null")}");
            }
        }

        if (slots[_currentIndex] != null)
        {
            slots[_currentIndex].ToggleHighlight();
            Debug.Log($"Initial highlight set on slot {_currentIndex}");
        }
        else
        {
            Debug.LogError($"Slot at index {_currentIndex} is null!");
        }

        playerNeeds = FindObjectOfType<PlayerNeeds>();
        weaponHandler = FindObjectOfType<WeaponHandler>();
        Debug.Log($"PlayerNeeds: {(playerNeeds != null ? "Found" : "Not found")}");
        Debug.Log($"WeaponHandler: {(weaponHandler != null ? "Found" : "Not found")}");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerControls.Enable();
        Debug.Log("HotbarDisplay OnEnable: PlayerControls enabled");

        _playerControls.Player.Hotbar1.performed += Hotbar1;
        _playerControls.Player.Hotbar2.performed += Hotbar2;
        _playerControls.Player.Hotbar3.performed += Hotbar3;
        _playerControls.Player.Hotbar4.performed += Hotbar4;
        _playerControls.Player.Hotbar5.performed += Hotbar5;
        _playerControls.Player.Hotbar6.performed += Hotbar6;
        _playerControls.Player.Hotbar7.performed += Hotbar7;
        _playerControls.Player.Hotbar8.performed += Hotbar8;
        _playerControls.Player.Hotbar9.performed += Hotbar9;
        _playerControls.Player.Hotbar10.performed += Hotbar10;
        _playerControls.Player.UseItem.performed += UseItem;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerControls.Disable();
        Debug.Log("HotbarDisplay OnDisable: PlayerControls disabled");

        _playerControls.Player.Hotbar1.performed -= Hotbar1;
        _playerControls.Player.Hotbar2.performed -= Hotbar2;
        _playerControls.Player.Hotbar3.performed -= Hotbar3;
        _playerControls.Player.Hotbar4.performed -= Hotbar4;
        _playerControls.Player.Hotbar5.performed -= Hotbar5;
        _playerControls.Player.Hotbar6.performed -= Hotbar6;
        _playerControls.Player.Hotbar7.performed -= Hotbar7;
        _playerControls.Player.Hotbar8.performed -= Hotbar8;
        _playerControls.Player.Hotbar9.performed -= Hotbar9;
        _playerControls.Player.Hotbar10.performed -= Hotbar10;
        _playerControls.Player.UseItem.performed -= UseItem;
    }

    #region Hotbar Select Methods
    private void Hotbar1(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar1 input detected");
        SetIndex(0);
    }
    private void Hotbar2(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar2 input detected");
        SetIndex(1);
    }
    private void Hotbar3(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar3 input detected");
        SetIndex(2);
    }
    private void Hotbar4(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar4 input detected");
        SetIndex(3);
    }
    private void Hotbar5(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar5 input detected");
        SetIndex(4);
    }
    private void Hotbar6(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar6 input detected");
        SetIndex(5);
    }
    private void Hotbar7(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar7 input detected");
        SetIndex(6);
    }
    private void Hotbar8(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar8 input detected");
        SetIndex(7);
    }
    private void Hotbar9(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar9 input detected");
        SetIndex(8);
    }
    private void Hotbar10(InputAction.CallbackContext obj)
    {
        Debug.Log("Hotbar10 input detected");
        SetIndex(9);
    }
    #endregion

    private void Update()
    {
        float scroll = _playerControls.Player.MouseWheel.ReadValue<float>();
        if (scroll != 0)
        {
            Debug.Log($"Mouse Wheel input: {scroll}");
        }
        if (scroll > 0.01f) ChangeIndex(-1);
        if (scroll < -0.01f) ChangeIndex(1);
    }

    private void UseItem(InputAction.CallbackContext obj)
    {
        Debug.Log($"UseItem called: _currentIndex = {_currentIndex}");

        // Kontrola pole slots
        if (slots == null || slots.Length == 0)
        {
            Debug.LogError("Slots array is null or empty!");
            return;
        }

        // Kontrola konkrétního slotu
        if (slots[_currentIndex] == null)
        {
            Debug.LogError($"Slot at index {_currentIndex} is null!");
            return;
        }

        // Kontrola AssignedInventorySlot
        var currentSlot = slots[_currentIndex].AssignedInventorySlot;
        if (currentSlot == null)
        {
            Debug.LogError($"AssignedInventorySlot at index {_currentIndex} is null!");
            return;
        }

        var item = currentSlot.ItemData;
        if (item == null)
        {
            Debug.Log("No item in current slot");
            return;
        }

        Debug.Log($"Using item: {item.name}, Type: {item.itemType}");
        switch (item.itemType)
        {
            case ItemType.Food:
            case ItemType.Drink:
                if (playerNeeds == null)
                {
                    Debug.LogError("PlayerNeeds is null!");
                    return;
                }
                item.UseItem(playerNeeds);
                currentSlot.RemoveFromStack(1);
                slots[_currentIndex].UpdateUISlot();
                break;

            case ItemType.Weapon:
                if (weaponHandler == null)
                {
                    Debug.LogError("WeaponHandler is null!");
                    return;
                }
                weaponHandler.EquipWeapon(item);
                break;

            default:
                Debug.Log("Tento typ itemu zatím neumíme použít.");
                break;
        }
    }

    private void ChangeIndex(int direction)
    {
        if (slots.Length == 0)
        {
            Debug.LogError("Slots array is empty!");
            return;
        }
        Debug.Log($"ChangeIndex: Current = {_currentIndex}, Direction = {direction}");
        slots[_currentIndex].ToggleHighlight();
        _currentIndex += direction;
        if (_currentIndex > _maxIndexSize)
            _currentIndex = 0;
        else if (_currentIndex < 0)
            _currentIndex = _maxIndexSize;
        slots[_currentIndex].ToggleHighlight();
        Debug.Log($"New Index: {_currentIndex}");
    }

    private void SetIndex(int newIndex)
    {
        if (slots.Length == 0)
        {
            Debug.LogError("Slots array is empty!");
            return;
        }
        Debug.Log($"SetIndex: Old = {_currentIndex}, New = {newIndex}");
        slots[_currentIndex].ToggleHighlight();
        _currentIndex = Mathf.Clamp(newIndex, 0, _maxIndexSize);
        slots[_currentIndex].ToggleHighlight();
        Debug.Log($"Set Index to: {_currentIndex}");
    }
}