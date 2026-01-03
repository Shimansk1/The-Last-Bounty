using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private CraftingRecipe recipe;
    [SerializeField] private PlayerInventoryHolder playerInventoryHolder;

    public AudioClip endgameMusic;
    public AudioSource audioSource;

    public void TryCraft()
    {
        InventorySystem inventory = playerInventoryHolder.PrimaryInventorySystem;

        if (!CanCraft(recipe, inventory))
        {
            return;
        }

        foreach (var ingredient in recipe.ingredients)
        {
            int remaining = ingredient.requiredAmount;

            if (inventory.ContainsItem(ingredient.itemData, out var slots))
            {
                foreach (var slot in slots)
                {
                    if (remaining <= 0) break;

                    int remove = Mathf.Min(slot.StackSize, remaining);
                    slot.RemoveFromStack(remove);
                    remaining -= remove;
                }
            }
        }

        inventory.AddToInventory(recipe.resultItem, recipe.resultAmount);
    }

    private bool CanCraft(CraftingRecipe recipe, InventorySystem inventory)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!inventory.ContainsItem(ingredient.itemData, out var slots))
                return false;

            int total = 0;
            foreach (var slot in slots)
                total += slot.StackSize;

            if (total < ingredient.requiredAmount)
                return false;
        }

        return true;
    }
}
