using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Crafting/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<BuildRequirement> ingredients;
    public InventoryItemData resultItem;
    public int resultAmount = 1;
}
