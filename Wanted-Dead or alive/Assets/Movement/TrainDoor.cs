using UnityEngine;

public class TrainDoor : MonoBehaviour
{
    [Header("Nastavení")]
    public TrainWagon myWagon;        // Odkaz na vagón
    public Transform onboardPosition; // Kam teleportovat

    private GameObject player;
    private bool isPlayerClose = false;

    void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            EnterTrain();
        }
    }

    void EnterTrain()
    {
        if (player == null) return;

        CharacterController cc = player.GetComponentInParent<CharacterController>();

        // 1. BLIK - Vypneme CC, aby šel hráè teleportovat
        if (cc != null) cc.enabled = false;

        // 2. Teleport
        Transform targetTransform = (cc != null) ? cc.transform : player.transform;
        targetTransform.position = onboardPosition.position;
        targetTransform.rotation = onboardPosition.rotation;

        // 3. BLIK - Hned ho zase ZAPNEME (jak jsi chtìl)
        if (cc != null) cc.enabled = true;

        // 4. Pøedáme hráèe vagónu (ten vypne jen PlayerMovementScript)
        if (myWagon != null)
        {
            myWagon.LockPlayer(player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            isPlayerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
            player = null;
        }
    }
}