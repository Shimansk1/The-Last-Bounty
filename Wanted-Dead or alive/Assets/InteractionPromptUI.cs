using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject uiPanel; // Celý panel s ikonkou 'E'
    public TextMeshProUGUI actionText; // Volitelné: Text jako "Sebrat", "Otevøít"

    private Camera mainCam;
    private Transform targetTransform;
    public Vector3 offset = new Vector3(0, 0.5f, 0); // O kolik výš nad objekt to má být

    private void Start()
    {
        mainCam = Camera.main;
        uiPanel.SetActive(false); // Na zaèátku schovat
    }

    private void LateUpdate() // LateUpdate je lepší pro UI, aby se netøáslo
    {
        if (targetTransform != null && uiPanel.activeSelf)
        {
            // Pøevod 3D pozice svìta na 2D pozici obrazovky
            Vector3 screenPos = mainCam.WorldToScreenPoint(targetTransform.position + offset);
            transform.position = screenPos;

            // Pojistka: Pokud je objekt za kamerou, schováme UI
            if (screenPos.z < 0)
            {
                uiPanel.SetActive(false);
            }
        }
    }

    public void Show(Transform target, string text = "Interact")
    {
        targetTransform = target;
        if (actionText != null) actionText.text = text;

        uiPanel.SetActive(true);
    }

    public void Hide()
    {
        uiPanel.SetActive(false);
        targetTransform = null;
    }
}