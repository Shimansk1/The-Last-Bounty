using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveGameManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private MouseLook mouseLook;

    private PlayerHealth playerHealth;
    private PlayerNeeds playerNeeds;
    private GameObject Player;

    public bool respawned = false;
    public static SaveData data;

    private void Awake()
    {
        data = new SaveData();
        SaveLoad.OnLoadGame += LoadData;
    }

    private void Update()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame) SaveData();
        if (Keyboard.current.f2Key.wasPressedThisFrame) TryLoadData();
        if (Keyboard.current.f3Key.wasPressedThisFrame) DeleteData();
    }

    public static void SaveData()
    {
        SaveLoad.Save(data);
    }

    public static void LoadData(SaveData _data)
    {
        data = _data;
    }

    public static void TryLoadData()
    {
        SaveLoad.Load();
    }

    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    public void Respawn()
    {
        deathScreen.SetActive(false);
        Debug.Log("?? Hráè byl respawnut!");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player = playerObj;
            playerHealth = Player.GetComponent<PlayerHealth>();
            playerNeeds = Player.GetComponent<PlayerNeeds>();

            if (playerHealth != null && playerNeeds != null)
            {
                playerHealth.currentHealth = 100;
                playerNeeds.currentHunger = 100;
                playerNeeds.currentThirst = 100;
                playerHealth.isDead = false;

                // Nejprve deaktivuj skript pohybu
                playerHealth.playerMovementScript.enabled = false;

                // ? Posuò hráèe o nìco výš, aby nepropadl
                Player.transform.position = new Vector3(-82.21f, -3.0f, 50.64f);

                // Až pak znovu aktivuj pohyb
                Invoke(nameof(EnableMovement), 0.1f);
            }
            else
            {
                Debug.LogError("? Chybí komponenty PlayerHealth nebo PlayerNeeds!");
            }
        }
        else
        {
            Debug.LogError("? Objekt s tagem 'Player' nebyl nalezen!");
        }

        respawned = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseLook.canMove = true;
    }

    private void EnableMovement()
    {
        if (playerHealth != null)
        {
            playerHealth.playerMovementScript.enabled = true;
        }
    }
}
