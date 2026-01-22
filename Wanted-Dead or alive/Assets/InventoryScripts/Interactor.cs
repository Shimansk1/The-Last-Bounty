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
    public InteractionPromptUI promptUI; // Sem pøetáhneš ten nový UI skript

    public bool IsInteracting { get; private set; }
    [SerializeField] private MouseLook mouseLook;

    // Pomocná promìnná pro nalezený interactable
    private IInteractable currentInteractable;

    private void Update()
    {
        // 1. NEUSTÁLÉ HLEDÁNÍ (Každý frame)
        // Najdeme všechny collidery v okolí
        var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);
        
        // Najdeme ten NEJBLIŽŠÍ (aby se UI nepralo, když je víc vìcí u sebe)
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

        // 2. AKTUALIZACE UI (Zobrazit nebo Schovat)
        if (closestInteractable != null)
        {
            currentInteractable = closestInteractable;
            
            // Zobrazíme prompt. Musíme získat Transform objektu.
            // Protože IInteractable je interface, musíme to pøetypovat na Component nebo MonoBehaviour
            var interactableObject = closestInteractable as MonoBehaviour; 
            if (interactableObject != null && promptUI != null)
            {
                promptUI.Show(interactableObject.transform);
            }
        }
        else
        {
            currentInteractable = null;
            if (promptUI != null) promptUI.Hide();
        }

        // 3. VSTUP (INPUT) - Samotná interakce
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentInteractable != null)
            {
                if (currentInteractable.RequiresCursorLock)
                {
                    mouseLook.canMove = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }

                StartInteraction(currentInteractable);
            }
        }

        // Zavírání interakce
        if (Keyboard.current.escapeKey.wasPressedThisFrame && IsInteracting)
        {
            EndInteraction();
        }
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccesful);
        IsInteracting = true;

        TutorialManager tutorial = FindObjectOfType<TutorialManager>();
        if (tutorial != null)
            tutorial.MarkStepComplete("openChest");
            
        // Když interagujeme, možná chceme prompt schovat?
        if (promptUI != null) promptUI.Hide();
    }

    void EndInteraction()
    {
        IsInteracting = false;
        Cursor.visible = false;
        mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}