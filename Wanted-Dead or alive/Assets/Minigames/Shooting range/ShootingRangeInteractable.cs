using UnityEngine;
using UnityEngine.Events;

public class ShootingRangeInteractable : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    // Tohle zaruèí, že ti tvùj Interactor odemkne myš, abys mohl kliknout na tlaèítko "Hrát"
    public bool RequiresCursorLock => true;

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        interactSuccesful = true;
        // Otevøe hlavní menu støelnice
        ShootingRangeManager.Instance.OpenMenu();
    }

    public void EndInteraction()
    {
        // Kdyby hráè dal ESC, zavøe se to
        ShootingRangeManager.Instance.CloseMenu();
    }
}