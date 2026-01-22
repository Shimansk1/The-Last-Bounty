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

    private bool isRiding = false;

    // NOVÉ: Promìnná pro èas, kdy jsme nasedli
    private float mountTime;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public bool RequiresCursorLock => false;

    void Start()
    {
        horseMovement.isMounted = false;
    }

    void Update()
    {
        // NOVÉ: Pøidána podmínka (Time.time > mountTime + 1f)
        // Znamená to: "Pokud uplynula aspoò 1 vteøina od nasednutí"
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

        // NOVÉ: Uložíme si aktuální èas nasednutí
        mountTime = Time.time;

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

        playerController.enabled = true;
        playerScript.enabled = true;

        horseCamera.SetActive(false);
        playerCamera.SetActive(true);

        horseMovement.isMounted = false;
    }
}