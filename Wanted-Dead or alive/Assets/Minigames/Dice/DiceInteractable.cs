using UnityEngine;
using UnityEngine.Events;

public class DiceInteractable : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public bool RequiresCursorLock => true;

    [Header("Lokalni nastaveni stolu")]
    public Transform localThrowPoint;
    public CityName localCity;
    public bool givesReputation = false;

    [Header("Pribehove nastaveni")]
    public bool isQuestTarget = false;

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        interactSuccesful = true;
        DiceGameManager.Instance.OpenMenu(localThrowPoint, localCity, givesReputation, isQuestTarget);
    }

    public void EndInteraction()
    {
        DiceGameManager.Instance.ExitMenu();
    }
}