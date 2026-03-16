using UnityEngine;
using UnityEngine.Events;

public class Studna : MonoBehaviour, IInteractable
{
    // Vyžadováno interfacem IInteractable
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    // Pro naplnìní èutory nepotøebujeme odemykat myš, takže vracíme false
    public bool RequiresCursorLock => false;

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        // Interactor nám posílá sám sebe, takže z nìj mùžeme zkusit vytáhnout skript Canteen.
        // Záleží, kde pøesnì máš Canteen umístìný. Toto ho zkusí najít pøímo na stejném objektu, nebo na nadøazeném.
        Canteen canteen = interactor.GetComponent<Canteen>();
        if (canteen == null)
        {
            canteen = interactor.GetComponentInParent<Canteen>();
        }

        // Pokud jsme èutoru našli, naplníme ji
        if (canteen != null)
        {
            canteen.Refill();
            Debug.Log("Èutora byla naplnìna vodou ze studny!");
            interactSuccessful = true;
        }
        else
        {
            Debug.LogWarning("Skript Canteen nebyl na hráèi nalezen!");
            interactSuccessful = false;
        }

        // Akce je jednorázová (jen klikneš a je naplnìno), takže rovnou interakci ukonèíme
        EndInteraction();
    }

    public void EndInteraction()
    {
        // Dá vìdìt systému, že interakce skonèila
        OnInteractionComplete?.Invoke(this);
    }
}