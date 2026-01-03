using UnityEngine;
using UnityEngine.Events;

public class RumBarrelInteractable : MonoBehaviour, IInteractable
{
    public GameObject rumBottlePrefab;
    private int clickCount = 0;
    private int maxClicks = 2;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public bool RequiresCursorLock => false;

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        interactSuccessful = false;

        if (clickCount >= maxClicks) return;

        clickCount++;

        if (rumBottlePrefab != null)
        {
            Instantiate(rumBottlePrefab, transform.position + Vector3.up, Quaternion.identity);
        }

        if (clickCount >= maxClicks)
        {
            Destroy(gameObject);
        }

        interactSuccessful = true;
        OnInteractionComplete?.Invoke(this);
    }

    public void EndInteraction()
    {
        // není třeba řešit
    }
}
