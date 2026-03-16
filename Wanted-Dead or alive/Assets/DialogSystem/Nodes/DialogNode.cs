using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialog", menuName = "Dialog/Node")]
public class DialogNode : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogText;

    public List<DialogResponse> responses;
}

[System.Serializable]
public struct DialogResponse
{
    public string responseText;
    public DialogNode nextNode;

    public bool triggersShop;
    public bool isExit;
    public bool triggersDuel;

    public bool advancesStory;
    public StoryProgress storyStateToSet;

    [Header("Item Podminky")]
    public InventoryItemData requiredItemToClick;
    public bool removesItemOnSubmit;

    [Header("Reputacni Podminky")]
    public float requiredReputation;
    public CityName reputationCity;
}