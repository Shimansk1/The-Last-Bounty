using UnityEngine;
using UnityEngine.Events;

public class HorseInteraction : MonoBehaviour, IInteractable
{
    [Header("Nastavení objektù")]
    public GameObject player;
    public GameObject horseCamera;
    public GameObject playerCamera;

    [Header("Pozice")]
    public Transform saddlePos;
    public Transform dismountPos;

    [Header("Skripty")]
    public HorseMovement horseMovement;
    public PlayerMovementScript playerScript;
    public CharacterController playerController;
    public PlayerHealth playerHealth;

    private bool isRiding = false;
    private float mountTime;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public bool RequiresCursorLock => false;

    void Start()
    {
        horseMovement.isMounted = false;
    }

    void Update()
    {
        if (isRiding && Input.GetKeyDown(KeyCode.E) && Time.time > mountTime + 1f)
        {
            Dismount();
        }
    }

    public void Interact(Interactor interactor, out bool interactSuccesful)
    {
        if (!isRiding)
        {
            Mount();
            interactSuccesful = true;
        }
        else
        {
            interactSuccesful = false;
        }

        OnInteractionComplete?.Invoke(this);
    }

    public void EndInteraction()
    {
    }

    void Mount()
    {
        isRiding = true;
        mountTime = Time.time;

        if (playerHealth != null)
        {
            playerHealth.isInvulnerable = true;
            playerHealth.ResetFallPosition();
        }

        playerScript.ResetVelocity();
        playerScript.enabled = false;
        playerController.enabled = false;

        player.transform.SetParent(transform);
        player.transform.position = saddlePos.position;
        player.transform.rotation = saddlePos.rotation;

        playerCamera.SetActive(false);
        horseCamera.SetActive(true);

        horseMovement.isMounted = true;
    }

    void Dismount()
    {
        isRiding = false;

        player.transform.SetParent(null);
        player.transform.position = dismountPos.position;
        player.transform.rotation = dismountPos.rotation;

        if (playerHealth != null)
        {
            playerHealth.isInvulnerable = false;
            playerHealth.ResetFallPosition();
        }

        playerScript.ResetVelocity();
        playerController.enabled = true;
        playerScript.enabled = true;

        horseCamera.SetActive(false);
        playerCamera.SetActive(true);

        horseMovement.isMounted = false;
    }
}