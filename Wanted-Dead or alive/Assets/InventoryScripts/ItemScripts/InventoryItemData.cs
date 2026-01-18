using UnityEngine;

public enum ItemType
{
    Food,
    Drink,
    Weapon,
    Tool,
    Material,
    QuestItem
}

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID = -1;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize = 1;
    public int MoneyValue;
    public GameObject ItemPrefab;

    public ItemType itemType;

    [Header("Food/Drink Settings")]
    public int hungerRestore = 1;
    public int thirstRestore = 1;

    [Header("Weapon Settings")]
    public int weaponDamage = 10;
    public float weaponRange = 2f;
    public float attackCooldown = 0.6f;

    [Header("Prefabs")]
    public GameObject WeaponInHandPrefab;



    public void UseItem(PlayerNeeds playerNeeds)
    {

        switch (itemType)
        {
            case ItemType.Food:
                if (hungerRestore > 0)
                {
                    playerNeeds.ModifyHunger(hungerRestore);
                }
                break;

            case ItemType.Drink:
                if (thirstRestore > 0)
                {
                    playerNeeds.ModifyThirst(thirstRestore);
                    TutorialManager tutorial = FindObjectOfType<TutorialManager>();
                    if (tutorial != null)
                    tutorial.MarkStepComplete("drinkRum");
                }
                break;

            default:
                break;
        }
    }
}
    