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
        UpdateQuestUI();

        if (mouseLook != null) mouseLook.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseMap()
    {
        mapWindow.SetActive(false);

        if (mouseLook != null) mouseLook.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateQuestUI()
    {
        var tracker = WantedQuestTracker.Instance;
        if (tracker == null) return;

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