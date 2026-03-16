using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MapManager : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject mapWindow;
    public TextMeshProUGUI mainQuestText;
    public TextMeshProUGUI sideQuestText;

    [Header("Ovládání")]
    [SerializeField] private MouseLook mouseLook;

    [Header("Map Rendering")]
    public Camera largeMapCamera;

    private bool isMapOpen = false;

    void Start()
    {
        if (mapWindow != null) mapWindow.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            ToggleMap();
        }

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
        largeMapCamera.enabled = true;
        UpdateQuestUI();

        if (mouseLook != null) mouseLook.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseMap()
    {
        mapWindow.SetActive(false);
        largeMapCamera.enabled = false;

        if (mouseLook != null) mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateQuestUI()
    {
        // 1. VEDLEJŠÍ ÚKOLY (Z tvého starého WantedQuestTrackeru)
        var tracker = WantedQuestTracker.Instance;
        if (tracker != null && tracker.ActiveSideContract != null)
        {
            WantedContract side = tracker.ActiveSideContract;
            sideQuestText.text = $"<color=white>{side.contractName}</color>\n" +
                                 $"<size=80%>{side.description}</size>\n" +
                                 $"<color=green>Odmìna: {side.reward} $</color>";
        }
        else
        {
            sideQuestText.text = "<color=grey>Žádný aktivní vedlejší kontrakt.</color>";
        }

        // 2. HLAVNÍ ÚKOL (Ze zbrusu nového MainStoryManageru)
        if (MainStoryManager.Instance != null)
        {
            string qName = MainStoryManager.Instance.GetQuestName();
            string qDesc = MainStoryManager.Instance.GetQuestDescription();

            mainQuestText.text = $"<color=yellow>{qName}</color>\n" +
                                 $"<size=80%>{qDesc}</size>\n" +
                                 $"<color=green>Hlavní pøíbìh</color>";
        }
        else
        {
            mainQuestText.text = "<color=grey>Pøíbìh není dostupný.</color>";
        }
    }
}