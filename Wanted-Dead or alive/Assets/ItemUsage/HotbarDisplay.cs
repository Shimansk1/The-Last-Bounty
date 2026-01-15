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

        // Bezpeènostní kontroly...
        if (slots == null || slots.Length == 0) return;
        _maxIndexSize = slots.Length - 1;

        playerNeeds = FindObjectOfType<PlayerNeeds>();
        weaponHandler = FindObjectOfType<WeaponHandler>();

        // Inicializace prvního slotu
        _currentIndex = 0;
        slots[_currentIndex].ToggleHighlight();

        // --- TOTO JE NOVÉ: Hned na startu zkontrolujeme, co držíme ---
        UpdateActiveSlot();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _playerControls.Enable();
        // Eventy pro èísla...
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
        // Odhlášení eventù... (zjednodušeno pro pøehlednost, nech tam to svoje)
        _playerControls.Player.UseItem.performed -= UseItem;
    }

    private void Update()
    {
        float scroll = _playerControls.Player.MouseWheel.ReadValue<float>();
        if (scroll > 0.01f) ChangeIndex(-1);
        if (scroll < -0.01f) ChangeIndex(1);
    }

    // --- NOVÁ METODA: Øeší automatické nasazování vìcí pøi zmìnì slotu ---
    private void UpdateActiveSlot()
    {
        // Pokud nemáme weapon handler, nemáme co øešit
        if (weaponHandler == null) return;

        // Získáme data ze slotu
        var currentSlot = slots[_currentIndex].AssignedInventorySlot;

        // Pokud je slot prázdný nebo item je null
        if (currentSlot == null || currentSlot.ItemData == null)
        {
            weaponHandler.UnequipWeapon(); // Nic nedržíme -> odvybavit zbraò
            return;
        }

        InventoryItemData item = currentSlot.ItemData;

        // Pokud je to zbraò, automaticky ji nasadíme
        if (item.itemType == ItemType.Weapon)
        {
            weaponHandler.EquipWeapon(item);
        }
        else
        {
            // Pokud držíme jablko nebo kámen, schováme pistoli
            weaponHandler.UnequipWeapon();
        }
    }

    // Upravená metoda pro zmìnu indexu (koleèko myši)
    private void ChangeIndex(int direction)
    {
        slots[_currentIndex].ToggleHighlight(); // Zhasnout starý
        _currentIndex += direction;

        if (_currentIndex > _maxIndexSize) _currentIndex = 0;
        else if (_currentIndex < 0) _currentIndex = _maxIndexSize;

        slots[_currentIndex].ToggleHighlight(); // Rozsvítit nový

        // --- ZAVOLAT UPDATE AKTIVNÍHO ITEMU ---
        UpdateActiveSlot();
    }

    // Upravená metoda pro nastavení indexu (klávesy 1-9)
    private void SetIndex(int newIndex)
    {
        slots[_currentIndex].ToggleHighlight(); // Zhasnout starý
        _currentIndex = Mathf.Clamp(newIndex, 0, _maxIndexSize);
        slots[_currentIndex].ToggleHighlight(); // Rozsvítit nový

        // --- ZAVOLAT UPDATE AKTIVNÍHO ITEMU ---
        UpdateActiveSlot();
    }

    private void UseItem(InputAction.CallbackContext obj)
    {
        // Tady øešíme POUZE konzumaci jídla/pití.
        // Zbranì už øeší WeaponHandler.Update() a HotbarDisplay.UpdateActiveSlot()

        var currentSlot = slots[_currentIndex].AssignedInventorySlot;
        if (currentSlot == null || currentSlot.ItemData == null) return;

        var item = currentSlot.ItemData;

        switch (item.itemType)
        {
            case ItemType.Food:
            case ItemType.Drink:
                if (playerNeeds != null)
                {
                    item.UseItem(playerNeeds);
                    currentSlot.RemoveFromStack(1);
                    slots[_currentIndex].UpdateUISlot();

                    // Pokud jsme snìdli poslední kus, musíme aktualizovat co držíme (aby zmizela tøeba prázdná láhev, kdyby to byl model)
                    if (currentSlot.StackSize <= 0)
                    {
                        UpdateActiveSlot();
                    }
                }
                break;

            // CASE WEAPON JSME VYHODILI - Zbraò se "nepoužívá" kliknutím v inventáøi, ale støílí se s ní
            case ItemType.Weapon:
                // Zde nedìláme nic, støelbu øeší WeaponHandler
                break;

            default:
                Debug.Log("Item used (generic)");
                break;
        }
    }
}