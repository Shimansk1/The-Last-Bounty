using UnityEngine;

public class TrainDoor : MonoBehaviour
{
    [Header("Nastavení")]
    public TrainWagon myWagon;
    public Transform onboardPosition;

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

        if (cc != null) cc.enabled = false;

        Transform targetTransform = (cc != null) ? cc.transform : player.transform;
        targetTransform.position = onboardPosition.position;
        targetTransform.rotation = onboardPosition.rotation;

        if (cc != null) cc.enabled = true;

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