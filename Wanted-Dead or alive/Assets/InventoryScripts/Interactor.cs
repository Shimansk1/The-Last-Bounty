using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform InteractionPoint;
    public LayerMask InteractionLayer;
    public float InteractionPointRadius = 1f;

    [Header("UI Reference")]
    public InteractionPromptUI promptUI;

    public bool IsInteracting { get; private set; }
    [SerializeField] private MouseLook mouseLook;

    private IInteractable currentInteractable;

    private void Update()
    {
        // Pokud jsme v UI panelu, èekáme jen na Escape
        if (IsInteracting)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                EndInteraction();
            }
            return;
        }

        var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);

        IInteractable closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            var interactable = col.GetComponentInParent<IInteractable>();
            if (interactable != null)
            {
                float dist = Vector3.Distance(InteractionPoint.position, col.transform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable != null)
        {
            currentInteractable = closestInteractable;
            var interactableObject = closestInteractable as MonoBehaviour;

            if (interactableObject != null && promptUI != null)
            {
                promptUI.Show(interactableObject.transform);
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                StartInteraction(currentInteractable);
            }
        }
        else
        {
            currentInteractable = null;
            if (promptUI != null) promptUI.Hide();
        }
    }

    void StartInteraction(IInteractable interactable)
    {
        // Schováme 'E' pøi startu jakékoliv interakce
        if (promptUI != null) promptUI.Hide();

        // Provedeme samotnou interakci
        interactable.Interact(this, out bool interactSuccesful);

        TutorialManager tutorial = FindObjectOfType<TutorialManager>();
        if (tutorial != null) tutorial.MarkStepComplete("openChest");

        // ROZHODNUTÍ: Je to panel (vyžaduje kurzor), nebo instantní akce?
        if (interactable.RequiresCursorLock)
        {
            // Jde o panel/inventáø -> Zablokujeme hráèe a ukážeme kurzor
            IsInteracting = true;
            if (mouseLook != null) mouseLook.canMove = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Jde o studnu, konì atd. -> Necháme IsInteracting na false!
            // Hráè mùže hned hrát dál a maèkat E znova.
        }
    }

    public void EndInteraction()
    {
        IsInteracting = false;
        Cursor.visible = false;
        if (mouseLook != null) mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}