using System.Collections;
using System.Collections.Generic;
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
    }

    protected override void Start()
    {
        base.Start();

        if (slots == null || slots.Length == 0) return;
        _maxIndexSize = slots.Length - 1;

        playerNeeds = FindObjectOfType<PlayerNeeds>();
        weaponHandler = FindObjectOfType<WeaponHandler>();

        _currentIndex = 0;
        slots[_currentIndex].ToggleHighlight();

        UpdateActiveSlot();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerControls.Enable();

        _playerControls.Player.Hotbar1.performed += ctx => SetIndex(0);
        _playerControls.Player.Hotbar2.performed += ctx => SetIndex(1);
        _playerControls.Player.Hotbar3.performed += ctx => SetIndex(2);
        _playerControls.Player.Hotbar4.performed += ctx => SetIndex(3);
        _playerControls.Player.Hotbar5.performed += ctx => SetIndex(4);
        _playerControls.Player.Hotbar6.performed += ctx => SetIndex(5);
        _playerControls.Player.Hotbar7.performed += ctx => SetIndex(6);
        _playerControls.Player.Hotbar8.performed += ctx => SetIndex(7);
        _playerControls.Player.Hotbar9.performed += ctx => SetIndex(8);
        _playerControls.Player.Hotbar10.performed += ctx => SetIndex(9);

        _playerControls.Player.UseItem.performed += UseItem;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _playerControls.Disable();
        _playerControls.Player.UseItem.performed -= UseItem;
    }

    private void Update()
    {
        float scroll = _playerControls.Player.MouseWheel.ReadValue<float>();
        if (scroll > 0.01f) ChangeIndex(-1);
        if (scroll < -0.01f) ChangeIndex(1);
    }

    private void UpdateActiveSlot()
    {
        if (weaponHandler == null) return;

        var currentSlot = slots[_currentIndex].AssignedInventorySlot;

        if (currentSlot == null || currentSlot.ItemData == null)
        {
            weaponHandler.UnequipWeapon();
            return;
        }

        InventoryItemData item = currentSlot.ItemData;
        weaponHandler.EquipItem(item);
    }

    private void ChangeIndex(int direction)
    {
        slots[_currentIndex].ToggleHighlight();
        _currentIndex += direction;

        if (_currentIndex > _maxIndexSize) _currentIndex = 0;
        else if (_currentIndex < 0) _currentIndex = _maxIndexSize;

        slots[_currentIndex].ToggleHighlight();

        UpdateActiveSlot();
    }

    private void SetIndex(int newIndex)
    {
        slots[_currentIndex].ToggleHighlight();
        _currentIndex = Mathf.Clamp(newIndex, 0, _maxIndexSize);
        slots[_currentIndex].ToggleHighlight();

        UpdateActiveSlot();
    }

    private void UseItem(InputAction.CallbackContext obj)
    {
        var currentSlot = slots[_currentIndex].AssignedInventorySlot;
        if (currentSlot == null || currentSlot.ItemData == null) return;

        var item = currentSlot.ItemData;

        switch (item.itemType)
        {
            case ItemType.Food:
            case ItemType.Drink:
                if (playerNeeds != null)
                {
                    if (item.useSound != null && Camera.main != null)
                    {
                        AudioSource.PlayClipAtPoint(item.useSound, Camera.main.transform.position);
                    }

                    item.UseItem(playerNeeds);
                    currentSlot.RemoveFromStack(1);
                    slots[_currentIndex].UpdateUISlot();

                    if (currentSlot.StackSize <= 0)
                    {
                        UpdateActiveSlot();
                    }
                }
                break;

            case ItemType.Weapon:
                break;

            default:
                break;
        }
    }
}