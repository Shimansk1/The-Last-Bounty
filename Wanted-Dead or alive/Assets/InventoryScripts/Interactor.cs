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
        }
        else
        {
            currentInteractable = null;
            if (promptUI != null) promptUI.Hide();
        }

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