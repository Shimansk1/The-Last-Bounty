using UnityEngine;
using UnityEngine.UI;

public class SettingsOpener : MonoBehaviour
{
    [Header("Reference")]
    public GameObject settingsPanel;
    public Button openSettingsButton;
    public Button closeSettingsButton;

    private void Start()
    {
        settingsPanel.SetActive(false);

        openSettingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
    }
    void Update()
    {
        if (settingsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSettings();
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
