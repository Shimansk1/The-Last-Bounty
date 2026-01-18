using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MapManager : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject mapWindow;           // Celý panel mapy
    public TextMeshProUGUI mainQuestText;  // Text pro hlavní úkol
    public TextMeshProUGUI sideQuestText;  // Text pro vedlejší úkol

    [Header("Ovládání")]
    [SerializeField] private MouseLook mouseLook; // Odkaz na tvùj skript pro kameru

    private bool isMapOpen = false;

    void Start()
    {
        // Zajistíme, že mapa je na zaèátku zavøená
        if (mapWindow != null) mapWindow.SetActive(false);
    }

    void Update()
    {
        // Otevírání na klávesu M
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            ToggleMap();
        }

        // Zavírání pøes ESC (pokud je mapa otevøená)
        if (isMapOpen && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleMap();
        }
    }

    public void ToggleMap()
    {
        isMapOpen = !isMapOpen;

        if (isMapOpen)
        {
            OpenMap();
        }
        else
        {
            CloseMap();
        }
    }

    void OpenMap()
    {
        mapWindow.SetActive(true);
        UpdateQuestUI(); // Tady naèteme data z trackeru

        // Odemknout myš a zastavit kameru
        if (mouseLook != null) mouseLook.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Zastavit èas (volitelné, u mapy se to èasto dìlá)
        // Time.timeScale = 0f; 
    }

    void CloseMap()
    {
        mapWindow.SetActive(false);

        // Zamknout myš a povolit kameru
        if (mouseLook != null) mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Obnovit èas
        // Time.timeScale = 1f;
    }

    // Tuhle funkci voláme vždy pøi otevøení mapy
    public void UpdateQuestUI()
    {
        // Získáme instanci trackeru
        var tracker = WantedQuestTracker.Instance;
        if (tracker == null) return;

        // --- 1. HLAVNÍ KONTRAKT ---
        if (tracker.ActiveMainContract != null)
        {
            WantedContract main = tracker.ActiveMainContract;
            mainQuestText.text = $"<color=yellow>{main.contractName}</color>\n" +
                                 $"<size=80%>{main.description}</size>\n" +
                                 $"<color=green>Odmìna: {main.reward} $</color>";
        }
        else
        {
            mainQuestText.text = "<color=grey>Žádný aktivní hlavní kontrakt.</color>";
        }

        // --- 2. VEDLEJŠÍ KONTRAKT ---
        if (tracker.ActiveSideContract != null)
        {
            WantedContract side = tracker.ActiveSideContract;
            sideQuestText.text = $"<color=white>{side.contractName}</color>\n" +
                                 $"<size=80%>{side.description}</size>\n" +
                                 $"<color=green>Odmìna: {side.reward} $</color>";
        }
        else
        {
            sideQuestText.text = "<color=grey>Žádný vedlejší kontrakt.</color>";
        }
    }
}