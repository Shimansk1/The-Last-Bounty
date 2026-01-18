using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("UI Reference")]
    public GameObject shopWindow;       // Celý panel obchodu
    public Transform itemsContainer;    // Content ve ScrollView (kam se hází tlaèítka)
    public GameObject itemButtonPrefab; // Obyèejný PREFAB tlaèítka (vysvìtlím níže)
    public TextMeshProUGUI MoneyText;    // Text kolik máš penìz

    private PlayerInventoryHolder playerInventory;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        shopWindow.SetActive(false);
    }

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventoryHolder>();
    }

    // Tuhle metodu zavolá NPC a pošle svùj seznam vìcí (InventoryItemData)
    public void OpenShop(List<InventoryItemData> itemsForSale)
    {
        shopWindow.SetActive(true);
        UpdateGoldUI();

        // 1. Smažeme stará tlaèítka
        foreach (Transform child in itemsContainer) Destroy(child.gameObject);

        // 2. Vygenerujeme nová tlaèítka pøímo z dat
        foreach (var item in itemsForSale)
        {
            // Vytvoøíme tlaèítko
            GameObject btnObj = Instantiate(itemButtonPrefab, itemsContainer);
            Button btn = btnObj.GetComponent<Button>();

            // --- TADY JE TEN TRIK BEZ DALŠÍHO SKRIPTU ---
            // Pøedpokládáme, že Prefab má v sobì Image (ikonka) a Text (Cena)

            // Najdeme Image pro ikonku (tøeba první Image v dìtech tlaèítka)
            // POZOR: Button má taky Image (pozadí), takže hledáme specifický název nebo prostì Image "Icon"
            Image[] images = btnObj.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                // Pokud to není pozadí tlaèítka, je to asi ikonka itemu
                if (img.gameObject != btnObj)
                {
                    img.sprite = item.Icon;
                    img.color = Color.white;
                }
            }

            // Najdeme Texty (Jméno a Cena)
            TextMeshProUGUI[] texts = btnObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length > 0) texts[0].text = item.DisplayName; // První text je jméno
            if (texts.Length > 1) texts[1].text = item.MoneyValue + " G"; // Druhý text je cena

            // Nastavíme kliknutí
            btn.onClick.AddListener(() => BuyItem(item));
        }

        // Odemkneme myš
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseShop()
    {
        shopWindow.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void BuyItem(InventoryItemData item)
    {
        if (playerInventory.CurrentMoney >= item.MoneyValue)
        {
            // Zkusíme pøidat do inventáøe (používáme tvoji metodu AddToInventory)
            if (playerInventory.AddToInventory(item, 1))
            {
                playerInventory.SpendMoney(item.MoneyValue);
                UpdateGoldUI();
                Debug.Log($"Koupil jsi {item.DisplayName}");
            }
            else
            {
                Debug.Log("Máš plný inventáø!");
            }
        }
        else
        {
            Debug.Log("Nemáš dost penìz!");
        }
    }

    void UpdateGoldUI()
    {
        if (MoneyText != null) MoneyText.text = "Zlato: " + playerInventory.CurrentMoney;
    }
}