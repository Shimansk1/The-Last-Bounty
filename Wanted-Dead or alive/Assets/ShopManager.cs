using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("UI Reference")]
    public GameObject shopWindow;
    public Transform itemsContainer;
    public GameObject itemButtonPrefab;
    public TextMeshProUGUI MoneyText;

    private PlayerInventoryHolder playerInventory;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (shopWindow != null) shopWindow.SetActive(false);
    }

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventoryHolder>();
        UpdateMoneyUI();
    }

    public void OpenShop(List<InventoryItemData> itemsForSale)
    {
        shopWindow.SetActive(true);

        if (playerInventory == null) playerInventory = FindObjectOfType<PlayerInventoryHolder>();

        UpdateMoneyUI();

        foreach (Transform child in itemsContainer) Destroy(child.gameObject);

        foreach (var item in itemsForSale)
        {
            GameObject btnObj = Instantiate(itemButtonPrefab, itemsContainer);
            Button btn = btnObj.GetComponent<Button>();

            Image[] images = btnObj.GetComponentsInChildren<Image>();
            foreach (var img in images)
            {
                if (img.gameObject != btnObj)
                {
                    img.sprite = item.Icon;
                    img.color = Color.white;
                }
            }

            TextMeshProUGUI[] texts = btnObj.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length > 0) texts[0].text = item.DisplayName;
            if (texts.Length > 1) texts[1].text = item.MoneyValue + " $";

            btn.onClick.AddListener(() => BuyItem(item));
        }

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
        if (playerInventory == null) return;

        if (playerInventory.CurrentMoney >= item.MoneyValue)
        {
            if (playerInventory.AddToInventory(item, 1))
            {
                playerInventory.SpendMoney(item.MoneyValue);
                UpdateMoneyUI();
            }
        }
    }

    void UpdateMoneyUI()
    {
        if (MoneyText == null || playerInventory == null) return;
        MoneyText.text = "Money: " + playerInventory.CurrentMoney + " $";
    }
}