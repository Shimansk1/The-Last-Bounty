using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] public string SceneToSwitch;

    [SerializeField] private Material defaultSkybox;

    private void Start()
    {
        ApplySkybox();
    }

    public void ChangeScene()
    {
        StartCoroutine(SwitchSceneWithSkybox());
    }

    IEnumerator SwitchSceneWithSkybox()
    {
        SceneManager.LoadScene(SceneToSwitch);

        yield return null;

        ApplySkybox();
    }

    void ApplySkybox()
    {
        if (defaultSkybox != null)
        {
            RenderSettings.skybox = defaultSkybox;
            DynamicGI.UpdateEnvironment();
        }
        else
        {
            Debug.LogWarning("Skybox material není pøiøazen!");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
