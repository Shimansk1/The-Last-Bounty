using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WantedPoster : MonoBehaviour, IInteractable
{
    [Header("Nastavení")]
    public WantedContract contract;
    public float respawnTime = 30f;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public bool RequiresCursorLock => false;

    private Collider myCollider;
    private Renderer myRenderer;

    private void Awake()
    {
        myCollider = GetComponent<Collider>();
        myRenderer = GetComponent<Renderer>();
    }

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        interactSuccesful = false;

        if (WantedQuestTracker.Instance == null || contract == null) return;

        bool accepted = WantedQuestTracker.Instance.TryAcceptContract(contract);

        if (accepted)
        {
            interactSuccesful = true;
            StartCoroutine(RespawnRoutine());
        }

        OnInteractionComplete?.Invoke(this);
    }

    public void EndInteraction()
    {
    }

    private IEnumerator RespawnRoutine()
    {
        TogglePoster(false);

        yield return new WaitForSeconds(respawnTime);

        TogglePoster(true);
    }

    private void TogglePoster(bool state)
    {
        if (myCollider != null) myCollider.enabled = state;
        if (myRenderer != null) myRenderer.enabled = state;

        // Pokud má plakát texty nebo obrázky jako dìti, vypneme je taky
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}