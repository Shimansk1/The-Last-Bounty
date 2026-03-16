using UnityEngine;

public enum QuestState { Locked, Active, Completed }

[CreateAssetMenu(fileName = "NewMainQuest", menuName = "TheLastBounty/Main Quest")]
public class MainQuest : ScriptableObject
{
    public int questID; // Poøadové èíslo questu (1 až 9)
    public string questName;
    [TextArea(3, 5)]
    public string description;

    // Stav questu se bude mìnit bìhem hry
    public QuestState state = QuestState.Locked;
}