using UnityEngine;

public class HorseInteraction : MonoBehaviour
{
    [Header("Nastavení objektù")]
    public GameObject player;             // Tvùj hráè
    public GameObject horseCamera;        // Kamera na koni
    public GameObject playerCamera;       // Kamera hráèe (MainCamera uvnitø hráèe)

    [Header("Pozice")]
    public Transform saddlePos;           // Kde hráè sedí
    public Transform dismountPos;         // Kde hráè seskoèí

    [Header("Skripty")]
    public HorseMovement horseMovement;   // Skript pohybu konì
    public PlayerMovementScript playerScript; // Tvùj skript pohybu hráèe
    public CharacterController playerController; // Abychom mohli vypnout kolize hráèe

    private bool isPlayerClose = false;
    private bool isRiding = false;

    void Start()
    {
        // Na zaèátku vypneme ovládání konì
        horseMovement.isMounted = false;
    }

    void Update()
    {
        // Pokud zmáèkneš E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isRiding)
            {
                Dismount();
            }
            else if (isPlayerClose)
            {
                Mount();
            }
        }
    }

    void Mount()
    {
        isRiding = true;

        // 1. Vypneme ovládání a kolize hráèe
        playerScript.enabled = false;
        playerController.enabled = false; // Dùležité! Jinak se CharacterController pere s pozicí

        // 2. Pøilepíme hráèe ke koni
        player.transform.SetParent(transform);
        player.transform.position = saddlePos.position;
        player.transform.rotation = saddlePos.rotation;

        // 3. Pøepneme kamery
        playerCamera.SetActive(false);
        horseCamera.SetActive(true);

        // 4. Zapneme ovládání konì
        horseMovement.isMounted = true;
    }

    void Dismount()
    {
        isRiding = false;

        // 1. Odlepíme hráèe
        player.transform.SetParent(null);
        player.transform.position = dismountPos.position;
        player.transform.rotation = dismountPos.rotation; // Aby stál rovnì

        // 2. Zapneme ovládání hráèe
        playerController.enabled = true;
        playerScript.enabled = true;

        // 3. Pøepneme kamery zpìt
        horseCamera.SetActive(false);
        playerCamera.SetActive(true);

        // 4. Vypneme ovládání konì
        horseMovement.isMounted = false;
    }

    // Detekce, jestli je hráè u konì
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerClose = true;
            Debug.Log("Jsi u kone, zmackni E"); // Pro kontrolu
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerClose = false;
        }
    }
}